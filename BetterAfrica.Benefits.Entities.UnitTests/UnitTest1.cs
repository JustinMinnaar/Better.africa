using System;
using System.IO;
using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Common;
using Knights.Core.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Xml_CanImportMemberships()
        {
            var xmlExpected = File.ReadAllText("..\\..\\Memberships1.xml");
            var memberships = FormMemberships.FromXml(xmlExpected);
            var node = FormMemberships.ToNode(memberships);

            var xmlActual = node.ToXml();
            File.WriteAllText("..\\..\\Memberships2.xml", xmlActual);
            Assert.AreEqual(xmlExpected, xmlActual);
        }

        [TestMethod]
        public void Xml_FormMembershipBeneficiary()
        {
            var xml = "<beneficiary name='Name' ratio='Ratio' phone='Phone' email='Email' identity='Identity' />";
            var beneficiary = FormMembershipBeneficiary.FromNode(CNode.FromXml(xml));

            var xmlActual = beneficiary.ToNode().ToXml(flatten: true);
            Assert.AreEqual(xml, xmlActual);
        }
    }
}