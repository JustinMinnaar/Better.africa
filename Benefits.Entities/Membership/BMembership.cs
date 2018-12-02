using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Benefits.Entities
{
    public class BMembership : BContract
    {
        public virtual ICollection<BMembershipDependency> Dependencies { get; } = new HashSet<BMembershipDependency>();

        #region BeforeSave

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            errors.Add(nameof(PeoplePrincipal), PrincipalError);
            errors.Add(nameof(PeopleSpouse), SpouseError);
            foreach (var dependency in Dependencies)
            {
                dependency.Person.BeforeSave(errors);
            }
        }

        public string PrincipalError
        {
            get
            {
                var principalsCount = Dependencies.Count(p => p.Type == BDependencyType.Principal);
                if (principalsCount != 1) return "There must be one principal.";

                if (InceptionDate != null)
                {
                    var principalYears = PeoplePrincipal.AgeInYearsAsAt(InceptionDate.Value);
                    if (principalYears < 18 || principalYears > 65)
                        return "Principal must be between 18 and 65 years old on inception date.";
                }

                return null;
            }
        }

        public string SpouseError
        {
            get
            {
                var spouse = PeopleSpouse;
                if (spouse == null) return null;

                var spouseValid = Dependencies.Count(p => p.Type == BDependencyType.Spouse) == 1;
                if (!spouseValid) return "There may not be more than one spouse.";

                if (InceptionDate != null)
                {
                    var spouseYears = PeopleSpouse.AgeInYearsAsAt(InceptionDate.Value);
                    if (spouseYears < 18 || spouseYears > 65)
                        return "Spouse must be between 18 and 65 years old on inception date.";
                }

                return null;
            }
        }

        #endregion BeforeSave

        #region Helper Properties

        [NotMapped]
        public BPerson PeoplePrincipal => GetPeople(BDependencyType.Principal).FirstOrDefault();

        [NotMapped]
        public BPerson PeopleSpouse => GetPeople(BDependencyType.Spouse).FirstOrDefault();

        [NotMapped]
        public IList<BPerson> PeopleChildren => GetPeople(BDependencyType.Child).ToList();

        [NotMapped]
        public IList<BPerson> PeopleExtended => GetPeople(BDependencyType.Person).ToList();

        private IEnumerable<BPerson> GetPeople(BDependencyType type)
        {
            return from d in Dependencies where d.Type == type select d.Person;
        }

        #endregion Helper Properties

        #region Helper Methods

        public BMembership WithPrincipal(BPerson principal)
        {
            Dependencies.Add(new BMembershipDependency
            {
                Membership = this,
                Person = principal,
                Type = BDependencyType.Principal
            });
            return this;
        }

        public BMembership WithSpouse(BPerson spouse)
        {
            Dependencies.Add(new BMembershipDependency
            {
                Membership = this,
                Person = spouse,
                Type = BDependencyType.Spouse
            });
            return this;
        }

        public BMembership WithChildren(params BPerson[] children)
        {
            foreach (var child in children)
            {
                Dependencies.Add(new BMembershipDependency
                {
                    Membership = this,
                    Person = child,
                    Type = BDependencyType.Child
                });
            }
            return this;
        }

        public BMembership WithPerson(params BPerson[] persons)
        {
            foreach (var person in persons)
            {
                Dependencies.Add(new BMembershipDependency
                {
                    Membership = this,
                    Person = person,
                    Type = BDependencyType.Person
                });
            }
            return this;
        }

        #endregion Helper Methods
    }
}