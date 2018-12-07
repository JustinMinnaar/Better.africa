using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    [TestClass]
    public class MembershipValidationUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void Membership_PrincipalIsRequired()
        {
            var a1 = new BAulPolicy();
            var m1 = new BMembership()
                .WithInceptionDate(yy: 2019, mm: 1)
                .WithSpouse(p2Bertha47);

            Assert.IsNotNull(m1.PrincipalError);
        }

        [TestMethod]
        public void Membership_PrincipalMustBeAge18OrAbove()
        {
            var m1 = new BMembership()
                .WithInceptionDate(yy: 2019, mm: 1)
                .WithPrincipal(p3Charles11);

            Assert.IsNotNull(m1.PrincipalError);
        }
    }
}