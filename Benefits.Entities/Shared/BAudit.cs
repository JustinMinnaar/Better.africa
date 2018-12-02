using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Entities
{
    public class BAudit
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public BEntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public DateTime When { get; set; }
        public BAuditAction Action { get; set; }
    }

    public enum BAuditAction : short
    {
    }

    public enum BEntityType : short
    {
    }
}