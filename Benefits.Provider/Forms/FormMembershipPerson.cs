using System;
using Benefits.Entities;
using Knights.Core.Common;

namespace Benefits.Provider.Forms
{
    public class FormMembershipPerson
    {
        public string Err => $"name={Name}";

        public string Name => LastName.Comma(FirstName);

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime? DateOfBirth { get; internal set; }
        public DateTime? DateOfDeath { get; internal set; }
        public BPersonGenders? Gender { get; internal set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string WorkName { get; internal set; }
        public string EmailAddress { get; set; }
        public bool? Scholar { get; set; }
        public string SchoolName { get; internal set; }
        public string Email { get; internal set; }
    }
}