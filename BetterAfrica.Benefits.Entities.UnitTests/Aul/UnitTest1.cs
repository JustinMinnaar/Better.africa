using System;
using System.Diagnostics;
using System.Linq;
using Knights.Core.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BetterAfrica.Benefits.Entities.UnitTests.Aul
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Policy_CalculationForMedicalPolicy()
        {
        }

        [TestMethod]
        public void Policy_CalculattionsForBronzePlans()
        {
            // Ensure that testing continues to work regardless of real-world date
            Clock.FreezeAt(new DateTime(2019, 1, 1));

            var pricing2019 = new AulPolicyPricing
            {
                FromDate = new DateTime(2019, 1, 1),

                FuneralPer1000PrincipalUnder66 = 2.25m,         // R 33.75, R 67.50,
                FuneralPer1000PrincipalUnder76 = 9.00m,         // R135.00, R270.00
                FuneralPer1000PrincipalUnder86 = 15.50m,        // R232.50, R465.00
                FuneralPer1000SpouseUnder66 = 0.90m,            // R232.50
                FuneralPer1000SpouseUnder76 = 9m,
                FuneralPer1000SpouseUnder86 = 9m,
                FuneralPer1000Children = 2m,
                FuneralPer1000Child = 5m,
                FuneralPer1000FamilyUnder66 = 9.5m,
                FuneralPer1000FamilyUnder76 = 99.5m,
                FuneralPer1000FamilyUnder86 = 109.5m,
            };

            // Create initial policy plans
            var plan = new AulPolicyPlan
            {
                Name = "Funeral (Bronze)",
                MonthlyCostPrincipalUnder66 = 10,
                MonthlyCostPrincipalUnder76 = 100,
                MonthlyCostPrincipalUnder86 = 500,
                MonthlyCostSpouse18to65 = 9,
                MonthlyCostChildren = 2,
                MonthlyCostChild = 5,
                MonthlyCostFamilyUnder66 = 9.5m,
                MonthlyCostFamilyUnder76 = 99.5m,
                MonthlyCostFamilyUnder86 = 109.5m,
            };

            var planMedical = new AulPolicyPlan
            {
            };

            // Age 18-65
            for (int agePrincipal = 18; agePrincipal < 66; agePrincipal++)
            {
                var p1 = new BPerson().WithDateOfBirth(1969, 7, 31);

                var policy = new AulPolicy { Plan = plan }
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
                policy.WithDependency(p6, BDependencyType.Family);

                var c4 = policy.CalculatedMonthlyCost().Sum(a => a.Amount);
                Assert.AreEqual(45.5m, c4);

                foreach (var li in policy.CalculatedMonthlyCost())
                {
                    Trace.WriteLine(li.Name + " : " + li.Amount);
                }
            }
        }
    }