using BetterAfrica.Benefits.Entities.Forms;
using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("funeral")]
    public class BPlanPolicyFuneral : BaseEntity
    {
        public decimal? Cover { get; set; }

        public CNode Export()
        {
            var node = new CNode(this.ToNickname());
            node.SetDecimal("cover", Cover);
            return node;
        }

        public override void Import(CNode node)
        {
            base.Import(node);

            Cover = node.TryGetDecimal("cover");
        }
    }
}