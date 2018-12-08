using BetterAfrica.Shared;
using Knights.Core.Nodes;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public static class BMembers
    {
        public static CNode ToNode(IEnumerable<BMember> memberships)
        {
            var node = new CNode("memberships");
            foreach (var mem in memberships)
            {
                node.AddChild(mem.ToNode());
            }
            return node;
        }

        public static IEnumerable<BMember> FromXmlFile(string xmlPath)
        {
            var node = CNode.FromXmlFile(xmlPath);
            return ReadMembers(node);
        }

        public static IEnumerable<BMember> FromXml(string xml)
        {
            var node = CNode.FromXml(xml);
            return ReadMembers(node);
        }

        public static IEnumerable<BMember> ReadMembers(CNode node)
        {
            if (node.Type != "memberships")
                throw new BenefitsException("Unknown node " + node.Err);

            foreach (var child in node.Children)
            {
                if (child.Type == "membership")
                {
                    var m = BaseExtensions<BMember>.FromNode(child);
                    child.ThrowUnknownAttributes();
                    yield return m;
                }
                else throw new BenefitsException("Unknown node " + child.Err);
            }
        }
    }
}