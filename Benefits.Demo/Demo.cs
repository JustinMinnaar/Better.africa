﻿using Benefits.Entities;
using Benefits.Entities.UnitTests;
using Benefits.Provider;
using Benefits.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Demo
{
    public class Demo : BaseValidationUnitTests
    {
        public Demo()
        {
            // We can create any number of memberships with members, even if there are errors in the data

            // This allows editing the data until correct, then submitting it for approval

            // We can save these to the database, even though they contain errors.
            var bp = new BenefitsProvider(Guid.NewGuid());

            var agent = new BPerson { NameFirst = "Luke " };
            bp.CreatePerson(agent);

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

            var newMemberships = bp.ListMemberships(Shared.WorkflowStatuses.New);
            Output("newMemberships", newMemberships);

            var pendingMemberships = bp.ListMemberships(Shared.WorkflowStatuses.Pending);
            Output("pendingMemberships", pendingMemberships);

            bp.ApproveMembership(m1.Id);

            var approvedMemberships = bp.ListMemberships(Shared.WorkflowStatuses.Approved);
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
                Console.WriteLine($"  - {dependency.Type} {dependency.Person.NameFirst} {dependency.Person.NameLast}");
            }
        }

        private void CreateMembers()
        {
        }
    }
}