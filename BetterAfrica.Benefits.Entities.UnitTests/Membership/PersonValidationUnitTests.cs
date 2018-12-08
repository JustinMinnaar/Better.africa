﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    [TestClass]
    public class PersonValidationUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void Person_PrincipalRequiresName()
        {
            var p1 = new BPerson();
            Assert.IsNotNull(p1.NameError);

            Assert.IsNull(p1Adam49.NameError);
        }

        [TestMethod]
        public void Person_RequiresIdentity()
        {
            // no identity fails
            var p1 = new BPerson();
            Assert.IsNotNull(p1.IdentityNumberError);

            // a valid identity passes
            Assert.IsNull(p1Adam49.IdentityNumberError);
        }
    }
}