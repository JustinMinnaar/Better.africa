using Benefits.Provider;
using BetterAfrica.Benefits.Entities;
using BetterAfrica.Benefits.Entities.Forms;
using BetterAfrica.Benefits.Entities.UnitTests;
using BetterAfrica.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benefits.Demo
{
    public class Demo : BaseValidationUnitTests
    {
        public Demo()
        {
            var bp = new BenefitsProvider(Guid.NewGuid());

            var forms = FormMemberships.FromXmlFile(xmlPath: "Memberships.xml").ToList();
            foreach (var form in forms)
            {
                Output(form.Err);

                // First validate the proposed changes, then create or update
                // membership, people, policies, etc. otherwise output errors
                var result = bp.ProcessForm(form);

                Output(result.Message);
            }

            // We can create memberships even if there are errors in the data
            // We can save these to the database, even though they contain errors.
            // This allows editing the data until correct, then submitting it

            var agent = new BPerson
            {
                FirstName = "Luke",
                LastName = "Griqua",
                IdentityNumber = "9503145173088",
                Gender = EPersonGenders.Male
            };

            bp.CreatePerson(agent);

            agent.CellPhone = "x";
            bp.UpdatePerson(agent);

            var m1 = new BMembership()
               .WithAgent(agent)
               .WithSignDate(2018, 12, 01)
               .WithInceptionDate(2019, 02)
               .WithPrincipal(p1Adam49)
               .WithSpouse(p2Bertha47)
               .WithChildren(p3Charles11)
               .WithPerson(p5Eddie27);

            if (!m1.IsValid) throw new BenefitsException();

            bp.CreateMembership(m1);

            var m2 = new BMembership()
               .WithSignDate(2018, 12, 2)
               .WithInceptionDate(2020, 1)
               .WithSpouse(p2Bertha47);

            bp.CreateMembership(m2);

            var m3 = new BMembership()
               .WithPrincipal(p2Bertha47)
               .WithSpouse(p1Adam49);

            bp.CreateMembership(m3);

            var allMemberships = bp.ListMemberships();

            Output("allMemberships", allMemberships);

            // We can list all memberships with errors
            var membershipsWithErrors = bp.ListMemberships(isValid: true);

            Output("membershipsWithErrors", membershipsWithErrors);

            // We can't approve a membership or member until all errors have been corrected
            bp.SubmitMembership(m1.Id);

            var newMemberships = bp.ListMemberships(WorkflowStatuses.New);

            Output("newMemberships", newMemberships);

            var pendingMemberships = bp.ListMemberships(WorkflowStatuses.Pending);

            Output("pendingMemberships", pendingMemberships);

            bp.ApproveMembership(m1.Id);

            var approvedMemberships = bp.ListMemberships(WorkflowStatuses.Approved);

            Output("approvedMemberships", approvedMemberships);

            // Output("MemberCannotBelongToMultipleMemberships");

            //if (m1.Errors.Count > 0)
            //{
            //    foreach (var err in m1.Errors)
            //    {
            //        Console.WriteLine(err.FieldName + ": " + err.Message);
            //    }
            //}
            //else using (var db = new BenefitsDbContext())
            //    {
            //        db.Memberships.AddRange(new[] { m1, m2, m3 });
            //        try
            //        {
            //            db.SaveChanges();
            //        }
            //        catch (SqlException)
            //        {
            //            var ve = db.GetValidationErrors();
            //        }
            //    }

            Console.ReadLine();
        }

        private void Output(object message)
        {
            throw new NotImplementedException();
        }

        private void Output(string message)
        {
            Console.WriteLine(message);
        }

        private void Output(string title, IList<BMembership> memberships)
        {
            Console.WriteLine();
            Console.WriteLine(title);
            foreach (var m in memberships)
            {
                Output(m);
            }
        }

        private void Output(BMembership m)
        {
            Console.WriteLine("  Membership " + m.Number + " status=" + m.WorkflowStatus + " valid=" + m.IsValid);
            foreach (var dependency in m.Dependencies)
            {
                Console.WriteLine($"  - {dependency.Type} {dependency.Person.FirstName} {dependency.Person.LastName}");
            }
        }

        private void CreateMembers()
        {
        }
    }
}