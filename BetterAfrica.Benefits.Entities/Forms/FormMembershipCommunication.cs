using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipCommunication : BaseForm<FormMembershipCommunication>, IToNode
    {
        public EReceivePaper? ReceiveLetters { get; set; }
        public bool? ReceiveSms { get; set; }
        public EHomeLanguage? HomeLanguage { get; set; }

        public override void Import(CNode node)
        {
            ReceiveLetters = node.TryGetEnum<EReceivePaper>("letters");
            ReceiveSms = node.TryGetBoolean("sms");
            HomeLanguage = node.TryGetEnum<EHomeLanguage>("language");
        }

        public override void Export(CNode node)
        {
            if (ReceiveLetters != null) node.SetEnum<EReceivePaper>("letters", ReceiveLetters.Value);
            node.SetBoolean("sms", ReceiveSms);
            if (HomeLanguage != null) node.SetEnum<EHomeLanguage>("language", HomeLanguage.Value);
        }
    }
}