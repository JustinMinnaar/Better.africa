using Knights.Core.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    public class FuneralPolicy_UnitTests : BaseValidationUnitTests
    {
    }

    [TestClass]
    public class PolicyValidationUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void OnlyOnePrincipalIsPermittedAndPrincipalIsRequired()
        {
            var m1 = new BMembership().WithInceptionDate(yy: 2019, mm: 1);
            Assert.IsNotNull(m1.PrincipalError);

            // one principal
            m1.WithPrincipal(p1Adam49);
            Assert.IsNull(m1.PrincipalError);

            // two principals
            m1.WithPrincipal(p2Bertha47);
            Assert.IsNotNull(m1.PrincipalError);
        }

        [TestMethod]
        public void PrincipalAge18OrAbove()
        {
            Clock.FreezeAt(new System.DateTime(2019, 1, 1));

            var m1 = new BMembership()
                .WithInceptionDate(yy: 2019, mm: 1)
                .WithPrincipal(p3Charles11);
            Assert.IsNotNull(m1.PrincipalError);

            m1.PeoplePrincipal.DateOfBirth = new System.DateTime(1969, 7, 31);
            Assert.IsNull(m1.PrincipalError);
        }

        [TestMethod]
        public void OnlyOneSpouseIsPermitted()
        {
            var a1 = new AulPolicy();
            var m1 = new BMembership()
                .WithInceptionDate(yy: 2019, mm: 1)
                .WithSpouse(p2Bertha47)
                .WithSpouse(p3Charles11);

            Assert.IsNotNull(m1.SpouseError);
        }

        [TestMethod]
        public void SpouseAge18OrAbove()
        {
            var m1 = new BMembership()
                .WithInceptionDate(yy: 2019, mm: 1)
                .WithPrincipal(p1Adam49)
                .WithSpouse(p3Charles11);

            Assert.IsNotNull(m1.SpouseError);
        }

        [TestMethod]
        public void ChildrenWithinAge()
        {
            var m1 = new BMembership()
                .WithSignDate(yy: 2019, mm: 1, dd: 7)
                .WithInceptionDate(yy: 2019, mm: 1)
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
            var m1 = new BMembership()
                .WithInceptionDate(yy: 2019, mm: 1)
               .WithPrincipal(p1Adam49)
               .WithSpouse(p1Adam49);

            var m1Errors = m1.Errors;
            Assert.AreEqual(0, m1Errors.Count);
        }
    }
}