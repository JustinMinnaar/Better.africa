using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipCommunication : IImportExport
    {
        public EReceivePaper? ReceiveLetters { get; set; }
        public bool? ReceiveSms { get; set; }
        public EHomeLanguage? HomeLanguage { get; set; }

        public void Import(CNode node)
        {
            ReceiveLetters = node.TryGetEnum<EReceivePaper>("ReceiveLetters");
            ReceiveSms = node.TryGetBoolean("ReceiveSms");
            HomeLanguage = node.TryGetEnum<EHomeLanguage>("HomeLanguage");
        }

        public void Export(CNode node)
        {
            if (ReceiveLetters != null) node.SetEnum<EReceivePaper>("ReceiveLetters", ReceiveLetters.Value);
            node.SetString("ReceiveSms", ReceiveSms);
            if (HomeLanguage != null) node.SetEnum<EHomeLanguage>("HomeLanguage", HomeLanguage.Value);
        }
    }
}