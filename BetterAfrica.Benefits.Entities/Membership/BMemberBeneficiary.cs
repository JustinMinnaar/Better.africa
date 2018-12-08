using BetterAfrica.Shared;
using Knights.Core.Common;
using Knights.Core.Nodes;
using Knights.Fluid.Datums;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BetterAfrica.Benefits.Entities
{
    [Nickname("beneficiary")]
    public class BMemberBeneficiary : BaseEntity
    {
        /// <summary>The beneficiary code is B# from 1 to N for each membership.</summary>
        public string Code { get; set; }

        /// <summary>The person or company name of the beneficiary.</summary>
        public string Name { get; set; }

        /// <summary>The email address of the beneficiary.</summary>
        public Email Email { get; set; }

        /// <summary>The person identity number or company registration number of the beneficiary.</summary>
        public string IdentityOrRegistrationNumber { get; set; }

        /// <summary>The phone number of the person or company.</summary>
        public Phone Phone { get; set; }

        /// <summary>The ratio of payment, which by default is 1. Each beneficiary gains this many pieces of the total.</summary>
        public int Ratio { get; set; } = 1;
    }
}