using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("medical")]
    public class DetailProductMedical : BaseForm<DetailProductMedical>, IToNode
    {
        public bool? HasTransport { get; set; }
        public bool? HasEmergency { get; set; }
        public decimal? DailyCover { get; set; }

        public CNode Export()
        {
            var node = new CNode(this.ToNickname());
            node.SetBoolean("hasTransport", HasTransport);
            node.SetBoolean("hasEmergency", HasEmergency);
            node.SetDecimal("dailyCover", DailyCover);
            return node;
        }

        public override void Import(CNode node)
        {
            base.Import(node);

            HasTransport = node.TryGetBoolean("hasTransport");
            HasEmergency = node.TryGetBoolean("hasEmergency");
            DailyCover = node.TryGetDecimal("dailyCover");
        }
    }
}