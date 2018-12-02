using Benefits.Shared;
using System;
using System.Collections.Generic;

namespace Benefits.Provider
{
    public class MembershipModel
    {
        public Guid? CreatedById { get; internal set; }
        public DateTime CreatedOn { get; internal set; }

        public Guid? AgentId { get; set; }

        public int Number { get; internal set; }

        public DateTime? InceptionDate { get; set; } = DateTime.Now.FirstDayOfNextMonth();
        public DateTime? SignDate { get; set; }

        public List<PersonModel> People { get; } = new List<PersonModel>();
    }
}