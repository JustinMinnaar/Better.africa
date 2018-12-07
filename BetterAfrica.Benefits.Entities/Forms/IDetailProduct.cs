using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class CDetailProduct<T> : BaseForm<T> where T : class, IImportExport, IToNode, new()
    {
    }
}