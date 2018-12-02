using System;

namespace Benefits.Entities
{
    /// <summary>
    ///     A person of a particular type to be covered by the policy.
    /// </summary>
    public class AulPolicyDependency : BDependency
    {
        public Guid PolicyId { get; set; }
        public BAulPolicy Policy { get; set; }
    }
}