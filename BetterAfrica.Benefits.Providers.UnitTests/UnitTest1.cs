using System;
using System.Linq;
using Benefits.Provider;
using Benefits.Provider.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Providers.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GivenNewMembershipForm_WhenValidAndComplete_OnApproval_CreatesNewMembership()
        {
            var provider = new MembershipProvider(Guid.NewGuid());

            var forms = BenefitsXmlReader.ReadForms("Memberships.xml");
            //var forms = FormMembership.ManyFromXmlFile("Memberships.xml");

            var newMemberForm = forms.FirstOrDefault(p => p.Detail.Number == "1");
            var membership = provider.CreateMembership(newMemberForm);
            Assert.IsNotNull(membership);
        }
    }
}