using Benefits.Shared;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Benefits.Entities
{
    public class BenefitsDbContext : DbContext
    {
        public DbSet<Membership> Members { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<AulPolicyPlan> PolicyPlans { get; set; }

        public DbSet<AulPolicy> Policies { get; set; }

        public DbSet<AulPolicyDependency> PolicyDependencies { get; set; }

        public BenefitsDbContext() : base("name=Benefits")
        {
            Database.CommandTimeout = 3;
            //Database.SetInitializer<BenefitsDbContext>(new CreateDatabaseIfNotExists<BenefitsDbContext>());
            //Database.SetInitializer<BenefitsDbContext>(new DropCreateDatabaseIfModelChanges<BenefitsDbContext>());
            Database.SetInitializer<BenefitsDbContext>(new DropCreateDatabaseAlways<BenefitsDbContext>());
            //Database.SetInitializer<BenefitsDbContext>(new SchoolDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            MapPerson(modelBuilder);
            MapMember(modelBuilder); // depends on person
            MapPolicy(modelBuilder); // depends on member
            MapPolicyPlan(modelBuilder);
        }

        private static EntityTypeConfiguration<T> MapBase<T>(DbModelBuilder modelBuilder) where T : BaseEntity
        {
            modelBuilder.Entity<T>().HasKey(s => s.Id);

            var e = modelBuilder.Entity<T>();
            e.Property(p => p.RowVersion).IsRequired().IsConcurrencyToken();
            e.Property(p => p.WorkflowStatus).IsRequired();
            e.Map(m => m.MapInheritedProperties());

            return e;
        }

        private static void MapPerson(DbModelBuilder modelBuilder)
        {
            var person = MapBase<Person>(modelBuilder);

            person.Property(p => p.DateOfBirth).IsOptional().HasColumnType("date");
            person.Property(p => p.DateOfDeath).IsOptional().HasColumnType("date");
            person.Property(p => p.NameFirst).IsOptional().HasMaxLength(40);
            person.Property(p => p.NameLast).IsOptional().HasMaxLength(40);
            person.Property(p => p.MembershipType).IsOptional();
        }

        private static void MapMember(DbModelBuilder modelBuilder)
        {
            var member = MapBase<Membership>(modelBuilder);

            member.Property(p => p.Number).IsOptional().HasMaxLength(50);
        }

        private static void MapPolicy(DbModelBuilder modelBuilder)
        {
            var member = MapBase<AulPolicy>(modelBuilder);

            member.Property(p => p.Number).IsOptional().HasMaxLength(50);
            member.Property(p => p.InceptionDate).IsOptional();
            member.Property(p => p.PlanId).IsOptional();
            member.Property(p => p.SignDate).IsOptional();

            var dependecies = modelBuilder.Entity<AulPolicyDependency>()
                .HasKey(s => new { s.PolicyId, s.PersonId });
        }

        private void MapPolicyPlan(DbModelBuilder modelBuilder)
        {
            var plan = MapBase<AulPolicyPlan>(modelBuilder);

            plan.Property(p => p.LastPolicyNumberIssued).IsRequired();
            plan.Property(p => p.MonthlyCostChild).IsRequired();
            plan.Property(p => p.MonthlyCostChildren).IsRequired();
            plan.Property(p => p.MonthlyCostFamily).IsRequired();
            plan.Property(p => p.MonthlyCostPrincipal).IsRequired();
            plan.Property(p => p.MonthlyCostSpouse).IsRequired();
            plan.Property(p => p.MaxAgeAdult).IsRequired();
            plan.Property(p => p.MaxAgeChild).IsRequired();
            plan.Property(p => p.MaxAgeChildScholar).IsRequired();
            plan.Property(p => p.MaxAgePrincipal).IsRequired();
            plan.Property(p => p.MaxAgeSpouse).IsRequired();
            plan.Property(p => p.MinAgeAdult).IsRequired();
            plan.Property(p => p.MinAgeChild).IsRequired();
            plan.Property(p => p.MinAgePrincipal).IsRequired();
            plan.Property(p => p.MinAgeSpouse).IsRequired();
        }
    }

    //public class BenefitsDbContextInitializer : dropCreateDatabase<BenefitsDbContext>
    //{
    //    protected override void Seed(BenefitsDbContext context)
    //    {
    //        base.Seed(context);
    //    }
    //}
}