using BetterAfrica.Benefits.Entities;
using Knights.Core.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Providers.UnitTests
{
    [TestClass]
    public class Membership_UnitTests
    {
        private MembershipProvider provider = new MembershipProvider(userId: 1);

        [TestMethod]
        public void GivenMemberForm_ProviderCreatesMember()
        {
            var member1 = new CMember
            {
                Address = "PO Box 1180",
                AddressCity = "Kempton Park",
                AddressCode = "1620",
                AddressState = "Gauteng",
                AddressCountry = "South Africa",

                CommunicateLanguage = ELanguage.English,
                CommunicateSms = ECommunicateSms.Yes,
                CommunicateVia = ECommunicateVia.Email,

                InceptionDate = Clock.Now,
                SignDate = Clock.Now,
                TerminationDate = null,
                Number = 101,
            };

            provider.CreateMember(member1);

            var member2 = provider.TryGetMember(member1.Id);

            var xmlExpected = member1.ToNode().ToXml(flatten: true);
            var xmlActual = member2.ToNode().ToXml(flatten: true);

            Assert.AreEqual(xmlExpected, xmlActual);
        }

        //[TestMethod]
        //public void GivenNewMembershipForm_WhenValidAndComplete_OnApproval_CreatesNewMembership()
        //{
        //    var provider = new MembershipProvider(Guid.NewGuid());

        //    var forms = FormMemberships.FromXmlFile("Memberships.xml");
        //    //var forms = FormMembership.ManyFromXmlFile("Memberships.xml");

        //    var newMemberForm = forms.FirstOrDefault(p => p.Detail.Number == "1");
        //    var member = provider.ApplyMembershipForm(newMemberForm);
        //    Assert.IsNotNull(member);
        //}
    }
}