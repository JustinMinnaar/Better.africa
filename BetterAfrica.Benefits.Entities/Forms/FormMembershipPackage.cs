using Benefits.Shared;
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
        public List<IDetailProduct> Products { get; } = new List<IDetailProduct>();

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
                    case "funeral": p.Products.Add(DetailProductFuneral.ReadDetail(child)); break;
                    case "medical": p.Products.Add(DetailProductMedical.ReadDetail(child)); break;
                    case "hamper": p.Products.Add(DetailProductHamper.ReadDetail(child)); break;
                    case "education": p.Products.Add(DetailProductEducation.ReadDetail(child)); break;
                    case "loyalty": p.Products.Add(DetailProductLoyalty.ReadDetail(child)); break;
                    default:
                        throw new BenefitsException("Unknown node " + child.Err);
                }
                child.ThrowUnknownAttributes();
            }

            return p;
        }
    }
}