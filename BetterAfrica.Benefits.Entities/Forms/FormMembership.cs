using Benefits.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("membership")]
    public class FormMembership : Form
    {
        public string Err => Detail.Err + " " + Principal.Err;

        public FormMembershipDetail Detail { get; set; }
        public FormMembershipArea Area { get; set; }
        public FormMembershipCommunication Communication { get; set; }
        public FormMembershipPerson Principal { get; set; }
        public FormMembershipPerson Spouse { get; set; }
        public List<FormMembershipPerson> Children { get; } = new List<FormMembershipPerson>();
        public List<FormMembershipPerson> Family { get; } = new List<FormMembershipPerson>();
        public List<DetailBeneficiary> Beneficiaries { get; } = new List<DetailBeneficiary>();
        public List<FormMembershipPackage> Packages { get; } = new List<FormMembershipPackage>();

        public CNode ToNode()
        {
            var node = new CNode(this.ToNickname());
            Export(node);
            return node;
        }

        public static FormMembership FromNode(CNode node)
        {
            var m = new FormMembership();
            m.Import(node);
            return m;
        }

        public override void Export(CNode node)
        {
            node.AddChild(Principal.tonode())
        }

        public override void Import(CNode node)
        {
            Detail.Import(node);

            foreach (var child in node.Children)
            {
                switch (child.Type.ToLower())
                {
                    case "principal": Principal = FormMembershipPerson.ReadDetail(child); break;
                    case "spouse": Spouse = FormMembershipPerson.ReadDetail(child); break;
                    case "child": Children.Add(FormMembershipPerson.ReadDetail(child)); break;
                    case "family": Family.Add(FormMembershipPerson.ReadDetail(child)); break;
                    case "beneficiary": Beneficiaries.Add(DetailBeneficiary.FromNode(child)); break;
                    case "package": Packages.Add(FormMembershipPackage.ReadDetail(child)); break;
                    default:
                        throw new BenefitsException("Unknown node " + child.Err);
                }
                child.ThrowUnknownAttributes();
            }
        }
    }
}