using System;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>
    ///     A person of a particular type to be included in the membership.
    /// </summary>
    public class BMembershipDependency : BDependency
    {
        /// <summary></summary>
        public Guid MembershipId { get; set; }

        /// <summary></summary>
        public virtual BMembership Membership { get; set; }
    }
}