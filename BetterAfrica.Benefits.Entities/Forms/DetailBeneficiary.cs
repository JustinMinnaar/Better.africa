using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BetterAfrica.Benefits.Entities.Forms
{
    [Nickname("beneficiary")]
    public class FormMembershipBeneficiary
    {
        public string Email { get; set; }
        public string Identity { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Ratio { get; set; }

        public CNode Export()
        {
            var node = new CNode(this.ToNickname());
            node.SetString("name", Name);
            node.SetString("ratio", Ratio);
            node.SetString("phone", Phone);
            node.SetString("email", Email);
            node.SetString("identity", Identity);
            return node;
        }

        public static FormMembershipBeneficiary FromNode(CNode child)
        {
            var b = new FormMembershipBeneficiary
            {
                Ratio = child.TryGetString("ratio"),
                Name = child.TryGetString("name"),
                Phone = child.TryGetString("phone"),
                Email = child.TryGetString("email"),
                Identity = child.TryGetString("identity"),
            };

            return b;
        }
    }
}