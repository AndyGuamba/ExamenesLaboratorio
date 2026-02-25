using Examenes.Modelos.Enums;
using Microsoft.EntityFrameworkCore;
using Examenes.Modelos;
using Examenes.Modelos.Entidades;
namespace Examenes.API.Data
{
    public class ExamenApiContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos el Enum para que se guarde como texto
            modelBuilder.Entity<Examen>()
                .Property(e => e.Tipo)
                .HasConversion<string>();
            modelBuilder.Entity<ParametroExamen>()
                .Property(p => p.Categoria)
                .HasConversion<string>();
        }
        public ExamenApiContext(DbContextOptions<ExamenApiContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<ResultadoExamen> ResultadosExamenes { get; set; }
        public DbSet<ParametroExamen> Parametros { get; set; }
    }
}
