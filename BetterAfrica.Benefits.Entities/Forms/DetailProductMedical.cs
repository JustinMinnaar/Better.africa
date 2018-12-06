using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("medical")]
    public class DetailProductMedical : IDetailProduct
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

        public static DetailProductMedical ReadDetail(CNode child)
        {
            return new DetailProductMedical
            {
                HasTransport = child.TryGetBoolean("hasTransport"),
                HasEmergency = child.TryGetBoolean("hasEmergency"),
                DailyCover = child.TryGetDecimal("dailyCover"),
            };
        }
    }
}