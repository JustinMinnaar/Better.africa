using BetterAfrica.Shared;
using Knights.Core.Nodes;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public static class FormMemberships
    {
        public static CNode ToNode(IEnumerable<FormMembership> memberships)
        {
            var node = new CNode("memberships");
            foreach (var mem in memberships)
            {
                node.AddChild(mem.ToNode());
            }
            return node;
        }

        public static IEnumerable<FormMembership> FromXmlFile(string xmlPath)
        {
            var node = CNode.FromXmlFile(xmlPath);
            return ReadMemberships(node);
        }

        public static IEnumerable<FormMembership> FromXml(string xml)
        {
            var node = CNode.FromXml(xml);
            return ReadMemberships(node);
        }

        public static IEnumerable<FormMembership> ReadMemberships(CNode node)
        {
            if (node.Type != "memberships")
                throw new BenefitsException("Unknown node " + node.Err);

            foreach (var child in node.Children)
            {
                if (child.Type == "membership")
                {
                    var m = FormMembership.FromNode(child);
                    child.ThrowUnknownAttributes();
                    yield return m;
                }
                else throw new BenefitsException("Unknown node " + child.Err);
            }
        }
    }
}