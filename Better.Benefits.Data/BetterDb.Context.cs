﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Better.Benefits.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BetterDb : DbContext
    {
        public BetterDb()
            : base("name=BetterDb")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Policy> Policy { get; set; }
        public virtual DbSet<Policy_Member> Policy_Member { get; set; }
        public virtual DbSet<Policy_Receipt> Policy_Receipt { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Policy_Type> Policy_Type { get; set; }
        public virtual DbSet<Policy_Status> Policy_Status { get; set; }
        public virtual DbSet<Person_Identity> Person_Identity { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Person_IdentityType> Person_IdentityType { get; set; }
        public virtual DbSet<Member> MemberSet { get; set; }
    }
}
