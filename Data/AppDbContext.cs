using Microsoft.EntityFrameworkCore;
using ArtGallery_Backend.Model.Domain;


namespace ArtGallery_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Art> Art { get; set; }
        public DbSet<ArtCategory> ArtCategory { get; set; }
        public DbSet<Favourites> Favourites { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<UserSession> UserSession { get; set; }




    } 
}
