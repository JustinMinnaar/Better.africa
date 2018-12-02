using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benefits.Shared
{
    public enum WorkflowStatuses : byte { New = 0, Pending, Approved, Rejected, Deleted }

    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public Guid? CreatedById { get; set; }

        public int RowVersion { get; set; } = 1;

        public WorkflowStatuses WorkflowStatus { get; set; }

        public bool IsValid => Errors.Count == 0;

        [NotMapped]
        public EntityErrors Errors
        {
            get
            {
                var errors = new EntityErrors();
                BeforeSave(errors);
                return errors;
            }
        }

        public void BeforeSave(EntityErrors errors)
        {
            BeforeSaveOverride(errors);
        }

        protected virtual void BeforeSaveOverride(EntityErrors errors)
        {
        }
    }
}