using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAfrica.Benefits.Providers
{
    public class MembershipProvider
    {
        private Guid userId;

        public MembershipProvider(Guid userId)
        {
            this.userId = userId;
        }

        public void ApplyMembershipForm(FormMembership membership)
        {
            // Create a single transaction to ensure everything saves, or nothing changes.
            using (var db = new BenefitsDbContext())
            {
            }
        }
    }
}