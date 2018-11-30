using Benefits.Entities;
using Benefits.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benefits.Entities.UnitTests
{
    [TestClass]
    public class PolicyValidationUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void PrincipalIsRequired()
        {
            var a1 = new AulPolicy();
            var m1 = new Membership()
                .WithInceptionDate(mm: 1, yy: 2019)
                .WithSpouse(p2Bertha47);

            Assert.IsNotNull(m1.PrincipalError);
        }

        [TestMethod]
        public void PrincipalAge18OrAbove()
        {
            var m1 = new Membership()
                .WithInceptionDate(mm: 1, yy: 2019)
                .WithPrincipal(p3Charles11);

            Assert.IsNotNull(m1.PrincipalError);
        }

        [TestMethod]
        public void SpouseIsRequired()
        {
            var a1 = new AulPolicy();
            var m1 = new Membership()
                .WithInceptionDate(mm: 1, yy: 2019)
                .WithSpouse(p2Bertha47)
                .WithSpouse(p3Charles11);

            Assert.IsNotNull(a1.SpouseError);
        }

        [TestMethod]
        public void SpouseAge18OrAbove()
        {
            var m1 = new Membership()
                .WithInceptionDate(mm: 1, yy: 2019)
                .WithPrincipal(p1Adam49)
                .WithSpouse(p3Charles11);

            Assert.IsNotNull(m1.SpouseError);
        }

        [TestMethod]
        public void ChildrenWithinAge()
        {
            var m1 = new Membership()
                .WithSignDate(dd: 7, mm: 1, yy: 2019)
                .WithInceptionDate(mm: 1, yy: 2019)
                .WithPrincipal(p1Adam49)
                .WithSpouse(p2Bertha47)
                .WithChildren(p3Charles11, p4Debbie1, p5Eddie27);

            // should fail on Eddie
            var errors = m1.Errors;
            Assert.IsFalse(m1.IsValid);
        }

        [TestMethod]
        public void Policy_CantAddSameMemberTwiceToPolicy()
        {
            var m1 = new Membership()
                .WithInceptionDate(mm: 1, yy: 2019)
               .WithPrincipal(p1Adam49)
               .WithSpouse(p1Adam49);

            var m1Errors = m1.Errors;
            Assert.AreEqual(0, m1Errors.Count);
        }
    }
}