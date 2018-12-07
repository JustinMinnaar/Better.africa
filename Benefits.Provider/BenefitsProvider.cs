using BetterAfrica.Shared;
using Knights.Core.Common;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using BetterAfrica.Benefits.Entities;
using BetterAfrica.Benefits.Entities.Forms;

namespace Benefits.Provider
{
    public class BenefitsProvider
    {
        public BenefitsProvider(Guid userId)
        {
            this.UserId = userId;

            using (var db = new BenefitsDbContext())
            {
                var options = db.Options.AsNoTracking().FirstOrDefault();
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
                    m.Dependencies.Add(d); // also adds the person record
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

        public FormResult ProcessForm(FormMembership app)
        {
            // Create a single transaction to ensure everything saves, or nothing changes.
            using (var db = new BenefitsDbContext())
            {
                // We increment the contract number in Options, and assign it to this contract
                // If this transaction fails, the LastContractNumber will not be changed and our
                // in memory instance of Options will be dropped.
                var options = db.Options.First();
                var number = ++(options.LastContractNumber);

                DateTime createOn = Clock.Now;

                var result = ProcessForm(db, app);
                return result.Membership;
            }
        }

        private FormResult ProcessForm(BenefitsDbContext db, FormMembership form)
        {
            var agent = LocateAgent(db: db, agentCode: form.Detail.AgentCode);

            var formType = form.Detail.Action.Value;
            var principal = FormToPerson(db, form.Principal, formType);

            var memberships = FindMemberships(identityNumber: form.Principal.IdentityNumber);
            var membershipsCount = memberships.Count();
            if (formType != EFormAction.Add)
            {
                if (membershipsCount != 1)
                    throw new BenefitsException($"Membership not found for principal with identity number {form.Principal.IdentityNumber}!");
            }
            else
            {
                if (membershipsCount > 1)
                    throw new BenefitsException($"{membershipsCount} memberships found for principal with identity number {form.Principal.IdentityNumber}!");
            }

            DateTime createOn = Clock.Now;

            var membership = memberships.First();
            var m = new BMembership
            {
                Id = membership.Id,
                AgentId = membership.AgentId,
                CreatedById = UserId,
                CreatedOn = createOn,
                InceptionDate = membership.InceptionDate,
                SignDate = membership.SignDate,
                Number = membership.Number,
                RowVersion = 1,
                WorkflowStatus = WorkflowStatuses.New,
            };
            db.Memberships.Add(m);

            return new FormResult { Message = "" };
        }

        private BPerson LocateAgent(BenefitsDbContext db, string agentCode)
        {
            var agent = db.People.FirstOrDefault(p => p.Code == agentCode);
            if (agent == null) throw new BenefitsException($"Cannot find agent '{agentCode}'!");
            return agent;
        }

        public IEnumerable<BMembership> FindMemberships(string name = null, string identityNumber = null)
        {
            using (var db = new BenefitsDbContext())
            {
                var q = db.Memberships.AsQueryable();
                if (name != null) q = q.Where(p => p.PeoplePrincipal.Name.Contains(name));
                if (identityNumber != null) q = q.Where(p => p.PeoplePrincipal.IdentityNumber.Contains(identityNumber));
                q = q.OrderBy(p => p.CreatedOn);
                return q;
            }
        }

        private BPerson FormToPerson(BenefitsDbContext db, FormMembershipPerson form, EFormAction type)
        {
            var people = FindPeople(identityNumber: form.IdentityNumber);

            var p = new BPerson();
            switch (type)
            {
                case EFormAction.Add:
                    if (people.Count() > 0)
                        throw new BenefitsException($"Person '{form.FirstName}{form.LastName}' already exists with {form.IdentityNumber}!");
                    break;

                case EFormAction.Update:
                case EFormAction.Delete:
                    if (people.Count() != 1)
                        throw new BenefitsException($"Person '{form.FirstName}{form.LastName}' matched {people.Count()} people with identity number {form.IdentityNumber}!");
                    break;

                default:
                    throw new BenefitsException($"Unknown form type {type}!");
            }

            var msg = "<person ";
            p = people.FirstOrDefault();
            if (p.CellPhone != form.CellPhone) { msg += $"CellPhone=[{p.CellPhone}] "; p.CellPhone = form.CellPhone; p.CellPhoneDial = form.CellPhone.ToDigitsOnly(); }
            if (p.CreatedById == null) { p.CreatedById = UserId; msg += $"CreatedById=[{p.CreatedById}] "; }
            if (p.CreatedOn == null) { p.CreatedOn = DateTime.Now; msg += $"CreatedOn=[{p.CreatedOn}] "; }
            if (p.DateOfBirth != form.DateOfBirth) { p.DateOfBirth = form.DateOfBirth; msg += $"DateOfBirth=[{p.DateOfBirth:yyyy-mm-dd}] "; }
            if (p.DateOfDeath != form.DateOfDeath) { p.DateOfDeath = form.DateOfDeath; msg += $"DateOfDeath=[{p.DateOfDeath:yyyy-mm-dd}] "; }
            if (p.EmailAddress != form.EmailAddress) { msg += "EmailAddress=" + p.EmailAddress; p.EmailAddress = form.EmailAddress; }
            if (p.Work != form.WorkName) { p.Work = form.WorkName; msg += $"Work=[{p.Work}]"; }
            if (p.WorkPhone != form.WorkPhone) { p.WorkPhone = form.WorkPhone; msg += $"WorkPhone=[{p.WorkPhone}] "; }
            if (p.FirstName != form.FirstName) { p.FirstName = form.FirstName; msg += $"FirstName=[{p.FirstName}] "; }
            if (p.Gender != form.Gender) { p.Gender = form.Gender; msg += $"Gender=[{p.Gender}] "; }
            if (p.HomePhone != form.HomePhone) { p.HomePhone = form.HomePhone; p.HomePhoneDial = form.HomePhone.ToDigitsOnly(); msg += $"HomePhone=[{p.HomePhone}] "; }
            if (p.IdentityNumber != form.IdentityNumber) { p.IdentityNumber = form.IdentityNumber; msg += $"IdentityNumber=[{p.IdentityNumber}] "; }
            if (p.LastName != form.LastName) { p.LastName = form.LastName; msg += $"LastName=[{p.LastName}]"; }
            if (p.WorkPhone != form.WorkPhone) { p.WorkPhone = form.WorkPhone; p.WorkPhoneDial = form.WorkPhone; msg += $"WorkPhone=[{p.WorkPhone}]"; }
            msg += " />";

            // convert to xml format for debug
            msg = msg.Replace("'", "&quot;").Replace("[", "'").Replace("]", "'");

            p.RowVersion++;

            return p;
        }

        public IEnumerable<BPerson> FindPeople(string name = null, string identityNumber = null)
        {
            using (var db = new BenefitsDbContext())
            {
                return FindPeople(db, name, identityNumber);
            }
        }

        private static IEnumerable<BPerson> FindPeople(BenefitsDbContext db, string name = null, string identityNumber = null)
        {
            var q = db.People.AsQueryable();
            if (name != null) q = q.Where(p => p.Name.Contains(name));
            if (identityNumber != null) q = q.Where(p => p.IdentityNumber.Contains(identityNumber));
            q = q.OrderBy(p => p.IdentityNumber);
            return q;
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
            { msg += $"CellPhone='{p.CellPhone}' "; p.CellPhone = person.CellPhone; }

            if (p.DateOfBirth != person.DateOfBirth)
            { msg += $"DateOfBirth='{p.DateOfBirth:yyyy-mm-dd}'"; p.DateOfBirth = person.DateOfBirth; }

            if (p.DateOfDeath != person.DateOfDeath)
            { msg += $"DateOfDeath='{p.DateOfDeath:yyyy-mm-dd}'"; p.DateOfDeath = person.DateOfDeath; }

            if (p.EmailAddress != person.EmailAddress)
            {
                msg += "EmailAddress=" + p.EmailAddress; p.EmailAddress = person.EmailAddress;
            }

            // TODO: audit
            if (p.Work != person.Work) { if (p.Work != null) msg += $"Work={p.Work}"; p.Work = person.Work; }
            if (p.WorkPhone != person.WorkPhone) { msg += $"{p.WorkPhone}"; p.WorkPhone = person.WorkPhone; }
            if (p.FirstName != person.FirstName) { if (p.FirstName != null) msg += $"{p.FirstName}"; p.FirstName = person.FirstName; }
            if (p.HomePhone != person.HomePhone) { if (p.HomePhone != null) msg += $"{p.FirstName}"; msg += $"{p.HomePhone}"; p.HomePhone = person.HomePhone; }
            if (p.LastName != person.LastName) { if (p.LastName != null) msg += $"{p.LastName}"; p.LastName = person.LastName; }
            if (p.WorkPhone != person.WorkPhone) { if (p.WorkPhone != null) msg += $"{p.WorkPhone}"; p.WorkPhone = person.WorkPhone; }

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