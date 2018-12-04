using System;
using System.Collections.Generic;

namespace Benefits.Provider.Forms
{
    public class FormMembershipPackage
    {
        public string Name { get; set; }
        public string Covers { get; set; }
        public string AgentCode { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? SignDate { get; set; }
        public List<IDetailProduct> Products { get; } = new List<IDetailProduct>();
    }
}