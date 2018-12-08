using BetterAfrica.Shared;
using Knights.Core.Common;

namespace BetterAfrica.Benefits.Entities
{
    public class AulPolicyPlan : BaseEntity
    {
        public string Err => $"Plan '{Name}'";

        public string Name { get; set; }

        public int LastPolicyNumberIssued { get; set; }

        public decimal MonthlyCostPrincipalUnder66 { get; set; }

        public decimal MonthlyCostPrincipalUnder76 { get; set; }
        public decimal MonthlyCostPrincipalUnder86 { get; set; }

        public decimal MonthlyCostSpouse18to65 { get; set; }

        public decimal MonthlyCostSpouseUnder76 { get; set; }
        public decimal MonthlyCostSpouseUnder86 { get; set; }

        public decimal MonthlyCostChildren { get; set; }
        public decimal MonthlyCostChild { get; set; }

        public decimal MonthlyCostFamilyUnder76 { get; set; }
        public decimal MonthlyCostFamilyUnder66 { get; set; }
        public decimal MonthlyCostFamilyUnder86 { get; set; }

        public int MinAgePrincipal { get; set; } = 18;
        public int MaxAgePrincipal { get; set; } = 65;

        public int MinAgeSpouse { get; set; } = 18;
        public int MaxAgeSpouse { get; set; } = 65;

        /// <summary>The minimum age of a child on this policy, under Children or under Family.</summary>
        public int MinAgeChild { get; set; } = 0;

        /// <summary>The maximum age of a child on this policy, under Children or under Family.</summary>
        public int MaxAgeChild { get; set; } = 18;

        /// <summary>The maximum age of a child that is studying on this policy, under Children or under Family.</summary>
        public int MaxAgeChildScholar { get; set; } = 25;

        public int MinAgeAdult { get; private set; } = 0;

        public int MaxAgeAdult { get; private set; } = 65;

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            MonthlyCostPrincipalUnder66.Bound(0m, 9999m);
            MonthlyCostSpouse18to65.Bound(0m, 9999m);
            MonthlyCostChild.Bound(0m, 9999m);
            MonthlyCostChildren.Bound(0m, 9999m);
            MonthlyCostFamilyUnder76.Bound(0m, 9999m);

            MinAgePrincipal.Bound(0, 99);
            MaxAgePrincipal.Bound(MinAgePrincipal, 99);

            MinAgeSpouse.Bound(0, 99);
            MaxAgeSpouse.Bound(MinAgeSpouse, 99);

            MinAgeChild.Bound(0, 99);
            MaxAgeChild.Bound(0, 99);
            MaxAgeChildScholar.Bound(0, 99);

            MinAgeAdult.Bound(0, 99);
            MaxAgeAdult.Bound(0, 99);
        }

        public int MaxAgeInYears(BDependencyType type, bool isTertiaryStudent)
        {
            switch (type)
            {
                case BDependencyType.Principal: return MaxAgePrincipal;
                case BDependencyType.Spouse: return MaxAgeSpouse;
                case BDependencyType.Child: return isTertiaryStudent ? MaxAgeChildScholar : MaxAgeChild;
                case BDependencyType.Family: return MaxAgeAdult;
                default: return 0;
            }
        }

        public int MinAgeInYears(BDependencyType type)
        {
            switch (type)
            {
                case BDependencyType.Principal: return MinAgePrincipal;
                case BDependencyType.Spouse: return MinAgeSpouse;
                case BDependencyType.Child: return MinAgeChild;
                case BDependencyType.Family: return MinAgeAdult;
                default: return 0;
            }
        }
    }
}