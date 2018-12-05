using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipArea : IImportExport
    {
        public string Postal { get; set; }
        public string City { get; set; }
        public string State { get; set; } = "Gauteng";
        public string Country { get; set; } = "South Africa";
        public string Code { get; set; }

        public void Import(CNode node)
        {
            Postal = node.TryGetString("postal");
            City = node.TryGetString("city");
            State = node.TryGetString("state");
            Country = node.TryGetString("country");
            Code = node.TryGetString("code");
        }

        public void Export(CNode node)
        {
            node.SetString("postal", Postal);
            node.SetString("city", City);
            node.SetString("state", State);
            node.SetString("country", Country);
            node.SetString("code", Code);
        }
    }
}