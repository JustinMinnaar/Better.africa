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
        }

        public Guid UserId { get; }

        public void CreateMembership(Membership membership)
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

                var m = new Membership
                {
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

                foreach (var person in membership.People)
                {
                    var p = new Person
                    {
                        CreatedById = UserId,
                        CreatedOn = createOn,
                        DateOfBirth = person.DateOfBirth,
                        DateOfDeath = person.DateOfDeath,
                        Gender = person.Gender,
                        Identity = person.Identity,
                        IsScholar = person.IsScholar,
                        Membership = m,
                        MembershipType = person.MembershipType,
                        NameFirst = person.NameFirst,
                        NameLast = person.NameLast,
                        RowVersion = 1,
                        WorkflowStatus = WorkflowStatuses.New,
                    };
                    m.People.Add(p);
                }

                //var a = new UserAction();

                db.SaveChanges();

                // Update the in-memory model with the changes made.
                membership.CreatedById = m.CreatedById;
                membership.CreatedOn = m.CreatedOn;
                membership.Number = m.Number;
            }
        }

        public void SubmitMembership(Guid id)
        {
            using (var db = new BenefitsDbContext())
            {
                var membership = db.Memberships.Find(id);
                ChangeStatus(membership, WorkflowStatuses.New, WorkflowStatuses.Pending);
                // TODO: audit
                var msg = $"{UserId} submitted membership {id}.";
                db.SaveChanges();
            }
        }

        private void ChangeStatus(Membership membership, WorkflowStatuses currentStatus, WorkflowStatuses newStatus)
        {
            if (membership.Errors.Count != 0)
                throw new BenefitsException($"Membership {membership.Number} has errors.");

            if (membership.WorkflowStatus != currentStatus)
                throw new BenefitsException($"Membership {membership.Number} is not {currentStatus}.");

            membership.WorkflowStatus = newStatus;
            membership.WorkflowByUserId = UserId;
            membership.WorkflowOn = Clock.Now;
        }

        public IEnumerable<Membership> ListMembershipsWithErrors()
        {
            using (var db = new BenefitsDbContext())
            {
                return db.Memberships.Where(m => !m.IsValid);
            }
        }
    }
}