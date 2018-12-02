using Benefits.Entities;
using System;

namespace Benefits.Entities.UnitTests
{
    public abstract class BaseValidationUnitTests
    {
        // NOTE: It is necessary for testing duplications to always create a new person

        protected BPerson p1Adam49 => new BPerson
        {
            NameFirst = "Adam",
            NameLast = "Adams",
            DateOfBirth = new DateTime(1969, 07, 31),
            Gender = BPersonGenders.Male,
            Identity = "690731",
        };

        protected BPerson p2Bertha47 => new BPerson
        {
            NameFirst = "Bertha",
            NameLast = "Adams",
            DateOfBirth = new DateTime(1971, 11, 12),
            Gender = BPersonGenders.Female,
            Identity = "711112",
        };

        protected BPerson p3Charles11 => new BPerson
        {
            NameFirst = "Charles",
            NameLast = "Adams",
            DateOfBirth = new DateTime(2007, 3, 7),
            Gender = BPersonGenders.Male,
            Identity = "070307",
        };

        protected BPerson p4Debbie1 => new BPerson
        {
            NameFirst = "Debbie",
            NameLast = "Adams",
            DateOfBirth = new DateTime(2017, 11, 17),
            Gender = BPersonGenders.Female,
            Identity = "171117",
        };

        protected BPerson p5Eddie27 = new BPerson
        {
            NameFirst = "Eddie",
            NameLast = "Adams",
            DateOfBirth = new DateTime(1991, 3, 4),
            Gender = BPersonGenders.Male,
            Identity = "910304",
        };
    }
}