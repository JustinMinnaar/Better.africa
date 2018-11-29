using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Benefits.Entities
{
    public class EntityErrors : Dictionary<string, string>
    {
        internal void AddError(string fieldName, string message)
        {
            if (message != null)
                Add(fieldName, message);
        }

        internal void AddError(string v, object signDateError)
        {
            throw new NotImplementedException();
        }
    }

    public class Member : BaseEntity
    {
        public string Number { get; set; }

        public DateTime? SignDate { get; set; }

        public string SignDateError
        {
            get
            {
                if (SignDate < new DateTime(2018, 12, 1) || SignDate > new DateTime(2028, 1, 1))
                    return "Sign date is required and must be on or after 2018-12-01 and before 2028-12-01.";
                else
                    return null;
            }
        }

        public DateTime? InceptionDate { get; set; }

        public string InceptionDateError
        {
            get
            {
                if (SignDate == null || SignDate < new DateTime(2018, 12, 1))
                    return "Sign date is required and must be on or after 2018-12-01";

                if (InceptionDate == null || InceptionDate < new DateTime(2018, 12, 1))
                    return "Inception date is required and must be on or after 2018-12-01";

                InceptionDate = new DateTime(InceptionDate.Value.Year, InceptionDate.Value.Month, 1);

                if (InceptionDate < SignDate.Value.AddDays(-7))
                    return "Inception Date cannot be more than 7 days before the sign date.";

                if (InceptionDate > SignDate.Value.AddYears(1))
                    return "Inception Date cannot be more than a year after the sign date.";

                return null;
            }
        }

        public ICollection<Person> People { get; } = new HashSet<Person>();

        #region BeforeSave

        public override void BeforeSave()
        {
            Errors.AddError(nameof(SignDate), SignDateError);
            Errors.AddError(nameof(InceptionDate), InceptionDateError);
            Errors.AddError(nameof(Principal), PrincipalError);
            Errors.AddError(nameof(Spouse), SpouseError);

            foreach (var child in People)
            {
                child.BeforeSave();
            }
        }

        [NotMapped]
        public string PrincipalError
        {
            get
            {
                var principalValid = People.Count(p => p.MembershipType == PersonMembershipTypes.Principal) == 1;
                if (!principalValid) return "There must be one principal.";

                if (InceptionDate != null)
                {
                    // Unless valid, the name will not be displayed on the member form.
                    var principalYears = Principal.AgeInYearsAsAt(InceptionDate.Value);
                    if (principalYears < 18 || principalYears > 65)
                        return "Principal must be between 18 and 65 years old on inception date.";
                }

                return null;
            }
        }

        [NotMapped]
        private string SpouseError
        {
            get
            {
                var spouseValid = People.Count(p => p.MembershipType == PersonMembershipTypes.Spouse) <= 1;
                if (!spouseValid)
                    return "There may not be more than one spouse.";

                if (InceptionDate != null)
                {
                    var spouseYears = Principal.AgeInYearsAsAt(InceptionDate.Value);
                    if (spouseYears < 18 || spouseYears > 65)
                        return "Spouse must be between 18 and 65 years old on inception date.";
                }

                return null;
            }
        }

        #endregion BeforeSave

        #region Helper Properties

        [NotMapped]
        public Person Principal => People.FirstOrDefault(p => p.MembershipType == PersonMembershipTypes.Principal);

        [NotMapped]
        public Person Spouse => People.FirstOrDefault(p => p.MembershipType == PersonMembershipTypes.Spouse);

        [NotMapped]
        public IList<Person> Children => People.Where(p => p.MembershipType == PersonMembershipTypes.Child).ToList();

        [NotMapped]
        public IList<Person> Extended => People.Where(p => p.MembershipType == PersonMembershipTypes.Family).ToList();

        #endregion Helper Properties

        #region Helper Methods

        public Member WithPrincipal(Person principal)
        {
            principal.MembershipType = PersonMembershipTypes.Principal;
            People.Add(principal);
            return this;
        }

        public Member WithSpouse(Person spouse)
        {
            spouse.MembershipType = PersonMembershipTypes.Spouse;
            People.Add(spouse);
            return this;
        }

        public Member WithChildren(params Person[] children)
        {
            foreach (var child in children)
            {
                child.MembershipType = PersonMembershipTypes.Child;
                People.Add(child);
            }
            return this;
        }

        public Member WithFamily(params Person[] family)
        {
            foreach (var child in family)
            {
                child.MembershipType = PersonMembershipTypes.Family;
                People.Add(child);
            }
            return this;
        }

        #endregion Helper Methods
    }
}