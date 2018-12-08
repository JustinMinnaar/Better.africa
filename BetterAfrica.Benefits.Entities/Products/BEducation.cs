using BetterAfrica.Shared;
using Knights.Core.Common;

namespace BetterAfrica.Benefits.Entities
{
    [Nickname("account")]
    public class BMemberEducation : BaseRow
    {
        public long? PlanId { get; set; }

        public virtual BPlanEducation Plan { get; set; }
    }

    [Nickname("plan-education")]
    public class BPlanEducation : BaseRow
    {
    }
}