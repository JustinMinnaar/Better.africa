using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benefits.Entities.UnitTests.Aul
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Policy_CalculatePriceForPlan()
        {
            var p1 = new Person().WithDateOfBirth(1969, 7, 31);

            var plan = new AulPolicyPlan
            {
                Name = "Funeral Bronze",
                MonthlyCostPrincipal = 10
            };

            var policy = new AulPolicy { Plan = plan }
            .WithDependency(p1, MembershipType.Principal)
            .WithSignDate(2019, 1, 1)
            .WithInceptionDate(2019, 1);

            var cost = policy.CalculatedMonthlyCost();
        }
    }
}