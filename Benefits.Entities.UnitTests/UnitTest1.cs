using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benefits.Entities.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private Person p1;
        private Person p2;
        private Person p3;

        public UnitTest1()
        {
            p1 = new Person
            {
                NameFirst = "Adam",
                NameLast = "Adams",
                DateOfBirth = new DateTime(1969, 07, 31),
                MembershipType = PersonMembershipTypes.Principal,
            };

            p2 = new Person
            {
                NameFirst = "Debbie",
                NameLast = "Adams",
                DateOfBirth = new DateTime(1971, 11, 12),
                MembershipType = PersonMembershipTypes.Spouse,
            };

            p3 = new Person
            {
                NameFirst = "Charles",
                NameLast = "Adams",
                DateOfBirth = new DateTime(2017, 11, 12),
                MembershipType = PersonMembershipTypes.Spouse,
            };
        }

        [TestMethod]
        public void PrincipalIsRequired()
        {
            var m1 = new Member().WithSpouse(p2);
            m1.BeforeSave();
            Assert.AreEqual("There must be one principal.", m1.PrincipalError);
        }

        [TestMethod]
        public void PrincipalAge18OrAbove()
        {
            var m1 = new Member { InceptionDate = new DateTime(2019, 01, 1) }.WithPrincipal(p3);
            m1.BeforeSave();
            var msg = m1.Errors["Principal"];
            Assert.AreEqual("Principal must be between 18 and 65 years old on inception date.", msg);
        }
    }
}