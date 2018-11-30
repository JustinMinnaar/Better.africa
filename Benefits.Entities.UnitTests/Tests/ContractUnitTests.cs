using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benefits.Entities.UnitTests
{
    [TestClass]
    public class ContractUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void Contract_RequiresSignDate()
        {
            var m1 = new Membership().WithPrincipal(p1Adam49);
            Assert.IsNotNull(m1.SignDateError);
        }

        [TestMethod]
        public void Contract_RequiresInceptionDate()
        {
            var m1 = new Membership().WithPrincipal(p1Adam49);
            Assert.IsNotNull(m1.InceptionDateError);
        }

        [TestMethod]
        public void Contract_CanStartOn1stOfMonthWhenSignDateBefore8th()
        {
            var m1 = new Membership().WithPrincipal(p1Adam49);
            m1.SignDate = new System.DateTime(2019, 01, 07);
            m1.InceptionDate = new System.DateTime(2019, 01, 01);
            Assert.IsNull(m1.InceptionDateError);
        }

        [TestMethod]
        public void Contract_CantStartOn1stOfMonthWhenSignDateBefore8th()
        {
            var m1 = new Membership().WithPrincipal(p1Adam49);
            m1.SignDate = new System.DateTime(2019, 01, 08);
            m1.InceptionDate = new System.DateTime(2019, 01, 01);
            Assert.IsNotNull(m1.InceptionDateError);
        }

        [TestMethod]
        public void Contract_CanStartOn1stOfFollowingMonthWhenSignDateAfter7th()
        {
            var m1 = new Membership().WithPrincipal(p1Adam49);
            m1.SignDate = new System.DateTime(2019, 01, 08);
            m1.InceptionDate = new System.DateTime(2019, 02, 01);
            Assert.IsNull(m1.InceptionDateError);
        }

        [TestMethod]
        public void Contract_CantStartBefore1stOfFollowingMonthWhenSignDateAfter7th()
        {
            var m1 = new Membership().WithPrincipal(p1Adam49);
            m1.SignDate = new System.DateTime(2019, 01, 08);
            m1.InceptionDate = new System.DateTime(2019, 01, 01);
            Assert.IsNotNull(m1.InceptionDateError);
        }
    }
}