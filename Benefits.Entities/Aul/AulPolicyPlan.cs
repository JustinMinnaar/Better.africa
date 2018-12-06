using BetterAfrica.Shared;

namespace Benefits.Entities
{
    public class AulPolicyPlan : BaseEntity
    {
        public string Err => $"Plan '{Name}'";

        public string Name { get; set; }

        public int LastPolicyNumberIssued { get; set; }

        public decimal MonthlyCostPrincipal { get; set; }
        public decimal MonthlyCostSpouse { get; set; }
        public decimal MonthlyCostChildren { get; set; }
        public decimal MonthlyCostChild { get; set; }
        public decimal MonthlyCostFamily { get; set; }

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

            MonthlyCostPrincipal.Bound(0m, 9999m);
            MonthlyCostSpouse.Bound(0m, 9999m);
            MonthlyCostChild.Bound(0m, 9999m);
            MonthlyCostChildren.Bound(0m, 9999m);
            MonthlyCostFamily.Bound(0m, 9999m);

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
                case BDependencyType.Person: return MaxAgeAdult;
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
                case BDependencyType.Person: return MinAgeAdult;
                default: return 0;
            }
        }
    }
}