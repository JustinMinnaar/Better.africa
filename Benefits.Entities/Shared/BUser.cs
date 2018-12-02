using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Entities
{
    public class BUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
    }
}