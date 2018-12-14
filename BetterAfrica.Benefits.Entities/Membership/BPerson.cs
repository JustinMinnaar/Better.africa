using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Fluid.Datums;
using System;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>
    ///     A person that may be a member of a member.
    /// </summary>
    public class CPerson : BaseEntity
    {
        public string Err => Name;

        #region Code

        public string Code { get; set; }

        #endregion Code

        #region Name

        public string Name => (FirstName + " " + LastName).Trim();

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

        public string FirstName { get; set; }

        public string LastName { get; set; }

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
                        if (saId.IsMale) Gender = EPersonGenders.Male;
                        if (saId.IsFemale) Gender = EPersonGenders.Female;
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

        #region Gender

        public EPersonGenders? Gender { get; set; }

        #endregion Gender

        #region DateOfBirth

        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthError
        {
            get
            {
                if (DateOfBirth != null)
                {
                    var min = Clock.Now.AddYears(-100);
                    var max = Clock.Now;

                    if (DateOfBirth > max)
                        return $"can't be born in the future.";

                    if (DateOfBirth < min)
                        return $"must be after {min} (under 100 years old).";
                }

                return null;
            }
        }

        #endregion DateOfBirth

        #region DateOfDeath

        /// <summary>If the person dies, we note the date of death.</summary>
        public DateTime? DateOfDeath { get; set; }

        #endregion DateOfDeath

        #region AgeInYearsAsAt

        public float AgeInYearsAsAt(DateTime date)
        {
            if (DateOfBirth == null) return 0;

            var ageInYears = (date - DateOfBirth.Value).Days / 365.25f;

            return ageInYears;
        }

        #endregion AgeInYearsAsAt

        #region CellPhone

        public Phone CellPhone { get; set; }

        #endregion CellPhone

        #region HomePhone

        public Phone HomePhone { get; set; }

        #endregion HomePhone

        #region Work

        public string WorkName { get; set; }
        public Phone WorkPhone { get; set; }

        #endregion Work

        #region Email

        public string EmailAddress { get; set; }

        #endregion Email

        #region Scholar

        public bool? Scholar { get; set; }

        public string SchoolName { get; set; }

        #endregion Scholar

        #region Helpers

        public CPerson WithDateOfBirth(int yy, int mm, int dd)
        {
            DateOfBirth = new DateTime(yy, mm, dd);
            return this;
        }

        #endregion Helpers
    }
}