using BetterAfrica.Benefits.Entities.Forms;
using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("education")]
    public class DetailProductEducation : IDetailProduct
    {
        public bool? Savings { get; set; }

        public CNode Export()
        {
            var node = new CNode(this.ToNickname());
            node.SetBoolean("savings", Savings);
            return node;
        }

        public static DetailProductEducation FromNode(CNode child)
        {
            return new DetailProductEducation
            {
                Savings = child.TryGetBoolean("savings"),
            };
        }
    }
}