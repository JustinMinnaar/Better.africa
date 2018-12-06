using BetterAfrica.Shared;
using Knights.Core.Nodes;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public static class FormMemberships
    {
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
                    var m = FormMembership.FromNode(child);
                    node.ThrowUnknownAttributes();
                    yield return m;
                }
                else throw new BenefitsException("Unknown node " + node.Err);
            }
        }
    }
}