using System;

namespace Benefits.Provider.Forms
{
    public class FormMembershipDetail
    {
        public string Err => $"number={Number} action={Action}";

        public EFormAction? Action { get; set; }
        public string AgentCode { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? InceptionDate { get; set; }
        public EBeneficiaryType? BeneficiaryType { get; set; }
        public string Number { get; internal set; }
    }
}