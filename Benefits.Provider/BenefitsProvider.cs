using Benefits.Entities;
using Benefits.Shared;
using Knights.Core.Common;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Provider
{
    public class BenefitsProvider
    {
        public BenefitsProvider(Guid userId)
        {
            this.UserId = userId;

            using (var db = new BenefitsDbContext())
            {
                var options = db.Options.FirstOrDefault();
                if (options == null)
                {
                    options = new BOptions { Id = Guid.Empty, LastContractNumber = 1000, };
                    db.Options.Add(options);
                    db.SaveChanges();
                }
            }
        }

        public Guid UserId { get; }

        public void CreateMembership(BMembership membership)
        {
            // Create a single transaction to ensure everything saves, or nothing changes.
            using (var db = new BenefitsDbContext())
            {
                var options = db.Options.First();

                // var agent = db.People.Find(membership.AgentId);

                // We increment the contract number in Options, and assign it to this contract
                // If this transaction fails, the LastContractNumber will not be changed and our
                // in memory instance of Options will be dropped.
                var number = ++(options.LastContractNumber);

                DateTime createOn = Clock.Now;

                var m = new BMembership
                {
                    Id = membership.Id,
                    AgentId = membership.AgentId,
                    CreatedById = UserId,
                    CreatedOn = createOn,
                    InceptionDate = membership.InceptionDate,
                    SignDate = membership.SignDate,
                    Number = number,
                    RowVersion = 1,
                    WorkflowStatus = WorkflowStatuses.New,
                };
                db.Memberships.Add(m);

                foreach (var dependency in membership.Dependencies)
                {
                    //if (dependency.Person == null) throw new BenefitsException();
                    //if (dependency.Membership == null) throw new BenefitsException();

                    //if (dependency.PersonId == null) dependency.PersonId = dependency.Person.Id;
                    //if (dependency.MembershipId == null) dependency.MembershipId = dependency.Membership.Id;

                    var person = dependency.Person;
                    var p = new BPerson
                    {
                        Id = person.Id,
                        CreatedById = UserId,
                        CreatedOn = createOn,
                        DateOfBirth = person.DateOfBirth,
                        DateOfDeath = person.DateOfDeath,
                        Gender = person.Gender,
                        IdentityNumber = person.IdentityNumber,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        RowVersion = 1,
                        WorkflowStatus = WorkflowStatuses.New,
                    };

                    var d = new BMembershipDependency
                    {
                        Membership = m,
                        Person = p,
                        Type = dependency.Type,
                    };
                    m.Dependencies.Add(d);
                }

                //var a = new UserAction();

                db.SaveChanges();

                // Update the in-memory model with the changes made.
                membership.CreatedById = m.CreatedById;
                membership.CreatedOn = m.CreatedOn;
                membership.Number = m.Number;
                foreach (var dependency in membership.Dependencies)
                {
                    dependency.MembershipId = dependency.Membership.Id;
                    dependency.PersonId = dependency.Person.Id;
                }
            }
        }

        public void UpdatePerson(BPerson person)
        {
            using (var db = new BenefitsDbContext())
            {
                var p = db.People.Find(person.Id);
                if (p == null)
                    throw new BenefitsException($"{person.Err} does not exist and cannot be updated.");

                if (p.RowVersion != person.RowVersion)
                    throw new BenefitsException($"{person.Err} has been modified by another user.");
                p.RowVersion++;

                SetPropertiesAndAudit(db, person, p, BAuditAction.Update);

                db.SaveChanges();
            }
        }

        private String SetPropertiesAndAudit(BenefitsDbContext db, BPerson person, BPerson p, BAuditAction action)
        {
            var msg = "";

            if (p.CellPhone != person.CellPhone)
            { msg += $"CellPhone='{person.CellPhone}' "; p.CellPhone = person.CellPhone; }

            if (p.DateOfBirth != person.DateOfBirth)
            { msg += $"DateOfBirth='{person.DateOfBirth:yyyy-mm-dd}'"; p.DateOfBirth = person.DateOfBirth; }

            if (p.DateOfDeath != person.DateOfDeath)
            { msg += $"DateOfDeath='{person.DateOfDeath:yyyy-mm-dd}'"; p.DateOfDeath = person.DateOfDeath; }

            // TODO: audit
            if (p.EmailAddress != person.EmailAddress) { p.EmailAddress = person.EmailAddress; }

            if (p.EmployedAt != person.EmployedAt) { p.EmployedAt = person.EmployedAt; }
            if (p.EmployedAtPhone != person.EmployedAtPhone) { p.EmployedAtPhone = person.EmployedAtPhone; }
            if (p.FirstName != person.FirstName) { p.FirstName = person.FirstName; }
            if (p.HomePhone != person.HomePhone) { p.HomePhone = person.HomePhone; }
            if (p.LastName != person.LastName) { p.LastName = person.LastName; }
            if (p.WorkPhone != person.WorkPhone) { p.WorkPhone = person.WorkPhone; }

            var audit = new BAudit
            {
                Action = action,
                EntityId = p.Id,
                EntityType = BEntityType.Person,
                UserId = UserId,
                When = Clock.Now,
                Description = msg,
            };
            db.Audits.Add(audit);

            return msg;
        }

        public void CreatePerson(BPerson person)
        {
            using (var db = new BenefitsDbContext())
            {
                var p = new BPerson
                {
                    Id = person.Id,
                    CreatedById = UserId,
                    CreatedOn = DateTime.Now,
                    RowVersion = 1,
                };

                var msg = SetPropertiesAndAudit(db, person, p, action: BAuditAction.Create);

                db.People.Add(p);

                db.SaveChanges();
            }
        }

        public void SubmitMembership(Guid id)
        {
            using (var db = new BenefitsDbContext())
            {
                var membership = db.GetMembership(id);

                ChangeStatus(membership, WorkflowStatuses.New, WorkflowStatuses.Pending);

                // TODO: audit
                var msg = $"{UserId} submitted membership {id}.";
                db.SaveChanges();
            }
        }

        public void ApproveMembership(Guid id)
        {
            using (var db = new BenefitsDbContext())
            {
                var membership = db.Memberships.Find(id);

                ChangeStatus(membership, WorkflowStatuses.Pending, WorkflowStatuses.Approved);
                // TODO: audit
                var msg = $"{UserId} approved membership {id}.";
                db.SaveChanges();
            }
        }

        private void ChangeStatus(BMembership membership, WorkflowStatuses currentStatus, WorkflowStatuses newStatus)
        {
            if (membership.Errors.Count != 0)
                throw new BenefitsException($"Membership {membership.Number} has errors.");

            if (membership.WorkflowStatus != currentStatus)
                throw new BenefitsException($"Membership {membership.Number} is not {currentStatus}.");

            membership.WorkflowStatus = newStatus;
            membership.WorkflowByUserId = UserId;
            membership.WorkflowOn = Clock.Now;
        }

        public IList<BMembership> ListMemberships(
            WorkflowStatuses? status = null,
            bool? isValid = null)
        {
            using (var db = new BenefitsDbContext())
            {
                var q = db.Memberships
                    .Include(nameof(BMembership.Dependencies))
                    .Include(m => m.Dependencies.Select(d => d.Person))
                    .Include(nameof(BMembership.Agent))
                    .AsQueryable();

                if (status != null)
                    q = q.Where(m => m.WorkflowStatus == status);

                if (isValid != null)
                    q = q.Where(m => m.IsValid == isValid);

                return q.ToList();
            }
        }
    }
}