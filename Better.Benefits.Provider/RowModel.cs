using System;
using Better.Benefits.Data;

namespace Better.Benefits.Provider
{
    public class RowModel
    {
        public RowModel(Guid id, Row row)
        {
            this.Id = id;
            this.Archived = row.Archived;
            this.ArchivedOn = row.ArchivedOn;
            this.Deleted = row.Deleted;
            this.DeletedParent = row.DeletedParent;
            this.ModifiedBy = row.ModifiedBy;
            this.ModifiedOn = row.ModifiedOn;
            this.Version = row.Version;
        }

        public Guid Id { get; internal set; }
        public bool Archived { get; internal set; }
        public DateTime? ArchivedOn { get; internal set; }
        public bool Deleted { get; internal set; }
        public bool DeletedParent { get; internal set; }
        public Guid ModifiedBy { get; internal set; }
        public DateTime ModifiedOn { get; internal set; }
        public int Version { get; internal set; }
        public bool IsValid { get; internal set; }
    }
}