using Benefits.Shared;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benefits.Entities
{
    // TODO: Check each month if any member is going to soon be too old to be covered
    // TODO: A child becomes extended member when to old to be a child, or scholar.
    public enum MembershipType
    {
        /// <summary>No membership</summary>
        Person = 0,

        /// <summary>The principal member is responsible for payment and receives payouts unless he/she dies.</summary>
        Principal = 1,

        /// <summary></summary>
        Spouse = 2,

        /// <summary></summary>
        Child = 3,

        /// <summary></summary>
        Family = 4
    }

    /// <summary>
    ///     A person that may be a member of a membership.
    /// </summary>
    public class Person : BaseEntity
    {
        public string Err => $"{MembershipType} '{Name}'";

        public Guid MembershipId { get; set; }

        /// <summary></summary>
        public Membership Membership { get; set; }

        public string MembershipError
        {
            get
            {
                if (MembershipType != MembershipType.Person)
                {
                    if (Membership == null)
                        return $"A member account must be assigned for '{Name}'.";
                }

                return null;
            }
        }

        /// <summary>This is Person if not a member, or Principal, Spouse, Child or Family member if a member.</summary>
        public MembershipType MembershipType { get; set; }

        /// <summary>True if the child is studying at a tertiary institution, which allows child to be covered while studying.</summary>
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

        public string DateOfBirthError
        {
            get
            {
                if (DateOfBirth != null)
                {
                    var min = Clock.Now.AddYears(-100);
                    var max = Clock.Now.AddYears(10); // system lifespan is 10 years
                    if (DateOfBirth < min && DateOfBirth > max)
                        return $"must be after {min} (under 100 years old) and before {max}.";
                }

                return null;
            }
        }

        public float AgeInYearsAsAt(DateTime date)
        {
            if (DateOfBirth == null) return 0;

            var ageInYears = (date - DateOfBirth.Value).Days / 365.25f;

            return ageInYears;
        }

        public DateTime? DateOfDeath { get; set; }

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            errors.Add(nameof(Name), NameError);
            errors.Add(nameof(Identity), IdentityError);
            errors.Add(nameof(Membership), MembershipError);
            Errors.Add(nameof(DateOfBirth), DateOfBirthError);
        }
    }

    public enum PersonGenders
    {
        Male,
        Female
    }
}