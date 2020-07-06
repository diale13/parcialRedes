using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using System.Data.Entity;

namespace Persistance
{
    public class Context : DbContext
    {
        public Context() : base("name=Context")
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MovieGenreAssociation> MovieGenreAssociations { get; set; }
        public DbSet<FileMovie> FileMovies { get; set; }
        public DbSet<MovieUserAssociation> MovieUserAssociations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>().ToTable("Peliculas");
            modelBuilder.Entity<Director>().ToTable("Directores");
            modelBuilder.Entity<Genre>().ToTable("Generos");
            modelBuilder.Entity<User>().ToTable("Usuarios");
            modelBuilder.Entity<MovieGenreAssociation>().ToTable("Bridge_Movie_Genre");
            modelBuilder.Entity<FileMovie>().ToTable("Files");
            modelBuilder.Entity<MovieUserAssociation>().ToTable("Bridge_Movie_User");

            modelBuilder.Entity<Movie>().Property(m => m.Name).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Movie>().HasKey(m => m.Name);
            modelBuilder.Entity<Director>().Property(d => d.Name).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Director>().HasKey(d => d.Name);
            modelBuilder.Entity<Genre>().Property(g => g.Name).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Genre>().HasKey(g => g.Name);
            modelBuilder.Entity<User>().Property(u => u.Identifier).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>().HasKey(u => u.Identifier);
            modelBuilder.Entity<MovieGenreAssociation>().Property(a => a.GenreName).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MovieGenreAssociation>().HasKey(a => new { a.GenreName, a.MovieName });
            modelBuilder.Entity<FileMovie>().Property(f => f.Path).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<FileMovie>().HasKey(f => f.Path);
            modelBuilder.Entity<MovieUserAssociation>().Property(a => a.Movie).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MovieUserAssociation>().HasKey(a => new { a.Movie, a.User });
        }
    }
}