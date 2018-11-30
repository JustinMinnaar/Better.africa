﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benefits.Entities.UnitTests
{
    [TestClass]
    public class MembershipValidationUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void Membership_PrincipalIsRequired()
        {
            var a1 = new AulPolicy();
            var m1 = new Membership()
                .WithInceptionDate(mm: 1, yy: 2019)
                .WithSpouse(p2Bertha47);

            Assert.IsNotNull(m1.PrincipalError);
        }

        [TestMethod]
        public void Membership_PrincipalMustBeAge18OrAbove()
        {
            var m1 = new Membership()
                .WithInceptionDate(mm: 1, yy: 2019)
                .WithPrincipal(p3Charles11);

            Assert.IsNotNull(m1.PrincipalError);
        }

        [TestMethod]
        public void MemberCannotBelongToMultipleMemberships()
        {
            using (var db = new BenefitsDbContext())
            {
                // Membership 1 with Adam
                var m1 = new Membership()
                    .WithInceptionDate(mm: 1, yy: 2019)
                    .WithPrincipal(p1Adam49);
                db.Members.Add(m1);

                // Membership 2 with Adam
                var m2 = new Membership()
                    .WithInceptionDate(mm: 1, yy: 2019)
                    .WithPrincipal(p1Adam49);
                db.Members.Add(m2);

                db.SaveChanges();
            }
        }
    }
}