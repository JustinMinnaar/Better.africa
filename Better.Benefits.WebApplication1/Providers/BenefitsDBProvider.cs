using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Providers
{
    public class BenefitsDBProvider : IDisposable
    {
        private BenefitsDb db = new BenefitsDb();

        public BenefitsDBProvider()
        {
            db.Database.Connection.Open();
        }

        private IQueryable<Person> QueryPeople(bool isAgent = false, bool isClient = false, bool isStaff = false)
        {
            var q = from person in db.People
                    where person.Row.RowCurrent && !person.Row.RowDeleted && !person.Row.RowDeletedParent
                    select person;
            if (isAgent) q = q.Where(person => person.IsAgent);
            if (isClient) q = q.Where(person => person.IsClient);
            if (isStaff) q = q.Where(person => person.IsStaff);
            return q;
        }

        public async Task<List<Person>> GetClientsAsync()
        {
            return await QueryPeople(isClient: true).OrderBy(person => person.IdNumber).ToListAsync();
        }

        public async Task<Person> GetClientAsync(Guid id)
        {
            return await QueryPeople(isClient: true).FirstOrDefaultAsync(person => person.Id == id);
        }

        public async Task<List<Person>> GetAgentsAsync()
        {
            return await QueryPeople(isAgent: true).OrderBy(person => person.IdNumber).ToListAsync();
        }

        public async Task<Person> GetAgentAsync(Guid id)
        {
            return await QueryPeople(isAgent: true).FirstOrDefaultAsync(person => person.Id == id);
        }

        public async Task SavePersonAsync(Person person)
        {
            // Mark the old record
            person.Row.RowCurrent = false;
            person.Row.RowDateEnd = DateTime.Now;
            await db.SaveChangesAsync();

            // Create a new record
            person.Row.RowCurrent = true;
            person.Row.RowDateBegin = DateTime.Now;
            person.Row.RowDateEnd = null;
            person.Row.RowVersion++;
            db.Entry(person).State = EntityState.Added;
            await db.SaveChangesAsync();
        }

        public async Task NewClientAsync(Person client)
        {
            client.Row.RowCurrent = true;
            client.Row.RowDateBegin = DateTime.Now;
            client.Row.RowVersion = 1;
            db.People.Add(client);
            await db.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(Guid id)
        {
            var client = await GetClientAsync(id);
            client.Row.RowDeleted = true;
            await SavePersonAsync(client);
        }

        public void Dispose()
        {
            if (db != null) { db.Dispose(); db = null; }
        }
    }
}