using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BetterAfrica.Benefits.Entities
{
    public class CMember : BContract
    {
        #region Properties

        public string Address { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; } = "Gauteng";
        public string AddressCountry { get; set; } = "South Africa";
        public string AddressCode { get; set; }
        public ECommunicateVia? CommunicateVia { get; set; }
        public ECommunicateSms? CommunicateSms { get; set; }
        public ELanguage? CommunicateLanguage { get; set; }
        public EBeneficiariesType? BeneficiariesType { get; set; }

        /// <summary>A member may have any number of dependencies. This includes the Principal, Spouse, Children and extended Family.</summary>
        public virtual ICollection<CMemberDependency> Dependencies { get; } = new HashSet<CMemberDependency>();

        /// <summary>A member may have any number of beneficiaries.</summary>
        public virtual ICollection<CMemberBeneficiary> Beneficiaries { get; } = new HashSet<CMemberBeneficiary>();

        /// <summary>A member may have any number of accounts, each linked to a plan. This supports policies
        /// likes medical, funeral and supports products like hamper, education and loyalty.</summary>
        public virtual ICollection<BMemberAccount> Accounts { get; } = new HashSet<BMemberAccount>();

        #endregion Properties

        #region BeforeSave

        protected override void BeforeSaveOverride(EntityErrors errors)
        {
            base.BeforeSaveOverride(errors);

            foreach (var dependency in Dependencies)
            {
                dependency.BeforeSave(errors);
            }

            foreach (var beneficiary in Beneficiaries)
            {
                beneficiary.BeforeSave(errors);
            }

            foreach (var memberPackage in Accounts)
            {
                memberPackage.BeforeSave(errors);
            }
        }

        public string PrincipalError
        {
            get
            {
                var principalsCount = Dependencies.Count(p => p.Type == EDependencyType.Principal);
                if (principalsCount != 1) return "There must be one principal.";

                if (InceptionDate != null)
                {
                    var principalYears = Principal.AgeInYearsAsAt(InceptionDate.Value);
                    if (principalYears < 18 || principalYears > 65)
                        return "Principal must be between 18 and 65 years old on inception date.";
                }

                return null;
            }
        }

        public string SpouseError
        {
            get
            {
                var spouse = Spouse;
                if (spouse == null) return null;

                var spouseValid = Dependencies.Count(p => p.Type == EDependencyType.Spouse) == 1;
                if (!spouseValid) return "There may not be more than one spouse.";

                if (InceptionDate != null)
                {
                    var spouseYears = Spouse.AgeInYearsAsAt(InceptionDate.Value);
                    if (spouseYears < 18 || spouseYears > 65)
                        return "Spouse must be between 18 and 65 years old on inception date.";
                }

                return null;
            }
        }

        #endregion BeforeSave

        #region Helper Properties

        [NotMapped]
        public CPerson Principal => GetPeople(EDependencyType.Principal).FirstOrDefault();

        [NotMapped]
        public CPerson Spouse => GetPeople(EDependencyType.Spouse).FirstOrDefault();

        [NotMapped]
        public IList<CPerson> Children => GetPeople(EDependencyType.Child).ToList();

        [NotMapped]
        public IList<CPerson> Family => GetPeople(EDependencyType.Family).ToList();

        private IEnumerable<CPerson> GetPeople(EDependencyType type)
        {
            return from d in Dependencies where d.Type == type select d.Person;
        }

        #endregion Helper Properties

        #region Helper Methods

        public CMember WithPrincipal(CPerson principal)
        {
            Dependencies.Add(new CMemberDependency
            {
                Member = this,
                Person = principal,
                Type = EDependencyType.Principal
            });
            return this;
        }

        public CMember WithSpouse(CPerson spouse)
        {
            Dependencies.Add(new CMemberDependency
            {
                Member = this,
                Person = spouse,
                Type = EDependencyType.Spouse
            });
            return this;
        }

        public CMember WithChildren(params CPerson[] children)
        {
            foreach (var child in children)
            {
                Dependencies.Add(new CMemberDependency
                {
                    Member = this,
                    Person = child,
                    Type = EDependencyType.Child
                });
            }
            return this;
        }

        public CMember WithFamily(params CPerson[] persons)
        {
            foreach (var person in persons)
            {
                Dependencies.Add(new CMemberDependency
                {
                    Member = this,
                    Person = person,
                    Type = EDependencyType.Family
                });
            }
            return this;
        }

        #endregion Helper Methods

        #region Import/Export

        public override void Export(CNode node, CCreator creator = null)
        {
            base.Export(node, creator);

            node.AddChild(Principal?.ToNode("principal"));
            node.AddChild(Spouse?.ToNode("spouse"));
            foreach (var child in Children) { node.AddChild(child.ToNode("child")); }
            foreach (var family in Family) { node.AddChild(family.ToNode("family")); }
            foreach (var beneficiary in Beneficiaries) { node.AddChild(beneficiary.ToNode("beneficiary")); }
            foreach (var product in Accounts) { node.AddChild(product.ToNode("product")); }
        }

        public override void Import(CNode node, CCreator creator = null)
        {
            base.Import(node, creator);

            foreach (var childNode in node.Children)
            {
                switch (childNode.Type.ToLower())
                {
                    case "principal": Dependencies.Add(new CMemberDependency { Member = this, Person = childNode.To<CPerson>(), Type = EDependencyType.Principal }); break;
                    case "spouse": Dependencies.Add(new CMemberDependency { Member = this, Person = childNode.To<CPerson>(), Type = EDependencyType.Spouse }); break;
                    case "child": Dependencies.Add(new CMemberDependency { Member = this, Person = childNode.To<CPerson>(), Type = EDependencyType.Child }); break;
                    case "family": Dependencies.Add(new CMemberDependency { Member = this, Person = childNode.To<CPerson>(), Type = EDependencyType.Family }); break;
                    case "beneficiary": Beneficiaries.Add(childNode.To<CMemberBeneficiary>()); break;
                    case "product": Accounts.Add(childNode.To<BMemberAccount>()); break;
                    default:
                        throw new BenefitsException("Unknown node " + childNode.Err);
                }
                childNode.ThrowUnknownAttributes();
            }
        }

        #endregion Import/Export
    }
}