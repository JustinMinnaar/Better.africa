using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("hamper")]
    public class DetailProductHamper : IDetailProduct
    {
        public decimal? Savings { get; set; }

        public CNode Export()
        {
            var node = new CNode(this.ToNickname());
            node.SetDecimal("savings", Savings);
            return node;
        }

        public static IDetailProduct ReadDetail(CNode child)
        {
            return new DetailProductHamper
            {
                Savings = child.TryGetDecimal("savings"),
            };
        }
    }
}