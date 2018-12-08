using Knights.Core.Nodes;

namespace BetterAfrica.Shared
{
    public static class BaseExtensions<T> where T : class, IImportExport, new()
    {
        public static T FromNode(CNode node)
        {
            var m = new T();
            m.Import(node);
            return m;
        }
    }
}