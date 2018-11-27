using System;
using System.Linq;
using System.Threading.Tasks;
using Better.Benefits.Data;

namespace Better.Benefits.Provider
{
    public sealed class PersonProvider
    {
        private readonly BetterDb db;
        private readonly Guid userId;
        private readonly DateTime when;

        public PersonProvider(BetterDb db, Guid userId)
        {
            this.db = db;
            this.userId = userId;
            this.when = Clock.Now;
        }

        public void SavePerson(PersonModel mPerson)
        {
            var p = new Person
            {
                Id = Guid.NewGuid(),
                Birthdate = mPerson.Birthdate,
                IsAgent = mPerson.IsAgent,
                IsClient = mPerson.IsClient,
                IsStaff = mPerson.IsStaff,
                PersonId = mPerson.PersonId
            };

            p.Row.Archived = false;
            p.Row.ArchivedOn = null;
            p.Row.Deleted = mPerson.Row.Deleted;
            p.Row.DeletedParent = mPerson.Row.DeletedParent;
            p.Row.ModifiedBy = userId;
            p.Row.ModifiedOn = when;

            if (mPerson.Row.Id != Guid.Empty)
            {
                var oldRow = db.Person.Find(mPerson.Row.Id);
                oldRow.Row.Archived = true;
                oldRow.Row.ArchivedOn = when;
                p.Row.Version = oldRow.Row.Version + 1;
            }
            else
            {
                p.Row.Version = 1;
            }

            db.Person.Add(p);

            foreach (var mIdentity in mPerson.Identities)
            {
                SaveIdentity(mPerson, mIdentity);
            }

            // Since the save was successful, we can update the model
            mPerson.Row.Id = p.Id;
            mPerson.Row.Archived = p.Row.Archived;
            mPerson.Row.ArchivedOn = p.Row.ArchivedOn;
            mPerson.Row.ModifiedBy = userId;
            mPerson.Row.ModifiedOn = when;
            mPerson.Row.Version = p.Row.Version;
        }

        public void SaveIdentity(PersonModel mPerson, IdentityModel mIdentity)
        {
            var country = db.Country.Find(mIdentity.CountryCode);
            var identityType = db.Person_IdentityType.Find((byte)mIdentity.IdentityType);

            var i = new Person_Identity
            {
                Id = Guid.NewGuid(),
                Country = country,
                IdentityType = identityType,
                Number = mIdentity.Number,
                PersonId = mPerson.PersonId,
            };
            i.Row.Deleted = mIdentity.Row.Deleted;
            i.Row.DeletedParent = mPerson.Row.Deleted;
            i.Row.ModifiedBy = userId;
            i.Row.ModifiedOn = when;

            if (mIdentity.Row.Id != Guid.Empty)
            {
                var oldRow = db.Person_Identity.Find(mIdentity.Row.Id);
                oldRow.Row.Archived = true;
                oldRow.Row.ArchivedOn = when;
                i.Row.Version = oldRow.Row.Version + 1;
            }
            else
            {
                i.Row.Version = 1;
            }

            db.Person_Identity.Add(i);

            // Since the save was successful, we can update the model
            mIdentity.Row.Id = i.Id;
            mIdentity.Row.Archived = i.Row.Archived;
            mIdentity.Row.ArchivedOn = i.Row.ArchivedOn;
            mIdentity.Row.ModifiedBy = userId;
            mIdentity.Row.ModifiedOn = when;
            mIdentity.Row.Version = i.Row.Version;
        }

        public PersonModel TryLoadPerson(Guid personId)
        {
            var person = db.Person.FirstOrDefault(p => p.PersonId == personId && !p.Row.Archived);
            if (person == null) return null;

            var m = new PersonModel
            {
                Birthdate = person.Birthdate,
                PersonId = person.PersonId,
                IsAgent = person.IsAgent,
                IsClient = person.IsClient,
                IsStaff = person.IsStaff,
                Row = new RowModel(person.Id, person.Row),
            };
            return m;
        }

        private void CopyPersonProperties(Person person, PolicyMemberModel memberModel)
        {
        }
    }
}