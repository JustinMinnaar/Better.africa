using System;

namespace Benefits.Entities
{
    public class Person : CEntity
    {
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public byte[] Photo { get; set; }

        //public Guid MemberId { get; set; }

        public Member Member { get; set; }

        public enum Types { Principal, Spouse, Child, Family }

        public Types Type { get; set; }
    }
}