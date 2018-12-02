using Benefits.Entities;
using System;

namespace Benefits.Entities.UnitTests
{
    public abstract class BaseValidationUnitTests
    {
        // NOTE: It is necessary for testing duplications to always create a new person

        protected Person p1Adam49 => new Person
        {
            NameFirst = "Adam",
            NameLast = "Adams",
            DateOfBirth = new DateTime(1969, 07, 31),
            Gender = PersonGenders.Male,
            Identity = "690731",
        };

        protected Person p2Bertha47 => new Person
        {
            NameFirst = "Bertha",
            NameLast = "Adams",
            DateOfBirth = new DateTime(1971, 11, 12),
            Gender = PersonGenders.Female,
            Identity = "711112",
        };

        protected Person p3Charles11 => new Person
        {
            NameFirst = "Charles",
            NameLast = "Adams",
            DateOfBirth = new DateTime(2007, 3, 7),
            Gender = PersonGenders.Male,
            Identity = "070307",
        };

        protected Person p4Debbie1 => new Person
        {
            NameFirst = "Debbie",
            NameLast = "Adams",
            DateOfBirth = new DateTime(2017, 11, 17),
            Gender = PersonGenders.Female,
            Identity = "171117",
        };

        protected Person p5Eddie27 = new Person
        {
            NameFirst = "Eddie",
            NameLast = "Adams",
            DateOfBirth = new DateTime(1991, 3, 4),
            Gender = PersonGenders.Male,
            Identity = "910304",
        };
    }
}