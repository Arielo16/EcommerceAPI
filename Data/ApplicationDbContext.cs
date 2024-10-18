using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Collections.Generic;
using EcommerceAPI.Models; // Asegúrate de que este namespace coincida con tu proyecto
using System;

namespace EcommerceAPI.Data // Asegúrate de que este namespace coincida con tu proyecto
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .Property(p => p.ProductosIds)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null));
        }
    }
}