using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class DetailProduct<T> : BaseForm<T>, IToNode where T : class, IImportExport, IToNode, new()
    {
        public decimal? Savings { get; set; }

        public CNode Export()
        {
            var node = new CNode(this.ToNickname());
            node.SetDecimal("savings", Savings);
            return node;
        }

        public override void Import(CNode node)
        {
            base.Import(node);
            Savings = node.TryGetDecimal("savings");
        }
    }
}