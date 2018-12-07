using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipArea : BaseForm<FormMembershipArea>, IToNode
    {
        public string Postal { get; set; }
        public string City { get; set; }
        public string State { get; set; } = "Gauteng";
        public string Country { get; set; } = "South Africa";
        public string Code { get; set; }

        public override void Import(CNode node)
        {
            base.Import(node);

            Postal = node.TryGetString("postal");
            City = node.TryGetString("city");
            State = node.TryGetString("state");
            Country = node.TryGetString("country");
            Code = node.TryGetString("code");
        }

        public override void Export(CNode node)
        {
            base.Export(node);

            node.SetString("postal", Postal);
            node.SetString("city", City);
            node.SetString("state", State);
            node.SetString("country", Country);
            node.SetString("code", Code);
        }
    }
}