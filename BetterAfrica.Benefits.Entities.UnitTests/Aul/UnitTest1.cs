using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests.Aul
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Policy_CalculatePriceForPlan()
        {
            var p1 = new BPerson().WithDateOfBirth(1969, 7, 31);

            var plan = new AulPolicyPlan
            {
                Name = "Funeral Bronze",
                MonthlyCostPrincipal = 10,
                MonthlyCostSpouse = 9,
                MonthlyCostChild = 5,
                MonthlyCostChildren = 2,
                MonthlyCostFamily = 9.5m,
            };

            var policy = new BAulPolicy { Plan = plan }
            .WithDependency(p1, BDependencyType.Principal)
            .WithSignDate(2019, 1, 1)
            .WithInceptionDate(2019, 1);

            var c1 = policy.CalculatedMonthlyCost().Sum(a => a.Amount);
            Assert.AreEqual(10, c1);

            var p2 = new BPerson().WithDateOfBirth(1971, 5, 4);
            policy.WithDependency(p2, BDependencyType.Spouse);

            var c2 = policy.CalculatedMonthlyCost().Sum(a => a.Amount);
            Assert.AreEqual(19, c2);

            var p3 = new BPerson().WithDateOfBirth(2010, 1, 1);
            policy.WithDependency(p3, BDependencyType.Child);

            var p4 = new BPerson().WithDateOfBirth(2011, 1, 1);
            policy.WithDependency(p4, BDependencyType.Child);

            var p5 = new BPerson().WithDateOfBirth(2012, 1, 1);
            policy.WithDependency(p5, BDependencyType.Child);

            var c3 = policy.CalculatedMonthlyCost().Sum(a => a.Amount);
            Assert.AreEqual(36, c3);

            var p6 = new BPerson().WithDateOfBirth(2000, 1, 1);
            policy.WithDependency(p6, BDependencyType.Person);

            var c4 = policy.CalculatedMonthlyCost().Sum(a => a.Amount);
            Assert.AreEqual(45.5m, c4);

            foreach (var li in policy.CalculatedMonthlyCost())
            {
                Trace.WriteLine(li.Name + " : " + li.Amount);
            }
        }
    }
}