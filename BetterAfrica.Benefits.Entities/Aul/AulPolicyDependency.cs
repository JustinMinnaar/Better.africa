using System;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>
    ///     A person of a particular type to be covered by the policy.
    /// </summary>
    public class AulPolicyDependency : BContractDependency
    {
        public int PolicyId { get; set; }
        public AulPolicy Policy { get; set; }
    }
}