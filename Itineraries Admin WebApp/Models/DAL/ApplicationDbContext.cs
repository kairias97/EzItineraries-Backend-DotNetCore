using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<TouristAttraction> TouristAttractions { get; set; }
        public DbSet<TouristAttractionSuggestion> TouristAttractionSuggestions { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TouristAttraction>()
                .OwnsOne(ta => ta.Geoposition, 
                geop => {
                    geop.Property(gp => gp.Latitude).HasColumnName("Latitude").HasColumnType("DECIMAL(8,6)");
                    geop.Property(gp => gp.Longitude).HasColumnName("Longitude").HasColumnType("DECIMAL(9,6)");
                });
            modelBuilder.Entity<TouristAttractionSuggestion>()
                .OwnsOne(tas => tas.Geoposition,
                geop => {
                    geop.Property(gp => gp.Latitude).HasColumnName("Latitude").HasColumnType("DECIMAL(8,6)");
                    geop.Property(gp => gp.Longitude).HasColumnName("Longitude").HasColumnType("DECIMAL(9,6)");
                });
            modelBuilder.Entity<Administrator>()
                .Property(a => a.Active)
                .HasDefaultValue(true);
            modelBuilder.Entity<TouristAttraction>()
                .Property(ta => ta.Active)
                .HasDefaultValue(true);
        }
    }
}
