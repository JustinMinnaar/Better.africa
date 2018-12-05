using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class DetailProductHamper : IDetailProduct
    {
        public bool? Savings { get; set; }

        public static IDetailProduct ReadDetail(CNode child)
        {
            return new DetailProductHamper
            {
                Savings = child.TryGetBoolean("savings"),
            };
        }
    }
}