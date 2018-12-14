using Knights.Fluid.Datums;
using System;

namespace BetterAfrica.Benefits.Entities.UnitTests
{
    public abstract class BasePersonUnitTests
    {
        // NOTE: It is necessary for testing duplications to always create a new person

        protected CPerson p1Adam49 => new CPerson
        {
            FirstName = "Adam",
            LastName = "Adams",
            DateOfBirth = new DateTime(1969, 07, 31),
            IdentityNumber = SouthAfricanIdentityNumber.Random(1960, 1980, true).Number,
            Gender = EPersonGenders.Male,
            CellPhone = new Phone("0813702097"),
            HomePhone = null,
            WorkPhone = null,
            EmailAddress = null,
            WorkName = null,
        };

        protected CPerson p2Bertha47 => new CPerson
        {
            FirstName = "Bertha",
            LastName = "Adams",
            DateOfBirth = new DateTime(1971, 11, 12),
            Gender = EPersonGenders.Female,
            IdentityNumber = SouthAfricanIdentityNumber.Random(1960, 1980, true).Number
        };

        protected CPerson p3Charles11 => new CPerson
        {
            FirstName = "Charles",
            LastName = "Adams",
            DateOfBirth = new DateTime(2007, 3, 7),
            Gender = EPersonGenders.Male,
            IdentityNumber = SouthAfricanIdentityNumber.Random(2007, 2009, true).Number
        };

        protected CPerson p4Debbie1 => new CPerson
        {
            FirstName = "Debbie",
            LastName = "Adams",
            DateOfBirth = new DateTime(2017, 11, 17),
            Gender = EPersonGenders.Female,
            IdentityNumber = SouthAfricanIdentityNumber.Random(2017, 2018, true).Number
        };

        protected CPerson p5Eddie27 = new CPerson
        {
            FirstName = "Eddie",
            LastName = "Adams",
            DateOfBirth = new DateTime(1991, 3, 4),
            Gender = EPersonGenders.Male,
            IdentityNumber = SouthAfricanIdentityNumber.Random(1940, 1960, true).Number
        };
    }
}