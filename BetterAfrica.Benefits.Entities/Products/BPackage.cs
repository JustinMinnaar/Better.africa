using BetterAfrica.Benefits.Entities.Forms;
using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;

namespace BetterAfrica.Benefits.Entities
{
    /// <summary>
    /// We define standard packages shared by many members, and custom packages when needed for specific members.
    /// </summary>
    [Nickname("package")]
    public class BPackage : BaseEntity
    {
        public string Name { get; set; }

        public long? MemberId { get; set; }
        public virtual BMember Member { get; set; }

        public BPlanPolicyFuneral Funeral { get; set; }
        public BPlanPolicyMedical Medical { get; set; }
        public BPlanEducation Education { get; set; }
        public BPlanProductHamper Hamper { get; set; }
        public BPlanProductLoyalty Loyalty { get; set; }

        protected override void ImportChild(CNode child, CCreator creator = null)
        {
            switch (child.Type.ToLower())
            {
                case "funeral": Funeral = child.To<BPlanPolicyFuneral>(); break;
                case "medical": Medical = child.To<BPlanPolicyMedical>(); break;
                case "hamper": Hamper = child.To<BPlanProductHamper>(); break;
                case "education": Education = child.To<BPlanEducation>(); break;
                case "loyalty": Loyalty = child.To<BPlanProductLoyalty>(); break;
                default:
                    throw new BenefitsException("Unknown node " + child.Err);
            }
            child.ThrowUnknownAttributes();
        }

        protected override void ExportChildren(CNode node, CCreator creator = null)
        {
            node.AddChild(Funeral?.ToNode("funeral"));
            node.AddChild(Medical?.ToNode("medical"));
            node.AddChild(Hamper?.ToNode("hamper"));
            node.AddChild(Education?.ToNode("education"));
            node.AddChild(Loyalty?.ToNode("loyalty"));
        }
    }
}