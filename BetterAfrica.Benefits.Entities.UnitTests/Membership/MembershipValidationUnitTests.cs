using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    [TestClass]
    public class MemberValidationUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void Member_PrincipalIsRequired()
        {
            var a1 = new AulPolicy();
            var m1 = new BMember()
                .WithInceptionDate(yy: 2019, mm: 1)
                .WithSpouse(p2Bertha47);

            Assert.IsNotNull(m1.PrincipalError);
        }

        [TestMethod]
        public void Member_PrincipalMustBeAge18OrAbove()
        {
            var m1 = new BMember()
                .WithInceptionDate(yy: 2019, mm: 1)
                .WithPrincipal(p3Charles11);

            Assert.IsNotNull(m1.PrincipalError);
        }
    }
}