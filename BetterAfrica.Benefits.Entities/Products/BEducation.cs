using BetterAfrica.Benefits.Entities.Forms;
using BetterAfrica.Shared;
using Knights.Core.Common;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("account")]
    public class BMemberAccount : BaseRow
    {
        public long? PlanId { get; set; }

        public virtual BPlanEducation Plan { get; set; }

        public ICollection<BMemberEducationTransaction>
    }

    [Nickname("plan-education")]
    public class BPlanEducation : BaseRow
    {
    }
}