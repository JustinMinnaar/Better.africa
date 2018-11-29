using Benefits.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Benefits.Entities
{
    public class Member : CEntity
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public ICollection<Person> People { get; } = new HashSet<Person>();

        public Person Principal => People.FirstOrDefault(p => p.Type == Person.Types.Principal);
        public Person Spouse => People.FirstOrDefault(p => p.Type == Person.Types.Spouse);
        public IList<Person> Children => People.Where(p => p.Type == Person.Types.Child).ToList();
        public IList<Person> Extended => People.Where(p => p.Type == Person.Types.Family).ToList();

        #region Helper Methods

        public Member WithPrincipal(Person principal)
        {
            principal.Type = Person.Types.Principal;
            People.Add(principal);
            return this;
        }

        public Member WithSpouse(Person spouse)
        {
            spouse.Type = Person.Types.Spouse;
            People.Add(spouse);
            return this;
        }

        public Member WithChildren(params Person[] children)
        {
            foreach (var child in children)
            {
                child.Type = Person.Types.Child;
                People.Add(child);
            }
            return this;
        }

        public Member WithFamily(params Person[] family)
        {
            foreach (var child in family)
            {
                child.Type = Person.Types.Family;
                People.Add(child);
            }
            return this;
        }

        #endregion Helper Methods

        //public void Validate(Dictionary<string, string> errors)
        //{
        //    var principalValid = People.Count(p => p.Type == Person.Types.Principal) == 1;
        //    if (!principalValid) errors.Add(nameof(Principal), "There must be one principal.");

        //    var years = 0;
        //    if (Principal.DateOfBirth == )
        //    var principalAgeInYears = Principal?.DateOfBirth != null
        //        ? (Clock.Now - Principal.DateOfBirth.Value).Days / 365.25f
        //        : null;
        //}
    }
}