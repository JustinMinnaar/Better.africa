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
            var m1 = new Membership().WithPrincipal(p1Adam49).WithSpouse(p2Bertha47).WithChildren(p3Charles11);
            var m2 = new Membership().WithSpouse(p2Bertha47);
            var m3 = new Membership().WithPrincipal(p2Bertha47).WithSpouse(p1Adam49);

            Assert.IsNull(m1.PrincipalError);
            Assert.IsNotNull(m2.PrincipalError);
            Assert.IsNull(m3.PrincipalError);

            // We can save these to the database

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
                    db.Members.AddRange(new[] { m1, m2, m3 });
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