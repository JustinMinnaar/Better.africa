using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class DetailProductFuneral : IDetailProduct
    {
        public decimal? Cover { get; set; }

        public static DetailProductFuneral ReadDetail(CNode child)
        {
            return new DetailProductFuneral
            {
                Cover = child.TryGetDecimal("cover"),
            };
        }
    }
}