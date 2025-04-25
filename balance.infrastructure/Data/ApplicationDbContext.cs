using balance.domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace balance.infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<Applicationuser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyFeature> PropertyFeatures { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<GeoLocation> GeoLocations { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationship for Property and GeoLocation
            modelBuilder.Entity<Property>()
                .HasOne(p => p.GeoLocation)
                .WithOne(g => g.Property)
                .HasForeignKey<GeoLocation>(g => g.PropertyId);

            // Configure circular reference between Project and Location
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Location)
                .WithOne(l => l.Project)
                .HasForeignKey<Location>(l => l.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure other relationships
            modelBuilder.Entity<Property>()
                .HasOne(p => p.PropertyType)
                .WithMany(pt => pt.Properties)
                .HasForeignKey(p => p.PropertyTypeId);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Project)
                .WithMany(pr => pr.Properties)
                .HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Agent)
                .WithMany()
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-many configurations can be added as needed for PropertyFeature
        }
    }
}
