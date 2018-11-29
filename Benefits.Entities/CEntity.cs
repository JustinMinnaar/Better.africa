using System;

namespace Benefits.Entities
{
    public abstract class CEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public enum Statuses { New, Pending, Approved, Rejected, Deleted }

        public Statuses Status { get; set; }

        public byte[] RowVersion { get; set; }
    }
}