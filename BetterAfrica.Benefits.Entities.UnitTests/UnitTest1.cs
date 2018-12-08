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
        public void CanAccessPrincipalAndSpouse()
        {
            var m = new FormMembership();
            var pd = m.Principal.DateOfBirth;
            var sd = m.Spouse.DateOfBirth;
        }

        [TestMethod]
        public void Xml_CanImportExportMemberships()
        {
            var xmlExpected = File.ReadAllText("..\\..\\Memberships1.xml");
            var memberships = FormMemberships.FromXml(xmlExpected);

            var node = FormMemberships.ToNode(memberships);
            var xmlActual = node.ToXml();
            File.WriteAllText("..\\..\\Memberships2.xml", xmlActual);
            Assert.AreEqual(xmlExpected, xmlActual);
        }
    }
}