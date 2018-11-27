using System;
using System.Collections.Generic;

namespace Better.Benefits.Provider
{
    public class PolicyModel
    {
        public RowModel Row { get; internal set; }

        public Guid PolicyId { get; internal set; }
        public Guid OwnerPersonId { get; set; }
        public PersonModel OwnerModel { get; set; }
        public string Code { get; set; }
        public EPolicyStatus Status { get; set; }
        public EPolicyType Type { get; set; }
        public IList<PolicyMemberModel> Members { get; } = new List<PolicyMemberModel>();
    }

    public class PolicyMemberModel
    {
        public RowModel Row { get; internal set; }

        public Guid PolicyId { get; set; }
        public Guid PersonId { get; set; }
        public bool IsDependant { get; set; }
    }

    public enum EPolicyStatus : byte
    {
    }

    public enum EPolicyType : byte
    {
    }
}