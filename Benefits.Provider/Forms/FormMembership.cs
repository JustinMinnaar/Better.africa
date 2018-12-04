using System.Collections.Generic;

namespace Benefits.Provider.Forms
{
    public class FormMembership
    {
        public string Err => Detail.Err + " " + Principal.Err;

        public FormMembershipDetail Detail { get; set; }
        public FormMembershipArea Area { get; set; }
        public FormMembershipCommunication Communication { get; set; }
        public FormMembershipPerson Principal { get; set; }
        public FormMembershipPerson Spouse { get; set; }
        public List<FormMembershipPerson> Children { get; } = new List<FormMembershipPerson>();
        public List<FormMembershipPerson> Family { get; } = new List<FormMembershipPerson>();
        public List<DetailBeneficiary> Beneficiaries { get; } = new List<DetailBeneficiary>();
        public List<FormMembershipPackage> Packages { get; } = new List<FormMembershipPackage>();
    }
}