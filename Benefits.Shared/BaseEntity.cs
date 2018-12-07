using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetterAfrica.Shared
{
    public enum WorkflowStatuses : byte
    {
        New = 0, Pending, Approved, Rejected, Deleted,
    }

    public abstract class BaseRow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pk { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
    }

    public abstract class BaseEntity : BaseRow
    {
        public int RowVersion { get; set; } = 1;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public Guid? CreatedById { get; set; }

        public WorkflowStatuses WorkflowStatus { get; set; }

        public Guid WorkflowByUserId { get; set; }

        public DateTime? WorkflowOn { get; set; }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
            private set { }
        }

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