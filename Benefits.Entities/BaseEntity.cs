using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benefits.Entities
{
    public enum WorkflowStatuses : byte { New = 0, Pending, Approved, Rejected, Deleted }

    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public WorkflowStatuses WorkflowStatus { get; set; }

        public int RowVersion { get; set; } = 1;

        [NotMapped]
        public EntityErrors Errors = new EntityErrors();

        public abstract void BeforeSave();
    }
}