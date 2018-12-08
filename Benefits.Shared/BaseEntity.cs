using Knights.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace BetterAfrica.Shared
{
    public abstract class BaseEntity : BaseRow
    {
        public int EntityVersion { get; set; } = 1;

        /// <summary>The user that is last worked with this entity.</summary>
        public int? EntityUserId { get; set; }

        public DateTime EntityModifiedOn { get; set; } = Clock.Now;

        public EWorkflowStatuses EntityWorkflowStatus { get; set; }

        public Guid EntityWorkflowByUserId { get; set; }

        public DateTime? EntityWorkflowOn { get; set; }
    }
}