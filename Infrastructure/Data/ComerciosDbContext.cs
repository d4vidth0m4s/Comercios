using Comercios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Comercios.Infrastructure.Data
{
    public class ComerciosDbContext(DbContextOptions<ComerciosDbContext> options) : DbContext(options)
    {
        public DbSet<Comercio> Comercios { get; set; }
        public DbSet<DatosComercio> DatosComercio { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("comercios");

            modelBuilder.Entity<Comercio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(26);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired();
                entity.HasOne(c => c.InfoComercio)
                      .WithOne(c => c.Comercio)
                      .HasForeignKey<DatosComercio>(l => l.Comercio_id);


            });
        }
    }
}
