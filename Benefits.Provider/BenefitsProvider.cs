using Benefits.Entities;
using Benefits.Shared;
using Knights.Core.Common;
using System;
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
                        NameFirst = person.NameFirst,
                        NameLast = person.NameLast,
                        RowVersion = 1,
                        WorkflowStatus = WorkflowStatuses.New,
                    };
                    db.People.Add(person);

                    var d = new BMembershipDependency
                    {
                        Membership = m,
                        Person = p
                    };
                    m.Dependencies.Add(d);
                }

                //var a = new UserAction();

                db.SaveChanges();

                // Update the in-memory model with the changes made.
                membership.CreatedById = m.CreatedById;
                membership.CreatedOn = m.CreatedOn;
                membership.Number = m.Number;
            }
        }

        public void CreatePerson(BPerson person)
        {
            using (var db = new BenefitsDbContext())
            {
                db.People.Add(person);
                db.SaveChanges();
            }
        }

        public void SubmitMembership(Guid id)
        {
            using (var db = new BenefitsDbContext())
            {
                var membership = db.Memberships.Find(id);
                //var agent = db.People.Find(membership.AgentId);

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