using System;
using System.Collections.Generic;

namespace Better.Benefits.Provider
{
    public class PersonModel
    {
        public RowModel Row { get; internal set; }

        public Guid PersonId { get; internal set; }
        public string IsStaff { get; internal set; }
        public string IsClient { get; internal set; }
        public string IsAgent { get; internal set; }
        public DateTime? Birthdate { get; internal set; }
        public IList<IdentityModel> Identities { get; } = new List<IdentityModel>();
    }
}