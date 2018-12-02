using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Entities.Membership
{
    public class BAddress
    {
        public string PostalAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; } = "Gauteng";
        public string Country { get; set; } = "South Africa";
        public string Code { get; set; }
    }

    public class BCommunication
    {
        public BReceiveVia ReceiveNewsLetters { get; set; }
        public bool ReceiveSms { get; set; }
        public string HomeLanguage { get; set; }
    }

    public enum BReceiveVia
    {
    }
}