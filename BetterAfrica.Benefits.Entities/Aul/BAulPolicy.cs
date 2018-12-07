using BetterAfrica.Shared;
using Knights.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAfrica.Benefits.Entities
{
    public class BAulPolicy : BContract
    {
        public string Err => $"Policy '{Number}'";

        [ForeignKey(nameof(Plan))]
        public Guid? PlanId { get; set; }

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

        public IEnumerable<TransactionItem> CalculatedMonthlyCost()
        {
            if (Plan != null)
            {
                if (Principal != null)
                    yield return new TransactionItem(name: "Principal", cost: Plan.MonthlyCostPrincipal);

                if (Spouse != null)
                    yield return new TransactionItem(name: "Spouse", cost: Plan.MonthlyCostSpouse);

                var childrenCount = Children.Count();
                if (childrenCount > 0)
                    yield return new TransactionItem(name: "Children",
                        cost: Plan.MonthlyCostChildren + childrenCount * Plan.MonthlyCostChild);

                var personCount = Family.Count();
                if (personCount > 0)
                    yield return new TransactionItem(name: "Family",
                        cost: personCount * Plan.MonthlyCostFamily);
            }
        }

        // Although the Person has a membership type in Better Africa, each policy
        // could be unique and actually cover a different person as Principal, etc.
        // For example, extended family member may need their own policy for medical cover.

        /// <summary>All persons covered by the plan, including the principal.</summary>
        public ICollection<AulPolicyDependency> Dependancies { get; } = new HashSet<AulPolicyDependency>();

        /// <summary>All Dependencies marked as the specific type.</summary>
        public IEnumerable<BPerson> GetDependencies(BDependencyType type)
        {
            foreach (var dependency in Dependancies.Where(p => p.Type == type))
            {
                yield return dependency.Person;
            }
        }

        /// <summary>The dependency marked as Principal, or null.</summary>
        public BPerson Principal
        {
            get
            {
                var dependants = Dependancies.Where(p => p.Type == BDependencyType.Principal).ToList();
                if (dependants.Count == 1) return dependants[0].Person;
                return null;
            }
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

        /// <summary>The Dependency marked as Spouse, or null.</summary>
        public BPerson Spouse
        {
            get
            {
                var dependants = Dependancies.Where(p => p.Type == BDependencyType.Spouse).ToList();
                if (dependants.Count == 1) return dependants[0].Person;
                if (dependants.Count > 1)
                    throw new BenefitsException($"{Err} has {dependants.Count} spouses. Only 1 is allowed.");

                return null;
            }
        }

        /// <summary>An error message, otherwise null.</summary>
        public string SpouseError
        {
            get
            {
                var spouses = GetDependencies(BDependencyType.Spouse).ToList();
                if (spouses.Count() > 1)
                    return $"There cannot be more than one spouse for policy {Number}.";

                return null;
            }
        }

        /// <summary>Persons covered by the policy as children.</summary>
        public IEnumerable<BPerson> Children => GetDependencies(BDependencyType.Child);

        /// <summary>Persons covered by the policy as family.</summary>
        public IEnumerable<BPerson> Family => GetDependencies(BDependencyType.Person);

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

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
                    var type = dependancy.Type;
                    var minAgeInYears = Plan.MinAgeInYears(type);
                    var maxAgeInYears = Plan.MaxAgeInYears(type, dependancy.IsTertiaryStudent);

                    var personYears = person.AgeInYearsAsAt(InceptionDate.Value);
                    if (personYears < minAgeInYears || personYears > maxAgeInYears)
                        errors.Add(nameof(Dependancies),
                            $"{person.Err} must be between {minAgeInYears} and {maxAgeInYears} years old on inception date {InceptionDate} for {Plan.Err} for {Err}.");

                    if (person.AgeInYearsAsAt(Clock.Now) >= maxAgeInYears)
                        Errors.Add(nameof(BMembership.Dependencies), $"{type} '{person.Name}' cannot be older than {maxAgeInYears} for policy {Number}.");
                }
            }
        }

        /// <summary>Add a person of a specific type to be covered.</summary>
        public BAulPolicy WithDependency(BPerson person, BDependencyType type)
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
}