using System;

namespace Benefits.Entities
{
    public class PersonModel
    {
        public PersonGenders Gender { get; internal set; }
        public string NameLast { get; internal set; }
        public string NameFirst { get; internal set; }
        public MembershipType MembershipType { get; internal set; }
        public bool IsScholar { get; internal set; }
        public string Identity { get; internal set; }
        public DateTime? DateOfDeath { get; internal set; }
        public DateTime? DateOfBirth { get; internal set; }
    }
}