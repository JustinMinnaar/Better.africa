using Benefits.Shared;
using Knights.Core.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benefits.Entities
{
    /// <summary>
    ///     A person that may be a member of a membership.
    /// </summary>
    public class BPerson : BaseEntity
    {
        public string Err => $"{MembershipType} '{Name}'";

        public Guid? MembershipId { get; set; }

        /// <summary></summary>
        public virtual BMembership Membership { get; set; }

        public string MembershipError
        {
            get
            {
                if (MembershipType != BMembershipType.Person)
                {
                    if (Membership == null)
                        return $"A member account must be assigned for '{Name}'.";
                }

                return null;
            }
        }

        /// <summary>This is Person if not a member, or Principal, Spouse, Child or Family member if a member.</summary>
        public BMembershipType MembershipType { get; set; }

        /// <summary>True if the child is studying at a tertiary institution, which allows child to be covered while studying.</summary>
        public bool IsScholar { get; set; }

        #region Name

        public string Name => (NameFirst + " " + NameLast).Trim();

        public string NameError
        {
            get
            {
                var name = Name;

                if (string.IsNullOrWhiteSpace(name))
                    return "Name is required";

                if (name.Length > 100)
                    return "Name cannot exceed 100 characters.";

                return null;
            }
        }

        public string NameFirst { get; set; }

        public string NameLast { get; set; }

        #endregion Name

        #region Identity

        public string IdentityNumber { get; set; }

        public string IdentityNumberError
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IdentityNumber))
                    return $"{Err} requires Identity.";

                if (IdentityNumber.Length != 6 && IdentityNumber.Length != 13)
                    return $"{Err} requires Identity to be 6 characters (birth date only) or 13 characters for South Africa.";

                if (IdentityNumber.Length == 13)
                {
                    var saId = new SouthAfricanIdentityNumber { Number = IdentityNumber };
                    if (saId.IsValid)
                    {
                        DateOfBirth = saId.Birthdate;
                        if (saId.IsMale) Gender = BPersonGenders.Male;
                        if (saId.IsFemale) Gender = BPersonGenders.Female;
                    }
                    else
                    {
                        return $"{Err} requires a valid 13 character South African Identity Number.";
                    }
                }

                return null;
            }
        }

        #endregion Identity

        public BPersonGenders? Gender { get; set; }

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
            errors.Add(nameof(IdentityNumber), IdentityNumberError);
            errors.Add(nameof(Membership), MembershipError);
            errors.Add(nameof(DateOfBirth), DateOfBirthError);
        }

        public BPerson WithDateOfBirth(int yy, int mm, int dd)
        {
            DateOfBirth = new DateTime(yy, mm, dd);
            return this;
        }
    }
}