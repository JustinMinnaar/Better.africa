using BetterAfrica.Benefits.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAfrica.Benefits.Providers
{
    public class MembershipProvider
    {
        private long userId;

        public MembershipProvider(long userId)
        {
            this.userId = userId;
        }

        public void CreateMember(CMember member)
        {
            throw new NotImplementedException();
        }

        public CMember TryGetMember(long id)
        {
            throw new NotImplementedException();
        }

        //public BMembership ApplyMembershipForm(FormMembership member)
        //{
        //    // Create a single transaction to ensure everything saves, or nothing changes.
        //    using (var db = new BenefitsDbContext())
        //    {
        //        return null;
        //    }
        //}
    }
}