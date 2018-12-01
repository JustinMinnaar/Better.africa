using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Entities
{
    public class AulPolicy : Contract
    {
        public string Err => $"Policy '{Number}'";

        public Guid PlanId { get; set; }

        /// <summary>The plan to be applied to this policy.</summary>
        public AulPolicyPlan Plan { get; set; }

        /// <summary>Returns an error message, or null if ok.</summary>
        public string PlanError
        {
            get
            {
                if (Plan == null) return $"Plan is required for policy {Number}.";

                if (!Plan.IsValid) return $"Invalid plan {Plan.Name} for policy {Number}.";

                return null;
            }
        }

        /// <summary>All persons covered by the plan, including the principal.</summary>
        public ICollection<AulPolicyDependency> Dependancies { get; } = new HashSet<AulPolicyDependency>();

        public override void BeforeSave(EntityErrors errors)
        {
            base.BeforeSave(errors);

            errors.Add(nameof(Plan), PlanError);
            if (Plan == null) return;

            errors.Add(nameof(Principal), PrincipalError);
            errors.Add(nameof(Spouse), SpouseError);

            foreach (var dependancy in Dependancies)
            {
                var person = dependancy.Person;
                person.BeforeSave(errors);

                if (InceptionDate != null)
                {
                    var type = person.MembershipType;
                    var minAgeInYears = Plan.MinAgeInYears(type);
                    var maxAgeInYears = Plan.MaxAgeInYears(type, person.IsScholar);

                    var personYears = Spouse.Person.AgeInYearsAsAt(InceptionDate.Value);
                    if (personYears < minAgeInYears || personYears > maxAgeInYears)
                        errors.Add(nameof(Dependancies),
                            $"{person.Err} must be between {minAgeInYears} and {maxAgeInYears} years old on inception date {InceptionDate} for {Plan.Err} for {Err}.");

                    if (person.AgeInYearsAsAt(Clock.Now) >= maxAgeInYears)
                        Errors.Add(nameof(Membership.People), $"{person.MembershipType} '{person.Name}' cannot be older than {maxAgeInYears} for policy {Number}.");
                }
            }
        }

        /// <summary>The dependency marked as Principal, or null.</summary>
        public AulPolicyDependency Principal
        {
            get
            {
                var dependants = Dependancies.Where(p => p.Type == MembershipType.Principal).ToList();
                if (dependants.Count == 0) return dependants[0];
                return null;
            }
        }

        /// <summary>The Dependency marked as Spouse, or null.</summary>
        public AulPolicyDependency Spouse
        {
            get
            {
                var dependants = Dependancies.Where(p => p.Type == MembershipType.Spouse).ToList();
                if (dependants.Count == 0) return dependants[0];
                return null;
            }
        }

        /// <summary>All Dependencies marked as the specific type.</summary>
        public IList<AulPolicyDependency> GetDependencies(MembershipType type)
        {
            return Dependancies.Where(p => p.Type == MembershipType.Principal).ToList();
        }

        /// <summary>An error message, otherwise null.</summary>
        public string PrincipalError
        {
            get
            {
                if (Principal == null)
                    return $"There must be one principal for policy {Number}.";

                return null;
            }
        }

        /// <summary>An error message, otherwise null.</summary>
        public string SpouseError
        {
            get
            {
                var spouses = GetDependencies(MembershipType.Spouse);
                if (spouses.Count > 1)
                    return $"There cannot be more than one spouse for policy {Number}.";

                return null;
            }
        }

        /// <summary>Add a person of a specific type to be covered.</summary>
        public AulPolicy WithDependency(Person person, MembershipType type)
        {
            Dependancies.Add(new AulPolicyDependency
            {
                Policy = this,
                Person = person,
                Type = type
            });
            return this;
        }
    }

    /// <summary>
    ///     A person of a particular type to be covered by the policy.
    /// </summary>
    public class AulPolicyDependency
    {
        public Guid PolicyId { get; set; }
        public AulPolicy Policy { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        /// <summary>
        /// Although the Person has a membership type in Better Africa, each policy
        /// could be unique and actually cover a different person as Principal, etc.
        /// For example, extended family member may need their own policy for medical cover.
        /// </summary>
        public MembershipType Type { get; set; }
    }

    public class AulPolicyPlan : BaseEntity
    {
        public string Err => $"Plan '{Name}'";

        public string Name { get; set; }

        public int LastPolicyNumberIssued { get; set; }

        public decimal MonthlyCostPrincipal { get; set; }
        public decimal MonthlyCostSpouse { get; set; }
        public decimal MonthlyCostChildren { get; set; }
        public decimal MonthlyCostChild { get; set; }
        public decimal MonthlyCostExtendedPersons { get; set; }
        public decimal MonthlyCostExtendedPerson { get; set; }

        public int MinAgePrincipal { get; set; } = 18;
        public int MaxAgePrincipal { get; set; } = 65;

        public int MinAgeSpouse { get; set; } = 18;
        public int MaxAgeSpouse { get; set; } = 65;

        /// <summary>The minimum age of a child on this policy, under Children or Extended Family.</summary>
        public int MinAgeChild { get; set; } = 0;

        /// <summary>The maximum age of a child on this policy, under Children or Extended Family.</summary>
        public int MaxAgeChild { get; set; } = 18;

        /// <summary>The maximum age of a child that is studying on this policy, under Children or Extended Family.</summary>
        public int MaxAgeChildScholar { get; set; } = 25;

        public int MinAgeAdult { get; private set; } = 0;
        public int MaxAgeAdult { get; private set; } = 65;

        public override void BeforeSave(EntityErrors errors)
        {
            base.BeforeSave(errors);

            MonthlyCostPrincipal.Bound(0m, 9999m);
            MonthlyCostSpouse.Bound(0m, 9999m);
            MonthlyCostChild.Bound(0m, 9999m);
            MonthlyCostChildren.Bound(0m, 9999m);
            MonthlyCostExtendedPerson.Bound(0m, 9999m);
            MonthlyCostExtendedPersons.Bound(0m, 9999m);

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

        public int MaxAgeInYears(MembershipType type, bool isScholar)
        {
            switch (type)
            {
                case MembershipType.Principal: return MaxAgePrincipal;
                case MembershipType.Spouse: return MaxAgeSpouse;
                case MembershipType.Child: return isScholar ? MaxAgeChildScholar : MaxAgeChild;
                case MembershipType.Family: return MaxAgeAdult;
                default: return 0;
            }
        }

        public int MinAgeInYears(MembershipType type)
        {
            switch (type)
            {
                case MembershipType.Principal: return MinAgePrincipal;
                case MembershipType.Spouse: return MinAgeSpouse;
                case MembershipType.Child: return MinAgeChild;
                case MembershipType.Family: return MinAgeAdult;
                default: return 0;
            }
        }
    }
}