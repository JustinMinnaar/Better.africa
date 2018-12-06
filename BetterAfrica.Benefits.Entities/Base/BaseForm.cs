using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public abstract class BaseForm<T> : IImportExport where T : class, IImportExport, new()
    {
        public CNode ToNode()
        {
            var node = new CNode(this.ToNickname());
            Export(node);
            return node;
        }

        public static T FromNode(CNode node)
        {
            var m = new T();
            m.Import(node);
            return m;
        }

        public virtual void Export(CNode node)
        { }

        public virtual void Import(CNode node)
        { }
    }
}