using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.db
{
    public class ClinicaDBContext : DbContext
    {
        public ClinicaDBContext(DbContextOptions<ClinicaDBContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.History)
                .WithOne()
                .HasForeignKey<History>(h => h.PetId);

            modelBuilder.Entity<History>()
                .HasMany(h => h.Vaccines)
                .WithOne()
                .HasForeignKey(v => v.PetId);
        }
    }
}
