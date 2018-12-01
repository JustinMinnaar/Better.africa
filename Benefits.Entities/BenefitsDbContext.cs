using Benefits.Shared;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Benefits.Entities
{
    public class BenefitsDbContext : DbContext
    {
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
            // TODO: MapPolicy(modelBuilder); // depends on member
        }

        private static EntityTypeConfiguration<T> MapBase<T>(DbModelBuilder modelBuilder) where T : BaseEntity
        {
            modelBuilder.Entity<T>().HasKey<Guid>(s => s.Id);

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

        //private static void MapPolicy(DbModelBuilder modelBuilder)
        //{
        //    var member = MapBase<Policy>(modelBuilder);

        //    member.Property(p => p.Number).IsOptional().HasMaxLength(50);
        //}

        public DbSet<Membership> Members { get; set; }

        public DbSet<Person> People { get; set; }

        //public DbSet<Policy> Policies { get; set; }
    }

    //public class BenefitsDbContextInitializer : dropCreateDatabase<BenefitsDbContext>
    //{
    //    protected override void Seed(BenefitsDbContext context)
    //    {
    //        base.Seed(context);
    //    }
    //}
}