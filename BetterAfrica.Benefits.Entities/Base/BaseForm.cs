using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public interface IToNode
    {
        CNode ToNode(string nickname = null);
    }

    public abstract class BaseForm<T> : IImportExport where T : class, IImportExport, IToNode, new()
    {
        public CNode ToNode(string nickname = null)
        {
            var node = new CNode(nickname ?? this.ToNickname());
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
        {
            this.ExportProperties(node);
        }

        public virtual void Import(CNode node)
        {
            this.ImportProperties(node);
        }
    }
}