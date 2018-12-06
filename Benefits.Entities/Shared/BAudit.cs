using BetterAfrica.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Entities
{
    public class BAudit : BaseRow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Guid UserId { get; set; }
        public BEntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public DateTime When { get; set; }
        public BAuditAction Action { get; set; }
        public string Description { get; set; }
    }

    public enum BAuditAction : short
    {
        Update,
        Create
    }

    public enum BEntityType : short
    {
        Person
    }
}