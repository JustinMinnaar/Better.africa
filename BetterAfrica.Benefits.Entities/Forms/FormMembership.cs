using Benefits.Shared;
using Knights.Core.Nodes;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembership
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

        public static IEnumerable<FormMembership> ReadMemberships(string xmlPath)
        {
            var node = CNode.FromXmlFile(xmlPath);
            return ReadMemberships(node);
        }

        public static IEnumerable<FormMembership> ReadMemberships(CNode node)
        {
            if (node.Type != "memberships")
                throw new BenefitsException("Unknown node " + node.Err);

            foreach (var child in node.Children)
            {
                if (node.Type == "membership")
                {
                    var m = ReadMembership(node);
                    node.ThrowUnknownAttributes();
                    yield return m;
                }
                else throw new BenefitsException("Unknown node " + node.Err);
            }
        }

        public static FormMembership ReadMembership(CNode node)
        {
            var m = new FormMembership
            {
                Detail = FormMembershipDetail.ReadDetail(node)
            };

            foreach (var child in node.Children)
            {
                switch (child.Type.ToLower())
                {
                    case "principal": m.Principal = FormMembershipPerson.ReadDetail(child); break;
                    case "spouse": m.Spouse = FormMembershipPerson.ReadDetail(child); break;
                    case "child": m.Children.Add(FormMembershipPerson.ReadDetail(child)); break;
                    case "family": m.Family.Add(FormMembershipPerson.ReadDetail(child)); break;
                    case "beneficiary": m.Beneficiaries.Add(DetailBeneficiary.FromNode(child)); break;
                    case "package": m.Packages.Add(FormMembershipPackage.ReadDetail(child)); break;
                    default:
                        throw new BenefitsException("Unknown node " + child.Err);
                }
                child.ThrowUnknownAttributes();
            }

            return m;
        }
    }
}