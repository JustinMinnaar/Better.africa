using Benefits.Shared;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benefits.Entities
{
    public enum PersonMembershipTypes { Principal = 1, Spouse = 2, Child = 3, Family = 4 }

    public class Person : BaseEntity
    {
        [NotMapped]
        public string Name => (NameFirst + " " + NameLast).Trim();

        public string NameFirst { get; set; }

        public string NameLast { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public float AgeInYearsAsAt(DateTime date)
        {
            if (DateOfBirth == null) return 0;

            var ageInYears = (date - DateOfBirth.Value).Days / 365.25f;

            return ageInYears;
        }

        public int MaxAgeInYears => MembershipType == PersonMembershipTypes.Child ? 25 : 65;

        public DateTime? DateOfDeath { get; set; }

        public byte[] Photo { get; set; }

        //public Guid MemberId { get; set; }

        public Member Member { get; set; }

        public PersonMembershipTypes MembershipType { get; set; }

        public override void BeforeSave()
        {
            if (Member == null)
                Errors.Add(nameof(Member.People), "A member account must be assigned for '{child.Name}'.");

            if (AgeInYearsAsAt(Clock.Now) >= MaxAgeInYears)
                Errors.Add(nameof(Member.People), $"'{Name}' cannot be older than {MaxAgeInYears}.");
        }
    }
}