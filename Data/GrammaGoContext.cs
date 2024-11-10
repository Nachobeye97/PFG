using Microsoft.EntityFrameworkCore;
using GrammaGo.Server.Models;

namespace GrammaGo.Server.Data
{
    public class GrammaGoContext : DbContext
    {
        public GrammaGoContext(DbContextOptions<GrammaGoContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseResult> ExerciseResults { get; set; } // Agrega esta línea

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la propiedad 'Id' como clave primaria y autogenerada para Usuario
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();  // Asegurar que el 'Id' se genera automáticamente en la base de datos
            
            // Configuración para ExerciseResult
            modelBuilder.Entity<ExerciseResult>()
                .HasKey(er => er.Id); // Asegúrate de que la clase ExerciseResult tenga una propiedad Id

            modelBuilder.Entity<ExerciseResult>()
                .Property(er => er.Id)
                .ValueGeneratedOnAdd(); // Asegurar que el 'Id' se genera automáticamente en la base de datos
        }
    }
}
