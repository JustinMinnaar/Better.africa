using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>
    ///     Selects a plan for terms and pricing, applies dependencies and beneficiaries, for a period of time.
    ///     Maintains transactions against the account to determine the current balance of the account.
    /// </summary>
    public class BMemberAccount : BContract
    {
        public long? MemberId { get; set; }
        public virtual BMember Member { get; set; }

        public long PackageId { get; set; }
        public virtual BPackage Package { get; set; }

        public virtual ICollection<BMemberDependency> Dependencies { get; } = new HashSet<BMemberDependency>();

        public virtual ICollection<BMemberBeneficiary> Beneficiaries { get; } = new HashSet<BMemberBeneficiary>();
    }
}