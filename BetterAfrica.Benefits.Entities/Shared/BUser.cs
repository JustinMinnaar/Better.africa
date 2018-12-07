using BetterAfrica.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAfrica.Benefits.Entities
{
    public class BUser : BaseEntity
    {
        public string Name { get; set; }

        public string Hash { get; set; }

        public string Role { get; set; }
    }
}