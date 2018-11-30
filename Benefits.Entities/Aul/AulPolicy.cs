using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Entities
{
    public class AulPolicy : Contract<AulPolicy>
    {
        public string Err => $"Policy '{Number}'";

        public Guid PlanId { get; set; }

        public AulPolicyPlan Plan { get; set; }

        public string PlanError
        {
            get
            {
                if (Plan == null) return $"Plan is required for policy {Number}.";

                if (!Plan.IsValid) return $"Invalid plan {Plan.Name} for policy {Number}.";

                return null;
            }
        }

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
                    var type = person.Type;
                    var minAgeInYears = Plan.MinAgeInYears(type);
                    var maxAgeInYears = Plan.MaxAgeInYears(type, person.IsScholar);

                    var personYears = Spouse.Person.AgeInYearsAsAt(InceptionDate.Value);
                    if (personYears < minAgeInYears || personYears > maxAgeInYears)
                        errors.Add(nameof(Dependancies),
                            $"{person.Err} must be between {minAgeInYears} and {maxAgeInYears} years old on inception date {InceptionDate} for {Plan.Err} for {Err}.");

                    if (person.AgeInYearsAsAt(Clock.Now) >= maxAgeInYears)
                        Errors.Add(nameof(Membership.People), $"{person.Type} '{person.Name}' cannot be older than {maxAgeInYears} for policy {Number}.");
                }
            }
        }

        public AulPolicyDependency Principal
        {
            get
            {
                var dependants = Dependancies.Where(p => p.Type == PersonType.Principal).ToList();
                if (dependants.Count == 0) return dependants[0];
                return null;
            }
        }

        public AulPolicyDependency Spouse
        {
            get
            {
                var dependants = Dependancies.Where(p => p.Type == PersonType.Spouse).ToList();
                if (dependants.Count == 0) return dependants[0];
                return null;
            }
        }

        public IList<AulPolicyDependency> GetDependencies(PersonType type)
        {
            return Dependancies.Where(p => p.Type == PersonType.Principal).ToList();
        }

        public string PrincipalError
        {
            get
            {
                if (Principal == null)
                    return $"There must be one principal for policy {Number}.";

                return null;
            }
        }

        public string SpouseError
        {
            get
            {
                var spouses = GetDependencies(PersonType.Spouse);
                if (spouses.Count > 1)
                    return $"There cannot be more than one spouse for policy {Number}.";

                return null;
            }
        }

        public AulPolicy Cover(Person person, PersonType type)
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

    public class AulPolicyDependency
    {
        public Guid PolicyId { get; set; }
        public AulPolicy Policy { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        public PersonType Type { get; set; }
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

        public int MaxAgeInYears(PersonType type, bool isScholar)
        {
            switch (type)
            {
                case PersonType.Principal: return MaxAgePrincipal;
                case PersonType.Spouse: return MaxAgeSpouse;
                case PersonType.Child: return isScholar ? MaxAgeChildScholar : MaxAgeChild;
                case PersonType.Family: return MaxAgeAdult;
                default: return 0;
            }
        }

        public int MinAgeInYears(PersonType type)
        {
            switch (type)
            {
                case PersonType.Principal: return MinAgePrincipal;
                case PersonType.Spouse: return MinAgeSpouse;
                case PersonType.Child: return MinAgeChild;
                case PersonType.Family: return MinAgeAdult;
                default: return 0;
            }
        }
    }
}