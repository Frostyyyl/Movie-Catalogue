using GrobelnyKasprzak.MovieCatalogue.DAOSql.Models;
using GrobelnyKasprzak.MovieCatalogue.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrobelnyKasprzak.MovieCatalogue.DAOSql
{
    public class MovieCatalogueContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Studio> Studios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=movies.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IMovie>() // NOTE: This should(?) be Movie and not IMovie
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies);

            modelBuilder.Entity<IStudio>() // NOTE: This should(?) be Studio and not IStudio
                .HasMany(s => s.Movies)
                .WithOne(m => m.Studio)
                .HasForeignKey(m => m.StudioId);
        }
    }

}
