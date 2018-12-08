using BetterAfrica.Shared;
using System;

namespace BetterAfrica.Benefits.Entities
{
    public abstract class BContractDependency : BaseRow
    {
        public int PersonId { get; set; }

        public virtual BPerson Person { get; set; }

        /// <summary>The person's dependency.
        /// A policy may cover a different person as Principal than their Member specifies.
        /// For example, extended family member may need their own policy for medical cover.
        /// </summary>
        public BDependencyType Type { get; set; }
    }
}