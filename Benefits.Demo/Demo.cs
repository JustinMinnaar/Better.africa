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
            var p1 = new Person { NameFirst = "Justin", NameLast = "Minnaar", DateOfBirth = new DateTime(1969, 07, 31), };
            var p2 = new Person { NameFirst = "Karen", NameLast = "Campbell", DateOfBirth = new DateTime(1971, 11, 12), };
            var m1 = new Member().WithPrincipal(p1).WithSpouse(p2);

            using (var db = new BenefitsDbContext())
            {
                db.Members.Add(m1);
                db.SaveChanges();
            }
        }
    }
}