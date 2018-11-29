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
            //Database.SetInitializer<BenefitsDbContext>(new CreateDatabaseIfNotExists<BenefitsDbContext>());
            Database.SetInitializer<BenefitsDbContext>(new DropCreateDatabaseIfModelChanges<BenefitsDbContext>());
            //Database.SetInitializer<BenefitsDbContext>(new DropCreateDatabaseAlways<BenefitsDbContext>());
            //Database.SetInitializer<BenefitsDbContext>(new SchoolDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure primary key
            modelBuilder.Entity<Member>().HasKey<Guid>(s => s.Id);
            modelBuilder.Entity<Person>().HasKey<Guid>(s => s.Id);

            var e = modelBuilder.Entity<CEntity>();
            e.Property(p => p.RowVersion).IsConcurrencyToken();

            var person = modelBuilder.Entity<Person>();
            //person.Property(p => p.MemberId).HasColumnName("MemberId").IsOptional();
            person.Property(p => p.DateOfDeath).IsOptional().HasColumnType("date");
            person.Property(p => p.DateOfBirth).IsOptional().HasColumnType("date");
            person.Property(p => p.NameFirst).IsOptional().HasMaxLength(50);
            person.Property(p => p.NameLast).IsOptional().HasMaxLength(50);
            person.Property(p => p.Status).IsRequired();
            person.Property(p => p.Type).IsOptional();
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Person> People { get; set; }
    }

    public class BenefitsDbContextInitializer : CreateDatabaseIfNotExists<BenefitsDbContext>
    {
        protected override void Seed(BenefitsDbContext context)
        {
            base.Seed(context);
        }
    }
}