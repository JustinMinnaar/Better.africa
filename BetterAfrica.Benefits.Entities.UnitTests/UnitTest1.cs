using System;
using System.IO;
using BetterAfrica.Benefits.Entities;
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
            var m = new FormMember();
            var pd = m.Principal.DateOfBirth;
            var sd = m.Spouse.DateOfBirth;
        }

        [TestMethod]
        public void Xml_CanImportExportMembers()
        {
            var xmlExpected = File.ReadAllText("..\\..\\Members1.xml");
            var memberships = BMembers.FromXml(xmlExpected);

            var node = BMembers.ToNode(memberships);
            var xmlActual = node.ToXml();
            File.WriteAllText("..\\..\\Members2.xml", xmlActual);
            Assert.AreEqual(xmlExpected, xmlActual);
        }
    }
}