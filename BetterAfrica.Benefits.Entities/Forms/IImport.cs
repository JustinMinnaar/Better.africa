using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public interface IImportExport
    {
        void Export(CNode node);

        void Import(CNode node);
    }
}