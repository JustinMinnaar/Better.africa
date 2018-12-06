﻿using Knights.Core.Nodes;
using System;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipDetail : IImportExport
    {
        public string Err => $"number={Number} action={Action}";

        public EFormAction? Action { get; set; }
        public string AgentCode { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? InceptionDate { get; set; }
        public EBeneficiaryType? BeneficiaryType { get; set; }
        public string Number { get; set; }

        public void Import(CNode node)
        {
            Action = node.TryGetEnum<EFormAction>("action");
            AgentCode = node.TryGetString("agent");
            InceptionDate = node.TryGetDateTime("inceptionDate");
            SignDate = node.TryGetDateTime("signDate");
            BeneficiaryType = node.TryGetEnum<EBeneficiaryType>("beneficiaryType");
            Number = node.TryGetString("number");
        }

        public void Export(CNode node)
        {
            if (Action != null) node.SetEnum<EFormAction>("action", Action.Value);
            node.SetString("agent", AgentCode);
            node.SetDateTime("inceptionDate", InceptionDate);
            node.SetDateTime("signDate", SignDate);
            if (BeneficiaryType != null) node.SetEnum<EBeneficiaryType>("beneficiaryType", BeneficiaryType.Value);
            node.SetString("number", Number);
        }
    }
}