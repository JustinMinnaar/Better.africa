using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benefits.Entities.UnitTests
{
    [TestClass]
    public class PersonValidationUnitTests : BaseValidationUnitTests
    {
        [TestMethod]
        public void Person_PrincipalRequiresName()
        {
            var p1 = new Person();
            Assert.IsNotNull(p1.NameError);

            Assert.IsNull(p1Adam49.NameError);
        }

        [TestMethod]
        public void Person_RequiresIdentity()
        {
            var p1 = new Person();
            Assert.IsNotNull(p1.IdentityError);

            Assert.IsNull(p1Adam49.IdentityError);
        }
    }
}