
using Microsoft.EntityFrameworkCore;
using SistemaTurnosCentroUnas.Models;

namespace SistemaTurnosCentroUnas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Turno> Turnos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Turnos)
                .WithOne(t => t.Cliente)
                .HasForeignKey(t => t.ClienteId);

            modelBuilder.Entity<Turno>()
                .Property(t => t.Precio)
                .HasPrecision(18, 2);
        }
    }
}