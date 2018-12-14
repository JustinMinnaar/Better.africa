using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    [TestClass]
    public class ContractUnitTests : BasePersonUnitTests
    {
        [TestMethod]
        public void Contract_RequiresSignDate()
        {
            var c1 = new BContract();
            Assert.IsNotNull(c1.SignDateError);
        }

        [TestMethod]
        public void Contract_RequiresInceptionDate()
        {
            var c1 = new BContract();
            Assert.IsNotNull(c1.InceptionDateError);
        }

        [TestMethod]
        public void Contract_CanStartOn1stOfMonth_WhenSignDateBefore8th()
        {
            var c1 = new BContract()
                .WithSignDate(2019, 2, 7)
                .WithInceptionDate(2019, 1);
            Assert.IsNotNull(c1.InceptionDateError);

            var c2 = new BContract()
                .WithSignDate(2019, 2, 7)
                .WithInceptionDate(2019, 2);
            Assert.IsNull(c2.InceptionDateError);
        }

        [TestMethod]
        public void Contract_CantStartBefore1stOfFollowingMonth_WhenSignDateAfter7th()
        {
            var c1 = new BContract()
               .WithSignDate(2019, 1, 8)
               .WithInceptionDate(2019, 1);
            Assert.IsNotNull(c1.InceptionDateError);

            var c2 = new BContract()
                .WithSignDate(2019, 1, 8)
                .WithInceptionDate(2019, 2);
            Assert.IsNull(c2.InceptionDateError);
        }
    }
}