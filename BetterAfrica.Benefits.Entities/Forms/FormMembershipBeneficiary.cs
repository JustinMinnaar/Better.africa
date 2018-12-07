using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("beneficiary")]
    public class FormMembershipBeneficiary : BaseForm<FormMembershipBeneficiary>, IToNode
    {
        public string Email { get; set; }
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Ratio { get; set; }
        public string Code { get; set; }

        public override void Export(CNode node)
        {
            base.Export(node);

            //node.SetString("name", Name);
            //node.SetString("ratio", Ratio);
            //node.SetString("phone", Phone);
            //node.SetString("email", Email);
            //node.SetString("identity", Identity);
        }

        public override void Import(CNode node)
        {
            base.Import(node);

            //Code = node.TryGetString("code");
            //Ratio = node.TryGetString("ratio");
            //Name = node.TryGetString("name");
            //Phone = node.TryGetString("phone");
            //Email = node.TryGetString("email");
            //Identity = node.TryGetString("identity");
        }
    }
}