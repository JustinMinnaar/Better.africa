using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class DetailProductMedical : IDetailProduct
    {
        public bool? HasTransport { get; set; }
        public bool? HasEmergency { get; set; }
        public decimal? DailyCover { get; set; }

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