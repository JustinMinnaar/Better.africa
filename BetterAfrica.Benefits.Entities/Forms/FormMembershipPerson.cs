using System;
using Benefits.Entities;
using Knights.Core.Common;
using Knights.Core.Nodes;

namespace BetterAfrica.Benefits.Entities.Forms
{
    public class FormMembershipPerson : BaseForm<FormMembershipPerson>
    {
        public string Err => $"name={Name}";

        public string Name => LastName.Comma(FirstName);

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public EPersonGenders? Gender { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string WorkName { get; set; }
        public string EmailAddress { get; set; }
        public bool? Scholar { get; set; }
        public string SchoolName { get; set; }
        public string Email { get; set; }

        public override void Import(CNode node)
        {
            base.Import(node);

            FirstName = node.TryGetString("firstName");
            LastName = node.TryGetString("lastName");
            IdentityNumber = node.TryGetString("identity");
            Gender = node.TryGetEnum<EPersonGenders>("gender");
            DateOfBirth = node.TryGetDateTime("birthDate");
            DateOfDeath = node.TryGetDateTime("deathDate");
            CellPhone = node.GetString("cellPhone");
            HomePhone = node.GetString("homePhone");
            WorkPhone = node.GetString("workPhone");
            WorkName = node.GetString("workName");
            Email = node.GetString("email");
            Scholar = node.TryGetBoolean("scholar");
            SchoolName = node.TryGetString("school");
        }
    }
}