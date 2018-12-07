using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("package")]
    public class FormMembershipPackage : BaseForm<FormMembershipPackage>, IToNode
    {
        public string Name { get; set; }
        public string Covers { get; set; }
        public string AgentCode { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? SignDate { get; set; }
        public virtual ICollection<IToNode> Products { get; } = new HashSet<IToNode>();

        public override void Import(CNode node)
        {
            base.Import(node);

            foreach (var child in node.Children)
            {
                switch (child.Type.ToLower())
                {
                    case "funeral": Products.Add(DetailProductFuneral.FromNode(child)); break;
                    case "medical": Products.Add(DetailProductMedical.FromNode(child)); break;
                    case "hamper": Products.Add(DetailProductHamper.FromNode(child)); break;
                    case "education": Products.Add(DetailProductEducation.FromNode(child)); break;
                    case "loyalty": Products.Add(DetailProductLoyalty.FromNode(child)); break;
                    default:
                        throw new BenefitsException("Unknown node " + child.Err);
                }
                child.ThrowUnknownAttributes();
            }
        }

        public override void Export(CNode node)
        {
            base.Export(node);
            foreach (var product in Products)
            {
                node.AddChild(product.ToNode());
            }
        }
    }
}