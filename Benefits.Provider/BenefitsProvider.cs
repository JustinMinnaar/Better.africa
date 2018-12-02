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

        public void CreateMembership(MembershipModel model)
        {
            // Create a single transaction to ensure everything saves, or nothing changes.
            using (var db = new BenefitsDbContext())
            {
                var options = db.Options.First();

                var agent = db.People.Find(model.AgentId);
                if (agent == null)
                    throw new BenefitsException("Membership requires an agent be assigned!");

                // We increment the contract number in Options, and assign it to this contract
                // If this transaction fails, the LastContractNumber will not be changed and our
                // in memory instance of Options will be dropped.
                var number = ++(options.LastContractNumber);

                DateTime createOn = Clock.Now;

                var m = new Membership
                {
                    AgentId = model.AgentId,
                    CreatedById = UserId,
                    CreatedOn = createOn,
                    InceptionDate = model.InceptionDate,
                    SignDate = model.SignDate,
                    Number = number,
                    RowVersion = 1,
                    WorkflowStatus = WorkflowStatuses.New,
                };
                db.Memberships.Add(m);

                foreach (var person in model.People)
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
                model.CreatedById = m.CreatedById;
                model.CreatedOn = m.CreatedOn;
                model.Number = m.Number;
            }
        }
    }
}