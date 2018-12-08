using BetterAfrica.Shared;
using System;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>Only exists as an extension to a person record.</summary>
    public class BPersonAgent : BaseRow
    {
        public int PersonId { get; set; }

        public virtual BPerson Person { get; set; }

        public DateTime? AgentBeginDate { get; set; }

        public DateTime? AgentEndDate { get; set; }
    }
}