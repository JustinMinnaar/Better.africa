using BetterAfrica.Shared;
using Knights.Core.Nodes;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities
{
    public static class BMembers
    {
        public static CNode ToNode(IEnumerable<CMember> members)
        {
            var node = new CNode("members");
            foreach (var mem in members)
            {
                node.AddChild(mem.ToNode());
            }
            return node;
        }

        public static IEnumerable<CMember> FromXmlFile(string xmlPath)
        {
            var node = CNode.FromXmlFile(xmlPath);
            return ReadMembers(node);
        }

        public static IEnumerable<CMember> FromXml(string xml)
        {
            var node = CNode.FromXml(xml);
            return ReadMembers(node);
        }

        public static IEnumerable<CMember> ReadMembers(CNode node)
        {
            if (node.Type != "members")
                throw new BenefitsException("Unknown node " + node.Err);

            foreach (var child in node.Children)
            {
                if (child.Type == "member")
                {
                    var m = BaseExtensions<CMember>.FromNode(child);
                    child.ThrowUnknownAttributes();
                    yield return m;
                }
                else throw new BenefitsException("Unknown node " + child.Err);
            }
        }
    }
}