using Benefits.Shared;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benefits.Entities
{
    // TODO: Check each month if any member is going to soon be too old to be covered
    // TODO: A child becomes extended member when to old to be a child, or scholar.
    public enum PersonType { Person = 0, Principal = 1, Spouse = 2, Child = 3, Family = 4 }

    public class Person : BaseEntity
    {
        public string Err => $"{Type} '{Name}'";

        public Guid MembershipId { get; set; }

        public Membership Membership { get; set; }

        public PersonType Type { get; set; }

        public bool IsScholar { get; set; }

        #region Name

        public string Name => (NameFirst + " " + NameLast).Trim();

        public string NameError
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return "Name is required";

                if (Name.Length > 100)
                    return "Name cannot exceed 100 characters.";

                return null;
            }
        }

        public string NameFirst { get; set; }

        public string NameLast { get; set; }

        #endregion Name

        #region Identity

        public string Identity { get; set; }

        public string IdentityError
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Identity))
                    return "Identity is required";

                if (Identity.Length > 13)
                    return "Identity must be 13 characters for South African Identity.";

                return null;
            }
        }

        #endregion Identity

        public PersonGenders Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public float AgeInYearsAsAt(DateTime date)
        {
            if (DateOfBirth == null) return 0;

            var ageInYears = (date - DateOfBirth.Value).Days / 365.25f;

            return ageInYears;
        }

        public DateTime? DateOfDeath { get; set; }

        public override void BeforeSave(EntityErrors errors)
        {
            base.BeforeSave(errors);

            if (Membership == null)
                errors.Add(nameof(Membership.People), $"A member account must be assigned for '{Name}'.");
        }
    }

    public enum PersonGenders
    {
        Male,
        Female
    }
}