using Knights.Core.Nodes;
using System;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipDetail
    {
        public string Err => $"number={Number} action={Action}";

        public EFormAction? Action { get; set; }
        public string AgentCode { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? InceptionDate { get; set; }
        public EBeneficiaryType? BeneficiaryType { get; set; }
        public string Number { get; set; }

        public static FormMembershipDetail ReadDetail(CNode node)
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
    }
}