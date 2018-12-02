using Benefits.Shared;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Benefits.Entities
{
    public class BenefitsDbContext : DbContext
    {
        public DbSet<BUser> Users { get; set; }

        public DbSet<BAudit> Audits { get; set; }

        public DbSet<BMembership> Memberships { get; set; }

        public DbSet<BPerson> People { get; set; }

        public DbSet<AulPolicyPlan> PolicyPlans { get; set; }

        public DbSet<BAulPolicy> Policies { get; set; }

        public DbSet<AulPolicyDependency> PolicyDependencies { get; set; }

        public DbSet<BOptions> Options { get; set; }

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
            MapUsers(modelBuilder);
            MapAudits(modelBuilder);
            MapOptions(modelBuilder);
            MapPerson(modelBuilder);
            MapMembership(modelBuilder); // depends on person
            MapPolicy(modelBuilder); // depends on member
            MapPolicyPlan(modelBuilder);
        }

        private void MapAudits(DbModelBuilder modelBuilder)
        {
            var audit = modelBuilder.Entity<BAudit>();
            audit.HasKey(s => s.Id);
            audit.ToTable("Audit");
            audit.Property(p => p.Action).IsRequired();
            audit.Property(p => p.EntityId).IsRequired();
            audit.Property(p => p.EntityType).IsRequired();
            audit.Property(p => p.UserId).IsRequired();
            audit.Property(p => p.When).IsRequired();
        }

        private void MapUsers(DbModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<BUser>();
            user.HasKey(s => s.Id);
            user.ToTable("User");
            user.Property(p => p.Name).IsRequired();
        }

        private void MapOptions(DbModelBuilder modelBuilder)
        {
            var options = modelBuilder.Entity<BOptions>();
            options.HasKey(s => s.Id);
            options.ToTable("Options");
            options.Property(p => p.LastContractNumber).IsRequired();
        }

        private static EntityTypeConfiguration<T> MapBase<T>
            (DbModelBuilder modelBuilder, string tableName) where T : BaseEntity
        {
            var e = modelBuilder.Entity<T>();

            e.ToTable(tableName);
            e.HasKey(s => s.Id);
            e.Map(m => m.MapInheritedProperties());

            e.Property(p => p.RowVersion).IsRequired().IsConcurrencyToken();
            e.Property(p => p.WorkflowStatus).IsRequired();

            return e;
        }

        private static void MapPerson(DbModelBuilder modelBuilder)
        {
            var person = MapBase<BPerson>(modelBuilder, "Person");

            person.Property(p => p.DateOfBirth).IsOptional().HasColumnType("date");
            person.Property(p => p.DateOfDeath).IsOptional().HasColumnType("date");
            person.Property(p => p.NameFirst).IsOptional().HasMaxLength(40);
            person.Property(p => p.NameLast).IsOptional().HasMaxLength(40);
        }

        private static void MapMembership(DbModelBuilder modelBuilder)
        {
            var membership = MapBase<BMembership>(modelBuilder, "Membership");

            membership.Property(p => p.Number).IsOptional();
            membership.Property(p => p.AgentId).IsOptional();
            membership.Property(p => p.CreatedById).IsRequired();
            membership.Property(p => p.CreatedOn).IsRequired();
            membership.Property(p => p.InceptionDate).IsOptional();
            membership.Property(p => p.Number).IsRequired();
            membership.Property(p => p.SignDate).IsOptional();

            var e = modelBuilder.Entity<BMembershipDependency>();
            e.ToTable("MembershipPerson");
            e.HasKey(s => new { s.MembershipId, s.PersonId });
        }

        private static void MapPolicy(DbModelBuilder modelBuilder)
        {
            var member = MapBase<BAulPolicy>(modelBuilder, "Policy");

            member.Property(p => p.Number).IsRequired();
            member.Property(p => p.InceptionDate).IsOptional();
            member.Property(p => p.PlanId).IsOptional();
            member.Property(p => p.SignDate).IsOptional();

            var dependecies = modelBuilder.Entity<AulPolicyDependency>()
                .HasKey(s => new { s.PolicyId, s.PersonId })
                .ToTable("PolicyDependency");
        }

        private void MapPolicyPlan(DbModelBuilder modelBuilder)
        {
            var plan = MapBase<AulPolicyPlan>(modelBuilder, "PolicyPlan");

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

    //public class BenefitsDbContextInitializer : DropCreateDatabaseAlways<BenefitsDbContext>
    //{
    //    protected override void Seed(BenefitsDbContext context)
    //    {
    //        base.Seed(context);
    //    }
    //}
}