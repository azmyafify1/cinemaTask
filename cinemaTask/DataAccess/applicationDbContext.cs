using cinemaTask.Models;
using Microsoft.EntityFrameworkCore;

namespace cinemaTask.DataAccess
{
    public class applicationDbContext : DbContext
    {
        internal object entities;

        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actormovie> Actormovies { get; set; }
        public DbSet<MovieSubimg> movieSubimgs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-HSF8481\\SQLEXPRESS;Initial Catalog = Cinema2;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key for the join table Actormovie
            modelBuilder.Entity<Actormovie>()
                .HasKey(am => new { am.ActorId, am.MovieId });

            // Relationship between Actor and Actormovie
            modelBuilder.Entity<Actormovie>()
                .HasOne(am => am.Actor)
                .WithMany(a => a.Actormovies)
                .HasForeignKey(am => am.ActorId);

            modelBuilder.Entity<Actormovie>()
                .HasOne(am => am.Movie)
                .WithMany(m => m.Actormovies)
                .HasForeignKey(am => am.MovieId);
        }
    }
}