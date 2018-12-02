using System;

namespace Benefits.Entities
{
    public abstract class BDependency
    {
        public Guid PersonId { get; set; }
        public virtual BPerson Person { get; set; }

        /// <summary>The person's dependency.
        /// A policy may cover a different person as Principal than their Membership specifies.
        /// For example, extended family member may need their own policy for medical cover.
        /// </summary>
        public BDependencyType Type { get; set; }

        /// <summary>For a child studying at a tertiary institution, which allows child to be covered while studying.</summary>
        public bool IsTertiaryStudent { get; set; }
    }
}