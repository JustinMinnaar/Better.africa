using System;
using Benefits.Entities;

namespace Benefits.Provider.Forms
{
    public class DetailPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string EmailAddress { get; set; }
        public string EmployedAt { get; set; }
        public string EmployedAtPhone { get; set; }
        public DateTime? DateOfBirth { get; internal set; }
        public DateTime? DateOfDeath { get; internal set; }
        public BPersonGenders? Gender { get; internal set; }
    }
}