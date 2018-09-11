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
        public DbSet<City> Cities { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<TouristAttraction> TouristAttractions { get; set; }
        public DbSet<TouristAttractionSuggestion> TouristAttractionSuggestions { get; set; }
        public DbSet<TouristAttractionConnection> TouristAttractionConnections { get; set; }
        
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TouristAttraction>()
                .OwnsOne(ta => ta.Geoposition, 
                geop => {
                    geop.Property(gp => gp.Latitude).HasColumnName("Latitude").HasColumnType("float(53)");
                    geop.Property(gp => gp.Longitude).HasColumnName("Longitude").HasColumnType("float(53)");
                });
            modelBuilder.Entity<TouristAttraction>()
                .HasIndex(ta => ta.GooglePlaceId)
                .IsUnique();

            modelBuilder.Entity<TouristAttractionSuggestion>()
                .OwnsOne(tas => tas.Geoposition,
                geop => {
                    geop.Property(gp => gp.Latitude).HasColumnName("Latitude").HasColumnType("float(53)");
                    geop.Property(gp => gp.Longitude).HasColumnName("Longitude").HasColumnType("float(53)");
                });
            modelBuilder.Entity<Administrator>()
                .HasIndex(a => a.Email)
                .IsUnique();
            
            modelBuilder.Entity<TouristAttractionSuggestion>()
                .Property(ta => ta.Approved)
                .HasDefaultValue(null);

            modelBuilder.Entity<TouristAttractionSuggestion>()
                .Property(ta => ta.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<TouristAttractionConnection>()
                .HasKey(tad => new { tad.CityId, tad.OriginId, tad.DestinationId });
            modelBuilder.Entity<TouristAttractionConnection>()
                .HasOne(tad => tad.Origin)
                .WithMany(ta => ta.OriginPositionDistances)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TouristAttractionConnection>()
                .HasOne(tad => tad.Destination)
                .WithMany(ta => ta.DestinationPositionDistances)
                .HasForeignKey(tad => tad.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
