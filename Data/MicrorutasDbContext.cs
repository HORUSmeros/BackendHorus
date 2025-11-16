using BackendHorus.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendHorus.Data
{
    public class MicrorutasDbContext : DbContext
    {
        public MicrorutasDbContext(DbContextOptions<MicrorutasDbContext> options)
            : base(options)
        {
        }

        public DbSet<Macroruta> Macrorutas { get; set; } = null!;
        public DbSet<Microruta> Microrutas { get; set; } = null!;
        public DbSet<Recolector> Recolectores { get; set; } = null!;
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<PositionSample> PositionSamples { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;
        
        public DbSet<RecolectorStats> RecolectorStats { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Macroruta
            modelBuilder.Entity<Macroruta>()
                .Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            // Microruta
            modelBuilder.Entity<Microruta>()
                .Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Microruta>()
                .Property(m => m.PolylineJson)
                .IsRequired()
                .HasDefaultValue("[]");

            // Recolector
            modelBuilder.Entity<Recolector>()
                .Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(120);

            // Relaciones básicas
            modelBuilder.Entity<Microruta>()
                .HasOne(m => m.Macroruta)
                .WithMany(ma => ma.Microrutas)
                .HasForeignKey(m => m.MacrorutaId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Recolector)
                .WithMany(r => r.Trips)
                .HasForeignKey(t => t.RecolectorId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Microruta)
                .WithMany(m => m.Trips)
                .HasForeignKey(t => t.MicrorutaId);

            modelBuilder.Entity<PositionSample>()
                .HasOne(p => p.Trip)
                .WithMany(t => t.Positions)
                .HasForeignKey(p => p.TripId);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Trip)
                .WithMany(t => t.Incidents)
                .HasForeignKey(i => i.TripId);
        }
    }
}
