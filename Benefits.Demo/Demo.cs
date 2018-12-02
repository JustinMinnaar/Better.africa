using Benefits.Entities;
using Benefits.Entities.UnitTests;
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

            var m1 = new MembershipModel()
                .WithSignDate(2018, 12, 01)
                .WithInceptionDate(2019, 02)
                .WithPrincipal(p1Adam49)
                .WithSpouse(p2Bertha47)
                .WithChildren(p3Charles11)
                .WithFamily(p5Eddie27);

            var m2 = new MembershipModel()
                .WithSignDate(2018, 12, 2)
                .WithInceptionDate(2020, 1)
                .WithSpouse(p2Bertha47);

            var m3 = new MembershipModel()
                .WithPrincipal(p2Bertha47)
                .WithSpouse(p1Adam49);

            // We can save these to the database, even though they contain errors.
            var bp = new BenefitsProvider(Guid.NewGuid());

            // We can list all memberships with errors

            // We can't approve a membership or member until all errors have been corrected

            if (m1.Errors.Count > 0)
            {
                foreach (var err in m1.Errors)
                {
                    Console.WriteLine(err.FieldName + ": " + err.Message);
                }
            }
            else using (var db = new BenefitsDbContext())
                {
                    db.Memberships.AddRange(new[] { m1, m2, m3 });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (SqlException)
                    {
                        var ve = db.GetValidationErrors();
                    }
                }
        }

        private void CreateMembers()
        {
        }
    }
}