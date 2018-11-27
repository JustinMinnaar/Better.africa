using System;
using Better.Benefits.Data;

namespace Better.Benefits.Provider
{
    public class PolicyProvider
    {
        private readonly BetterDb db;
        private readonly Guid userId;
        private readonly DateTime when;

        public PolicyProvider(BetterDb db, Guid userId)
        {
            this.db = db;
            this.userId = userId;
            this.when = Clock.Now;
        }

        /*
        /// <summary>Creates or updates the Owner, Policy and Member records.</summary>
        /// <param name="mPolicy">The model filled in by the UI.</param>
        public async Task SavePolicy(PolicyModel mPolicy)
        {
            // Validate that the current user has permission to edit policies

            // Create the owner record if missing
            var owner = TryLoadActivePerson(mPolicy.OwnerModel.PersonId);
            if (owner == null)
                owner = InsertPersonRecord(mPolicy.OwnerModel);
            else
                UpdatePersonRecord(owner, mPolicy.OwnerModel);

            // Create a policy record in the database
            var policy = new Policy();
            CopyRowProperties(policy.Row, mPolicy);
            CopyPolicyProperties(policy, mPolicy);
            db.Policy.Add(policy);

            // Create the related member details in the database
            foreach (var memberModel in mPolicy.Members)
            {
                var person = TryLoadActivePerson(memberModel.PersonId);

                var person = new Person();
                CopyRowProperties(person.Row, memberModel);
                CopyPersonProperties(person, memberModel);
                var member = new Policy_Member
                {
                    Id = memberModel.Id,
                    Policy = policy,
                    Person = person
                }
                }
        }

         */
    }
}