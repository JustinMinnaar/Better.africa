using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public static class NodeExtensions
    {
        public static CNode ToNode(this object obj)
        {
            if (obj == null) return null;

            var t = obj.GetType();
            var p = t.GetProperties();
        }
    }

    [Nickname("beneficiary")]
    public class DetailBeneficiary
    {
        public string Email { get; set; }
        public string Identity { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Ratio { get; set; }

        public CNode ToNode()
        {
            var node = new CNode(this.ToNickname());
        }

        public static DetailBeneficiary FromNode(CNode child)
        {
            var b = new DetailBeneficiary
            {
                Ratio = child.TryGetString("ratio"),
                Name = child.TryGetString("name"),
                Phone = child.TryGetString("phone"),
                Email = child.TryGetString("email"),
                Identity = child.TryGetString("identity"),
            };

            return b;
        }
    }
}