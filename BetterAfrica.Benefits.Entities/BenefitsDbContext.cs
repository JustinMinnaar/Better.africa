using BetterAfrica.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAfrica.Benefits.Entities
{
    public class BenefitsDbContext : DbContext
    {
        public DbSet<BUser> Users { get; set; }

        public DbSet<BAudit> Audits { get; set; }

        public DbSet<BMember> Members { get; set; }

        public DbSet<BMemberDependency> MemberDependencies { get; set; }

        public BMember GetMember(int id)
        {
            var membership = Members
                .Include(nameof(BMember.Dependencies))
                .Include(m => m.Dependencies.Select(d => d.Person))
                .Include(nameof(BMember.Agent))
                .AsQueryable()
                .FirstOrDefault(m => m.EntityId == id);
            return membership;
        }

        public DbSet<BPerson> People { get; set; }

        public DbSet<AulPolicyPlan> PolicyPlans { get; set; }

        public DbSet<AulPolicy> Policies { get; set; }

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
            MapMember(modelBuilder); // depends on person
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
            audit.Property(p => p.Description).IsOptional();
        }

        private void MapUsers(DbModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<BUser>();
            //user.HasKey(s => s.Id);
            user.HasIndex(s => s.EntityId).IsUnique();
            user.ToTable("User");
            user.Property(p => p.Name).IsRequired();
        }

        private void MapOptions(DbModelBuilder modelBuilder)
        {
            var options = modelBuilder.Entity<BOptions>();
            options.HasKey(s => s.Id);
            options.HasIndex(s => s.EntityId).IsUnique();
            options.ToTable("Options");
            options.Property(p => p.LastContractNumber).IsRequired();
        }

        private static EntityTypeConfiguration<T> MapBase<T>
            (DbModelBuilder modelBuilder, string tableName) where T : BaseEntity
        {
            var e = modelBuilder.Entity<T>();

            e.HasKey(s => s.Id);
            e.HasIndex(s => s.EntityId).IsUnique();
            e.Map(m => m.MapInheritedProperties());

            e.ToTable(tableName);
            e.Property(p => p.EntityVersion).IsRequired().IsConcurrencyToken();
            e.Property(p => p.EntityWorkflowStatus).IsRequired();

            e.Property(p => p.EntityWorkflowByUserId).IsOptional();

            return e;
        }

        private static void MapPerson(DbModelBuilder modelBuilder)
        {
            var person = MapBase<BPerson>(modelBuilder, "Person");

            person.Property(p => p.DateOfBirth).IsOptional().HasColumnType("date");
            person.Property(p => p.DateOfDeath).IsOptional().HasColumnType("date");
            person.Property(p => p.FirstName).IsOptional().HasMaxLength(40);
            person.Property(p => p.LastName).IsOptional().HasMaxLength(40);
            person.Property(p => p.CellPhone.Number).HasColumnName("CellPhoneNumber").IsOptional().HasMaxLength(40);
            person.Property(p => p.CellPhone.Dial).HasColumnName("CellPhoneDial").IsOptional().HasMaxLength(40);
            person.Property(p => p.HomePhone.Number).HasColumnName("HomePhoneNumber").IsOptional().HasMaxLength(40);
            person.Property(p => p.HomePhone.Dial).HasColumnName("HomePhoneDial").IsOptional().HasMaxLength(40);
            person.Property(p => p.WorkPhone.Number).HasColumnName("WorkPhoneNumber").IsOptional().HasMaxLength(40);
            person.Property(p => p.WorkPhone.Dial).HasColumnName("WorkPhoneDial").IsOptional().HasMaxLength(40);
        }

        private static void MapMember(DbModelBuilder modelBuilder)
        {
            var membership = MapBase<BMember>(modelBuilder, "Member");

            membership.Property(p => p.Number).IsOptional();
            membership.Property(p => p.AgentId).IsOptional();
            membership.Property(p => p.EntityUserId).IsRequired();
            membership.Property(p => p.EntityCreatedOn).IsRequired();
            membership.Property(p => p.InceptionDate).IsOptional();
            membership.Property(p => p.Number).IsRequired();
            membership.Property(p => p.SignDate).IsOptional();

            var e = modelBuilder.Entity<BMemberDependency>();
            e.ToTable("MemberDependency");
            e.HasKey(s => new { s.MemberId, s.PersonId });
            e.Property(p => p.Type).IsRequired();
        }

        private static void MapPolicy(DbModelBuilder modelBuilder)
        {
            var member = MapBase<AulPolicy>(modelBuilder, "Policy");

            member.Property(p => p.Number).IsRequired();
            member.Property(p => p.InceptionDate).IsOptional();
            member.Property(p => p.PlanId).IsOptional();
            member.Property(p => p.SignDate).IsOptional();

            var e = modelBuilder.Entity<AulPolicyDependency>();
            e.ToTable("PolicyDependency");
            e.HasKey(s => new { s.PolicyId, s.PersonId });
            e.Property(p => p.Type).IsRequired();
        }

        private void MapPolicyPlan(DbModelBuilder modelBuilder)
        {
            var plan = MapBase<AulPolicyPlan>(modelBuilder, "PolicyPlan");

            plan.Property(p => p.LastPolicyNumberIssued).IsRequired();
            plan.Property(p => p.MonthlyCostChild).IsRequired();
            plan.Property(p => p.MonthlyCostChildren).IsRequired();
            plan.Property(p => p.MonthlyCostFamilyUnder76).IsRequired();
            plan.Property(p => p.MonthlyCostPrincipalUnder66).IsRequired();
            plan.Property(p => p.MonthlyCostSpouse18to65).IsRequired();
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