using System;
using System.Linq;
using Benefits.Provider;
using BetterAfrica.Benefits.Entities.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Providers.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GivenPFuneral_CaulcationOfBilling()
        {
            var policy = new Policy
        }

        [TestMethod]
        public void GivenNewMembershipForm_WhenValidAndComplete_OnApproval_CreatesNewMembership()
        {
            var provider = new MembershipProvider(Guid.NewGuid());

            var forms = FormMemberships.FromXmlFile("Memberships.xml");
            //var forms = FormMembership.ManyFromXmlFile("Memberships.xml");

            var newMemberForm = forms.FirstOrDefault(p => p.Detail.Number == "1");
            var membership = provider.ApplyMembershipForm(newMemberForm);
            Assert.IsNotNull(membership);
        }
    }
}