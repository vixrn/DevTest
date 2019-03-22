namespace DeveloperTest_ThomasParfitt.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CarRentalDBModel : DbContext
    {
        public CarRentalDBModel()
            : base("name=CarRentalDBModel")
        {
        }

        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Returns> Returns { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservations>()
                .HasMany(e => e.Returns)
                .WithRequired(e => e.Reservations)
                .HasForeignKey(e => e.BookingNumber)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.VehicleType)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Vehicle)
                .HasForeignKey(e => e.VehID)
                .WillCascadeOnDelete(false);
        }
    }
}
