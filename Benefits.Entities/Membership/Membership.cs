using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Benefits.Entities
{
    public class Membership : Contract
    {
        public ICollection<Person> People { get; } = new HashSet<Person>();

        #region BeforeSave

        public override void BeforeSave(EntityErrors errors)
        {
            base.BeforeSave(errors);

            errors.Add(nameof(Principal), PrincipalError);
            errors.Add(nameof(Spouse), SpouseError);
            foreach (var person in People)
            {
                person.BeforeSave(errors);
            }
        }

        public string PrincipalError
        {
            get
            {
                var principalValid = People.Count(p => p.MembershipType == MembershipType.Principal) == 1;
                if (!principalValid) return "There must be one principal.";


                var principalYears = Principal.AgeInYearsAsAt(InceptionDate.Value);
                if (principalYears < 18 || principalYears > 65)
                    return "Principal must be between 18 and 65 years old on inception date.";

                return null;
            }
        }

        public string SpouseError
        {
            get
            {
                var spouseValid = People.Count(p => p.MembershipType == MembershipType.Spouse) <= 1;
                if (!spouseValid)
                    return "There may not be more than one spouse.";

                var spouseYears = Spouse.AgeInYearsAsAt(InceptionDate.Value);
                if (spouseYears < 18 || spouseYears > 65)
                    return "Spouse must be between 18 and 65 years old on inception date.";

                return null;
            }
        }

        #endregion BeforeSave

        #region Helper Properties

        [NotMapped]
        public Person Principal => People.FirstOrDefault(p => p.MembershipType == MembershipType.Principal);

        [NotMapped]
        public Person Spouse => People.FirstOrDefault(p => p.MembershipType == MembershipType.Spouse);

        [NotMapped]
        public IList<Person> Children => People.Where(p => p.MembershipType == MembershipType.Child).ToList();

        [NotMapped]
        public IList<Person> Extended => People.Where(p => p.MembershipType == MembershipType.Family).ToList();

        #endregion Helper Properties

        #region Helper Methods

        public Membership WithPrincipal(Person principal)
        {
            // can't add person twice during testing
            if (principal.MembershipType != MembershipType.Person)
                throw new BenefitsException(principal.Name);

            principal.MembershipType = MembershipType.Principal;
            principal.Membership = this;
            People.Add(principal);
            return this;
        }

        public Membership WithSpouse(Person spouse)
        {
            // can't add person twice during testing
            if (spouse.MembershipType != MembershipType.Person)
                throw new BenefitsException(spouse.Name);

            spouse.MembershipType = MembershipType.Spouse;
            spouse.Membership = this;
            People.Add(spouse);
            return this;
        }

        public Membership WithChildren(params Person[] children)
        {
            foreach (var child in children)
            {
                // can't add person twice during testing
                if (child.MembershipType != MembershipType.Person)
                    throw new BenefitsException(child.Name);

                child.MembershipType = MembershipType.Child;
                child.Membership = this;
                People.Add(child);
            }
            return this;
        }

        public Membership WithFamily(params Person[] family)
        {
            foreach (var person in family)
            {
                // can't add person twice during testing
                if (person.MembershipType != MembershipType.Person)
                    throw new BenefitsException(person.Name);

                person.MembershipType = MembershipType.Family;
                person.Membership = this;
                People.Add(person);
            }
            return this;
        }

        #endregion Helper Methods
    }
}