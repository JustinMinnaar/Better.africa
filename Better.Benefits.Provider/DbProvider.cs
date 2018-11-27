namespace Better.Benefits.Provider
{
    using Better.Benefits.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class DbProvider : IDisposable
    {
        private BetterDb db = new BetterDb();
        private Guid userId;

        public DbProvider(Guid userId)
        {
            db.Database.Connection.Open();
            this.userId = userId;
        }

        public PersonProvider Person { get => new PersonProvider(db, userId); }

        public PolicyProvider Policy { get => new PolicyProvider(db, userId); }

        private void CopyPolicyProperties(Policy policy, PolicyModel model)
        {
            policy.Id = model.Row.Id;
            policy.PolicyId = model.PolicyId;
            policy.Code = model.Code;
            policy.OwnerPersonId = model.OwnerPersonId;
            policy.StatusNo = (byte)model.Status;
            policy.TypeNo = (byte)model.Type;
        }

        private void CopyRowProperties(Row row, RowModel model)
        {
            row.Archived = model.Archived;
            row.Deleted = model.Deleted;
            row.DeletedParent = model.DeletedParent;
            row.IsValid = model.IsValid;
            row.ModifiedBy = model.ModifiedBy;
            row.ModifiedOn = model.ModifiedOn;
            row.Version = model.Version;
        }

        private IQueryable<Policy> QueryPolicy(Policy_Type type = null)
        {
            if (type == null) return db.Policy;

            var q = from p in db.Policy
                    where p.Type == type && !p.Row.Archived
                    select p;
            return q;
        }

        /// <summary>Returns a list of Policy, that are not archived and of a specified type.</summary>
        /// <param name="type">A policy type, or null for any.</param>
        /// <returns>A list of Policy</returns>
        public async Task<List<Policy>> ListPolicyAsync(Policy_Type type = null)
        {
            return await QueryPolicy(type).OrderBy(p => p.Code).ToListAsync();
        }

        /// <summary>Returns a Policy, that is not archived and identified by id.</summary>
        /// <param name="policyId">The unique identifier for the policy.</param>
        /// <returns>A single Policy record.</returns>
        public async Task<Policy> GetPolicyAsync(Guid policyId)
        {
            return await QueryPolicy().FirstOrDefaultAsync(p => p.PolicyId == policyId);
        }

        //private IQueryable<Person> QueryPeople(bool isAgent = false, bool isClient = false, bool isStaff = false)
        //{
        //    var q = from person in db.People
        //            where person.Person_Details.Row && !person.Row.RowDeleted && !person.Row.RowDeletedParent
        //            select person;
        //    if (isAgent) q = q.Where(person => person.IsAgent);
        //    if (isClient) q = q.Where(person => person.IsClient);
        //    if (isStaff) q = q.Where(person => person.IsStaff);
        //    return q;
        //}

        //public async Task<List<Person>> GetClientsAsync()
        //{
        //    return await QueryPeople(isClient: true).OrderBy(person => person.IdNumber).ToListAsync();
        //}

        //public async Task<Person> GetClientAsync(Guid id)
        //{
        //    return await QueryPeople(isClient: true).FirstOrDefaultAsync(person => person.Id == id);
        //}

        //public async Task<List<Person>> GetAgentsAsync()
        //{
        //    return await QueryPeople(isAgent: true).OrderBy(person => person.IdNumber).ToListAsync();
        //}

        //public async Task<Person> GetAgentAsync(Guid id)
        //{
        //    return await QueryPeople(isAgent: true).FirstOrDefaultAsync(person => person.Id == id);
        //}

        //public async Task SavePersonAsync(Person person)
        //{
        //    // Mark the old record
        //    person.Row.RowCurrent = false;
        //    person.Row.RowDateEnd = DateTime.Now;
        //    await db.SaveChangesAsync();

        //    // Create a new record
        //    person.Row.RowCurrent = true;
        //    person.Row.RowDateBegin = DateTime.Now;
        //    person.Row.RowDateEnd = null;
        //    person.Row.RowVersion++;
        //    db.Entry(person).State = EntityState.Added;
        //    await db.SaveChangesAsync();
        //}

        //public async Task NewClientAsync(Person client)
        //{
        //    client.Row.RowCurrent = true;
        //    client.Row.RowDateBegin = DateTime.Now;
        //    client.Row.RowVersion = 1;
        //    db.People.Add(client);
        //    await db.SaveChangesAsync();
        //}

        //public async Task DeleteClientAsync(Guid id)
        //{
        //    var client = await GetClientAsync(id);
        //    client.Row.RowDeleted = true;
        //    await SavePersonAsync(client);
        //}

        public void Dispose()
        {
            if (db != null) { db.Dispose(); db = null; }
        }
    }
}