using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ItinerariesApi.Models.DAL
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }
        public virtual DbSet<TouristAttractionConnection> TouristAttractionConnections { get; set; }
        public virtual DbSet<TouristAttraction> TouristAttractions { get; set; }
        public virtual DbSet<TouristAttractionSuggestion> TouristAttractionSuggestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasIndex(e => e.CountryId);

                entity.Property(e => e.CountryId)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IsoNumericCode);

                entity.Property(e => e.IsoNumericCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Alpha2Code)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Alpha3Code)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasIndex(e => e.SentBy);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.SentByNavigation)
                    .WithMany(p => p.Invitations)
                    .HasForeignKey(d => d.SentBy);
            });

            modelBuilder.Entity<TouristAttractionConnection>(entity =>
            {
                entity.HasKey(e => new { e.CityId, e.OriginId, e.DestinationId });

                entity.HasIndex(e => e.DestinationId);

                entity.HasIndex(e => e.OriginId);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TouristAttractionConnections)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.TouristAttractionConnectionsDestination)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Origin)
                    .WithMany(p => p.TouristAttractionConnectionsOrigin)
                    .HasForeignKey(d => d.OriginId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TouristAttraction>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.HasIndex(e => e.CityId);

                entity.HasIndex(e => e.CreatedBy);

                entity.HasIndex(e => e.GooglePlaceId)
                    .IsUnique()
                    .HasFilter("([GooglePlaceId] IS NOT NULL)");

                entity.Property(e => e.Address)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.GooglePlaceId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.WebsiteUrl)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TouristAttractions)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TouristAttractions)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TouristAttractions)
                    .HasForeignKey(d => d.CreatedBy);
            });

            modelBuilder.Entity<TouristAttractionSuggestion>(entity =>
            {
                entity.HasIndex(e => e.AnsweredBy);

                entity.HasIndex(e => e.CategoryId);

                entity.HasIndex(e => e.CityId);

                entity.Property(e => e.Address)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.GooglePlaceId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.WebsiteUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.AnsweredByNavigation)
                    .WithMany(p => p.TouristAttractionSuggestions)
                    .HasForeignKey(d => d.AnsweredBy);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TouristAttractionSuggestions)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TouristAttractionSuggestions)
                    .HasForeignKey(d => d.CityId);
            });
            modelBuilder.Entity<TouristAttraction>()
                .OwnsOne(ta => ta.Geoposition,
                geop => {
                    geop.Property(gp => gp.Latitude).HasColumnName("Latitude").HasColumnType("float(53)");
                    geop.Property(gp => gp.Longitude).HasColumnName("Longitude").HasColumnType("float(53)");
                });
            modelBuilder.Entity<TouristAttractionSuggestion>()
                .OwnsOne(tas => tas.Geoposition,
                geop => {
                    geop.Property(gp => gp.Latitude).HasColumnName("Latitude").HasColumnType("float(53)");
                    geop.Property(gp => gp.Longitude).HasColumnName("Longitude").HasColumnType("float(53)");
                });
        }
    }
}
