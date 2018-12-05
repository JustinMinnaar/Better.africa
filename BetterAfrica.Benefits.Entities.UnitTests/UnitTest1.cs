using System;
using BetterAfrica.Benefits.Entities.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void XmlReader_DetailBeneficiary()
        {
            var d = new DetailBeneficiary
            {
                Email = "Email",
                Identity = "Identity",
                Name = "Name",
                Phone = "Phone",
                Ratio = "Ratio",
            };
            DetailBeneficiary.FromNode(node)

            BenefitsXmlReader.ReadForms()
        }
    }
}