using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Benefits.Entities
{
    public class BMembership : BContract
    {
        public virtual ICollection<BPerson> People { get; } = new HashSet<BPerson>();

        #region BeforeSave

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            errors.Add(nameof(PeoplePrincipal), PrincipalError);
            errors.Add(nameof(PeopleSpouse), SpouseError);
            foreach (var person in People)
            {
                person.BeforeSave(errors);
            }
        }

        public string PrincipalError
        {
            get
            {
                var principalValid = People.Count(p => p.MembershipType == BMembershipType.Principal) == 1;
                if (!principalValid) return "There must be one principal.";

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

                var spouseValid = People.Count(p => p.MembershipType == BMembershipType.Spouse) == 1;
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
        public BPerson PeoplePrincipal => People.FirstOrDefault(p => p.MembershipType == BMembershipType.Principal);

        [NotMapped]
        public BPerson PeopleSpouse => People.FirstOrDefault(p => p.MembershipType == BMembershipType.Spouse);

        [NotMapped]
        public IList<BPerson> PeopleChildren => People.Where(p => p.MembershipType == BMembershipType.Child).ToList();

        [NotMapped]
        public IList<BPerson> PeopleExtended => People.Where(p => p.MembershipType == BMembershipType.Family).ToList();

        #endregion Helper Properties

        #region Helper Methods

        public BMembership WithPrincipal(BPerson principal)
        {
            // can't add person twice during testing
            if (principal.MembershipType != BMembershipType.Person)
                throw new BenefitsException(principal.Name);

            principal.MembershipType = BMembershipType.Principal;
            principal.Membership = this;
            People.Add(principal);
            return this;
        }

        public BMembership WithSpouse(BPerson spouse)
        {
            // can't add person twice during testing
            if (spouse.MembershipType != BMembershipType.Person)
                throw new BenefitsException(spouse.Name);

            spouse.MembershipType = BMembershipType.Spouse;
            spouse.Membership = this;
            People.Add(spouse);
            return this;
        }

        public BMembership WithChildren(params BPerson[] children)
        {
            foreach (var child in children)
            {
                // can't add person twice during testing
                if (child.MembershipType != BMembershipType.Person)
                    throw new BenefitsException(child.Name);

                child.MembershipType = BMembershipType.Child;
                child.Membership = this;
                People.Add(child);
            }
            return this;
        }

        public BMembership WithFamily(params BPerson[] family)
        {
            foreach (var person in family)
            {
                // can't add person twice during testing
                if (person.MembershipType != BMembershipType.Person)
                    throw new BenefitsException(person.Name);

                person.MembershipType = BMembershipType.Family;
                person.Membership = this;
                People.Add(person);
            }
            return this;
        }

        #endregion Helper Methods
    }
}