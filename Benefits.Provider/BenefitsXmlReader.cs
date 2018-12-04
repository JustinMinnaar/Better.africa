using Benefits.Entities;
using Benefits.Provider.Forms;
using Benefits.Shared;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Provider
{
    public static class BenefitsXmlReader
    {
        public static IEnumerable<FormMembership> ReadForms(string xmlPath)
        {
            var node = CNode.FromXmlFile(xmlPath);
            return ReadForms(node);
        }

        public static IEnumerable<FormMembership> ReadForms(this CNode node)
        {
            if (node.Type != "memberships")
                throw new BenefitsException("Unknown node " + node.Err);

            foreach (var child in node.Children)
            {
                if (node.Type == "membership")
                {
                    var m = ReadFormMembership(node);
                    node.ThrowUnknownAttributes();
                    yield return m;
                }
                else throw new BenefitsException("Unknown node " + node.Err);
            }
        }

        private static FormMembership ReadFormMembership(this CNode node)
        {
            var m = new FormMembership
            {
                Detail = node.ReadDetailMembership()
            };

            foreach (var child in node.Children)
            {
                switch (child.Type.ToLower())
                {
                    case "principal": m.Principal = ReadPerson(child); break;
                    case "spouse": m.Spouse = ReadPerson(child); break;
                    case "child": m.Children.Add(ReadPerson(child)); break;
                    case "family": m.Family.Add(ReadPerson(child)); break;
                    case "beneficiary": m.Beneficiaries.Add(ReadPersonBeneficiary(child)); break;
                    case "package": m.Packages.Add(ReadPackage(child)); break;
                    default:
                        throw new BenefitsException("Unknown node " + child.Err);
                }
                child.ThrowUnknownAttributes();
            }

            return m;
        }

        private static FormMembershipDetail ReadDetailMembership(this CNode node)
        {
            var d = new FormMembershipDetail
            {
                Action = node.TryGetEnum<EFormAction>("action"),
                AgentCode = node.GetString("agent"),
                InceptionDate = node.TryGetDateTime("inceptionDate"),
                SignDate = node.TryGetDateTime("signDate"),
                BeneficiaryType = node.TryGetEnum<EBeneficiaryType>("beneficiaryType"),
                Number = node.GetString("number"),
            };

            return d;
        }

        private static FormMembershipPerson ReadPerson(this CNode node)
        {
            var p = new FormMembershipPerson();

            p.FirstName = node.TryGetString("firstName");
            p.LastName = node.TryGetString("lastName");
            p.IdentityNumber = node.TryGetString("identity");
            p.Gender = node.TryGetEnum<BPersonGenders>("gender");
            p.DateOfBirth = node.TryGetDateTime("birthDate");
            p.DateOfDeath = node.TryGetDateTime("deathDate");
            p.CellPhone = node.GetString("cellPhone");
            p.HomePhone = node.GetString("homePhone");
            p.WorkPhone = node.GetString("workPhone");
            p.WorkName = node.GetString("workName");
            p.Email = node.GetString("email");
            p.Scholar = node.TryGetBoolean("scholar");
            p.SchoolName = node.TryGetString("school");

            return p;
        }

        private static DetailBeneficiary ReadPersonBeneficiary(CNode child)
        {
            var b = new DetailBeneficiary
            {
                Ratio = child.TryGetString("ratio"),
                Name = child.TryGetString("name"),
                Phone = child.TryGetString("phone"),
                Email = child.TryGetString("email"),
                Identity = child.TryGetString("identity"),
            };

            return b;
        }

        private static FormMembershipPackage ReadPackage(CNode node)
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
                    case "funeral": p.Products.Add(ReadProductFuneral(child)); break;
                    case "medical": p.Products.Add(ReadProductMedical(child)); break;
                    case "hamper": p.Products.Add(ReadProductHamper(child)); break;
                    case "education": p.Products.Add(ReadProductEducation(child)); break;
                    case "loyalty": p.Products.Add(ReadProductLoyalty(child)); break;
                    default:
                        throw new BenefitsException("Unknown node " + child.Err);
                }
                child.ThrowUnknownAttributes();
            }

            return p;
        }

        private static IDetailProduct ReadProductFuneral(CNode child)
        {
            return new DetailProductFuneral { Cover = child.TryGetDecimal("cover"), };
        }

        private static IDetailProduct ReadProductMedical(CNode child)
        {
            return new DetailProductMedical
            {
                HasTransport = child.TryGetBoolean("hasTransport"),
                HasEmergency = child.TryGetBoolean("hasEmergency"),
                DailyCover = child.TryGetDecimal("dailyCover"),
            };
        }

        private static IDetailProduct ReadProductHamper(CNode child)
        {
            return new DetailProductHamper
            {
                Savings = child.TryGetBoolean("savings"),
            };
        }

        private static IDetailProduct ReadProductEducation(CNode child)
        {
            return new DetailProductEducation
            {
                Savings = child.TryGetBoolean("savings"),
            };
        }

        private static IDetailProduct ReadProductLoyalty(CNode child)
        {
            return new DetailProductLoyalty
            {
                Savings = child.TryGetBoolean("savings"),
            };
        }
    }
}