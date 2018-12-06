using BetterAfrica.Shared;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipPackage
    {
        public string Name { get; set; }
        public string Covers { get; set; }
        public string AgentCode { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? SignDate { get; set; }
        public virtual ICollection<IDetailProduct> Products { get; } = new HashSet<IDetailProduct>();

        public static FormMembershipPackage ReadDetail(CNode node)
        {
            var p = new FormMembershipPackage
            {
                Name = node.TryGetString("name"),
                Covers = node.TryGetString("covers"),
                AgentCode = node.GetString("agent"),
                InceptionDate = node.TryGetDateTime("inceptionDate"),
                SignDate = node.TryGetDateTime("signDate"),
            };

            foreach (var child in node.Children)
            {
                switch (child.Type.ToLower())
                {
                    case "funeral": p.Products.Add(DetailProductFuneral.FromNode(child)); break;
                    case "medical": p.Products.Add(DetailProductMedical.ReadDetail(child)); break;
                    case "hamper": p.Products.Add(DetailProductHamper.ReadDetail(child)); break;
                    case "education": p.Products.Add(DetailProductEducation.FromNode(child)); break;
                    case "loyalty": p.Products.Add(DetailProductLoyalty.FromNode(child)); break;
                    default:
                        throw new BenefitsException("Unknown node " + child.Err);
                }
                child.ThrowUnknownAttributes();
            }

            return p;
        }
    }
}