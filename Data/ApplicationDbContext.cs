using ClinicCare.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicCare.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientVisit> PatientVisits { get; set; }
        public DbSet<MedicineCategory> MedicineCategories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PatientVisit>()
                .Property(p => p.AmountPaid)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Medicine>()
    .Property(m => m.PurchasePrice)
    .HasPrecision(10, 2);
        }
    }
}