using Knights.Fluid.Datums;
using System;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    public abstract class BaseValidationUnitTests
    {
        // NOTE: It is necessary for testing duplications to always create a new person

        protected BPerson p1Adam49 => new BPerson
        {
            FirstName = "Adam",
            LastName = "Adams",
            DateOfBirth = new DateTime(1969, 07, 31),
            IdentityNumber = SouthAfricanIdentityNumber.Random(1960, 1980, true).Number,
            Gender = EPersonGenders.Male,
            CellPhone = "0813702097",
            CellPhoneDial = "0813702097",
            HomePhone = "",
            HomePhoneDial = "",
            WorkPhone = "",
            WorkPhoneDial = "",
            EmailAddress = "",
            Work = "",
        };

        protected BPerson p2Bertha47 => new BPerson
        {
            FirstName = "Bertha",
            LastName = "Adams",
            DateOfBirth = new DateTime(1971, 11, 12),
            Gender = EPersonGenders.Female,
            IdentityNumber = SouthAfricanIdentityNumber.Random(1960, 1980, true).Number
        };

        protected BPerson p3Charles11 => new BPerson
        {
            FirstName = "Charles",
            LastName = "Adams",
            DateOfBirth = new DateTime(2007, 3, 7),
            Gender = EPersonGenders.Male,
            IdentityNumber = SouthAfricanIdentityNumber.Random(2007, 2009, true).Number
        };

        protected BPerson p4Debbie1 => new BPerson
        {
            FirstName = "Debbie",
            LastName = "Adams",
            DateOfBirth = new DateTime(2017, 11, 17),
            Gender = EPersonGenders.Female,
            IdentityNumber = SouthAfricanIdentityNumber.Random(2017, 2018, true).Number
        };

        protected BPerson p5Eddie27 = new BPerson
        {
            FirstName = "Eddie",
            LastName = "Adams",
            DateOfBirth = new DateTime(1991, 3, 4),
            Gender = EPersonGenders.Male,
            IdentityNumber = SouthAfricanIdentityNumber.Random(1940, 1960, true).Number
        };
    }
}