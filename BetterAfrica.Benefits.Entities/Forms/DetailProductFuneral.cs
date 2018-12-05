using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("funeral")]
    public class DetailProductFuneral : IDetailProduct
    {
        public decimal? Cover { get; set; }

        public CNode Export()
        {
            var node = new CNode(this.ToNickname());
            node.SetDecimal("cover", Cover);
            return node;
        }

        public static DetailProductFuneral FromNode(CNode child)
        {
            return new DetailProductFuneral
            {
                Cover = child.TryGetDecimal("cover"),
            };
        }
    }
}