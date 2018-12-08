using BetterAfrica.Benefits.Entities.Forms;
using BetterAfrica.Benefits.Entities.Member;
using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BetterAfrica.Benefits.Entities
{
    public class BMember : BContract
    {
        #region Properties

        public string Address { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; } = "Gauteng";
        public string AddressCountry { get; set; } = "South Africa";
        public string AddressCode { get; set; }
        public ECommunicateVia? CommunicateVia { get; set; }
        public bool? CommunicateSms { get; set; }
        public ELanguage? CommunicateLanguage { get; set; }
        public EBeneficiariesType? BeneficiariesType { get; set; }

        /// <summary>A member may have any number of dependencies. This includes the Principal, Spouse, Children and extended Family.</summary>
        public virtual ICollection<BMemberDependency> Dependencies { get; } = new HashSet<BMemberDependency>();

        /// <summary>A member may have any number of beneficiaries.</summary>
        public virtual ICollection<BMemberBeneficiary> Beneficiaries { get; } = new HashSet<BMemberBeneficiary>();

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

            foreach (var membershipPackage in Accounts)
            {
                membershipPackage.BeforeSave(errors);
            }
        }

        public string PrincipalError
        {
            get
            {
                var principalsCount = Dependencies.Count(p => p.Type == BDependencyType.Principal);
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

                var spouseValid = Dependencies.Count(p => p.Type == BDependencyType.Spouse) == 1;
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
        public BPerson Principal => GetPeople(BDependencyType.Principal).FirstOrDefault();

        [NotMapped]
        public BPerson Spouse => GetPeople(BDependencyType.Spouse).FirstOrDefault();

        [NotMapped]
        public IList<BPerson> Children => GetPeople(BDependencyType.Child).ToList();

        [NotMapped]
        public IList<BPerson> Family => GetPeople(BDependencyType.Family).ToList();

        private IEnumerable<BPerson> GetPeople(BDependencyType type)
        {
            return from d in Dependencies where d.Type == type select d.Person;
        }

        #endregion Helper Properties

        #region Helper Methods

        public BMember WithPrincipal(BPerson principal)
        {
            Dependencies.Add(new BMemberDependency
            {
                Member = this,
                Person = principal,
                Type = BDependencyType.Principal
            });
            return this;
        }

        public BMember WithSpouse(BPerson spouse)
        {
            Dependencies.Add(new BMemberDependency
            {
                Member = this,
                Person = spouse,
                Type = BDependencyType.Spouse
            });
            return this;
        }

        public BMember WithChildren(params BPerson[] children)
        {
            foreach (var child in children)
            {
                Dependencies.Add(new BMemberDependency
                {
                    Member = this,
                    Person = child,
                    Type = BDependencyType.Child
                });
            }
            return this;
        }

        public BMember WithFamily(params BPerson[] persons)
        {
            foreach (var person in persons)
            {
                Dependencies.Add(new BMemberDependency
                {
                    Member = this,
                    Person = person,
                    Type = BDependencyType.Family
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
                    case "principal": Dependencies.Add(new BMemberDependency { Member = this, Person = childNode.To<BPerson>(), Type = BDependencyType.Principal }); break;
                    case "spouse": Dependencies.Add(new BMemberDependency { Member = this, Person = childNode.To<BPerson>(), Type = BDependencyType.Spouse }); break;
                    case "child": Dependencies.Add(new BMemberDependency { Member = this, Person = childNode.To<BPerson>(), Type = BDependencyType.Child }); break;
                    case "family": Dependencies.Add(new BMemberDependency { Member = this, Person = childNode.To<BPerson>(), Type = BDependencyType.Family }); break;
                    case "beneficiary": Beneficiaries.Add(childNode.To<BMemberBeneficiary>()); break;
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