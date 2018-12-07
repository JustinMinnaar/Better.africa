using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("membership")]
    public class FormMembership : BaseForm<FormMembership>, IToNode
    {
        public string Err => Detail.Err + " " + Principal.Err;

        public FormMembershipDetail Detail { get; set; } //= new FormMembershipDetail();
        public FormMembershipArea Area { get; set; } //= new FormMembershipArea();
        public FormMembershipCommunication Communication { get; set; } //= new FormMembershipCommunication();
        public FormMembershipPerson Principal { get; set; } //= new FormMembershipPerson();
        public FormMembershipPerson Spouse { get; set; }
        public virtual ICollection<FormMembershipPerson> Children { get; } = new HashSet<FormMembershipPerson>();
        public virtual ICollection<FormMembershipPerson> Family { get; } = new HashSet<FormMembershipPerson>();
        public virtual ICollection<FormMembershipBeneficiary> Beneficiaries { get; } = new HashSet<FormMembershipBeneficiary>();
        public virtual ICollection<FormMembershipPackage> Packages { get; } = new HashSet<FormMembershipPackage>();

        public override void Export(CNode node)
        {
            Detail?.Export(node);

            node.AddChild(Principal?.ToNode("principal"));
            node.AddChild(Spouse?.ToNode("spouse"));
            foreach (var child in Children) { node.AddChild(child.ToNode("child")); }
            foreach (var family in Family) { node.AddChild(family.ToNode("family")); }
            node.AddChild(Area?.ToNode("area"));
            node.AddChild(Communication?.ToNode("communication"));
            foreach (var beneficiary in Beneficiaries) { node.AddChild(beneficiary.ToNode("beneficiary")); }
            foreach (var package in Packages) { node.AddChild(package.ToNode()); }
        }

        public override void Import(CNode node)
        {
            Detail = FormMembershipDetail.FromNode(node);

            foreach (var childNode in node.Children)
            {
                switch (childNode.Type.ToLower())
                {
                    case "principal": Principal = FormMembershipPerson.FromNode(childNode); break;
                    case "spouse": Spouse = FormMembershipPerson.FromNode(childNode); break;
                    case "child": Children.Add(FormMembershipPerson.FromNode(childNode)); break;
                    case "family": Family.Add(FormMembershipPerson.FromNode(childNode)); break;
                    case "beneficiary": Beneficiaries.Add(FormMembershipBeneficiary.FromNode(childNode)); break;
                    case "package": Packages.Add(FormMembershipPackage.FromNode(childNode)); break;
                    case "communication": Communication = FormMembershipCommunication.FromNode(childNode); break;
                    case "area": Area = FormMembershipArea.FromNode(childNode); break;
                    default:
                        throw new BenefitsException("Unknown node " + childNode.Err);
                }
                childNode.ThrowUnknownAttributes();
            }
        }
    }
}