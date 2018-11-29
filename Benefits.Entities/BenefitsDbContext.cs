using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            MapMember(modelBuilder);
        }

        private static void MapPerson(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey<Guid>(s => s.Id);

            var person = modelBuilder.Entity<Person>();
            person.Map(m => m.MapInheritedProperties());
            person.Property(p => p.RowVersion).IsRequired().IsConcurrencyToken();
            person.Property(p => p.WorkflowStatus).IsRequired();
            person.Property(p => p.DateOfDeath).IsOptional().HasColumnType("date");
            person.Property(p => p.DateOfBirth).IsOptional().HasColumnType("date");
            person.Property(p => p.NameFirst).IsOptional().HasMaxLength(40);
            person.Property(p => p.NameLast).IsOptional().HasMaxLength(40);
            person.Property(p => p.WorkflowStatus).IsRequired();
            person.Property(p => p.MembershipType).IsOptional();
        }

        private static void MapMember(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasKey<Guid>(s => s.Id);
            var member = modelBuilder.Entity<Member>();
            member.Map(m => m.MapInheritedProperties());
            member.Property(p => p.Number).IsOptional().HasMaxLength(50);
            member.Property(p => p.RowVersion).IsRequired().IsConcurrencyToken();
            member.Property(p => p.WorkflowStatus).IsRequired();
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Person> People { get; set; }
    }

    //public class BenefitsDbContextInitializer : dropCreateDatabase<BenefitsDbContext>
    //{
    //    protected override void Seed(BenefitsDbContext context)
    //    {
    //        base.Seed(context);
    //    }
    //}
}