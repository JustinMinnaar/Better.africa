using System.Collections.Generic;

namespace Benefits.Provider.Forms
{
    public class FormMembership
    {
        public Form Form { get; set; }
        public DetailPerson Principal { get; set; }
        public DetailArea PrincipalArea { get; set; }
        public DetailCommunication PrincipalCommunication { get; set; }
        public DetailPerson Spouse { get; set; }
        public IEnumerable<DetailPerson> Children { get; set; }
        public IEnumerable<DetailPerson> Family { get; set; }
        public IEnumerable<DetailPerson> Beneficiaries { get; set; }
    }
}