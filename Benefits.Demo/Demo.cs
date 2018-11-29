using Benefits.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benefits.Demo
{
    public class Demo
    {
        public Demo()
        {
            var p1 = new Person
            {
                NameFirst = "Justin",
                NameLast = "Minnaar",
                DateOfBirth = new DateTime(1969, 07, 31),
                MembershipType = PersonMembershipTypes.Principal,
            };

            var p2 = new Person
            {
                NameFirst = "Karen",
                NameLast = "Campbell",
                DateOfBirth = new DateTime(1971, 11, 12),
                MembershipType = PersonMembershipTypes.Spouse,
            };

            var m1 = new Member().WithPrincipal(p1).WithSpouse(p2);

            if (m1.Errors.Count > 0)
            {
                foreach (var err in m1.Errors)
                {
                    Console.WriteLine(err.Key + ": " + err.Value);
                }
            }
            else using (var db = new BenefitsDbContext())
                {
                    db.Members.Add(m1);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var ve = db.GetValidationErrors();
                    }
                }
        }
    }
}