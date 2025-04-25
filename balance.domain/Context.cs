using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace balance.domain
{
    public class Context : IdentityDbContext<Applicationuser>
    {
        public Context(DbContextOptions<Context> option) : base(option)
        {


        }
        public DbSet<Property> Property { get; set; }
        public DbSet<Project> Project { get; set; }

        public DbSet<PropertyFeature> PropertyFeature { get; set; }
        public DbSet<PropertyImage> PropertyImage { get; set; }
        public DbSet<PropertyType> PropertyType { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<GeoLocation> GeoLocations { get; set; }


        public DbSet<Favorite> Favourite { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Property>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); 
      


        }
       

    }
    
}
