using BetterAfrica.Shared;
using Knights.Core.Common;

namespace BetterAfrica.Benefits.Entities
{
    [Nickname("medical")]
    public class BPlanPolicyMedical : BaseEntity
    {
        public bool? HasTransport { get; set; }
        public bool? HasEmergency { get; set; }
        public decimal? DailyCover { get; set; }
    }
}