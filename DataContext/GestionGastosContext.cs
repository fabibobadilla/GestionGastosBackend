using GestionGastos;
using GestionGastos.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace GestionGastos.DataContext
{
    public class GestionGastosContext : DbContext
    {
        public GestionGastosContext(DbContextOptions<GestionGastosContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación entre Gasto y Categoria
            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Categoria) // Un gasto tiene una categoría
                .WithMany(c => c.Gastos) // Una categoría tiene muchos gastos
                .HasForeignKey(g => g.CategoriaId) // La clave foránea es CategoriaId
                .OnDelete(DeleteBehavior.SetNull); // Si se elimina la categoría, establece null en CategoriaId

            // Relación entre Categoria y Usuario (Uno a Uno)
            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Usuario) // Categoria tiene un Usuario
                .WithMany(u => u.Categorias) // Un Usuario tiene muchas Categorias
                .HasForeignKey(c => c.UsuarioId) // Clave foránea
                .OnDelete(DeleteBehavior.Cascade); // Cascada: si se elimina el Usuario, se eliminan las Categorias

            // Relación entre Usuario y Categoria (Uno a Muchos)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Categorias) // Un Usuario tiene muchas Categorias
                .WithOne(c => c.Usuario) // Una Categoria pertenece a un Usuario
                .HasForeignKey(c => c.UsuarioId) // Clave foránea en Categoria
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina el Usuario, se eliminan las Categorias

            // Relación entre Usuario y Gasto (Uno a Muchos)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Gastos) // Un Usuario tiene muchos Gastos
                .WithOne(g => g.Usuario) // Un Gasto pertenece a un Usuario
                .HasForeignKey(g => g.UsuarioId) // Clave foránea en Gasto
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina el Usuario, se eliminan los Gastos

            // Relación entre Usuario y Presupuesto (Uno a Muchos)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Presupuestos) // Un Usuario tiene muchos Presupuestos
                .WithOne(p => p.Usuario) // Un Presupuesto pertenece a un Usuario
                .HasForeignKey(p => p.UsuarioId) // Clave foránea en Presupuesto
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina el Usuario, se eliminan los Presupuestos


        }
    }
}
