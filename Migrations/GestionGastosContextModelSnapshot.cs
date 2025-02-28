﻿// <auto-generated />
using System;
using GestionGastos.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestionGastos.Migrations
{
    [DbContext(typeof(GestionGastosContext))]
    partial class GestionGastosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("GestionGastos.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("GestionGastos.Models.Gasto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("FechaCarga")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Monto")
                        .HasColumnType("longtext");

                    b.Property<string>("Tipo")
                        .HasColumnType("longtext");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Gastos");
                });

            modelBuilder.Entity("GestionGastos.Models.Ingreso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaIngreso")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Ingresos");
                });

            modelBuilder.Entity("GestionGastos.Models.Presupuesto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("MontoTotal")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Presupuestos");
                });

            modelBuilder.Entity("GestionGastos.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("GestionGastos.Models.Categoria", b =>
                {
                    b.HasOne("GestionGastos.Models.Usuario", "Usuario")
                        .WithMany("Categorias")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionGastos.Models.Gasto", b =>
                {
                    b.HasOne("GestionGastos.Models.Categoria", "Categoria")
                        .WithMany("Gastos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("GestionGastos.Models.Usuario", "Usuario")
                        .WithMany("Gastos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Categoria");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionGastos.Models.Ingreso", b =>
                {
                    b.HasOne("GestionGastos.Models.Usuario", "Usuario")
                        .WithMany("Ingresos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionGastos.Models.Presupuesto", b =>
                {
                    b.HasOne("GestionGastos.Models.Usuario", "Usuario")
                        .WithMany("Presupuestos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionGastos.Models.Categoria", b =>
                {
                    b.Navigation("Gastos");
                });

            modelBuilder.Entity("GestionGastos.Models.Usuario", b =>
                {
                    b.Navigation("Categorias");

                    b.Navigation("Gastos");

                    b.Navigation("Ingresos");

                    b.Navigation("Presupuestos");
                });
#pragma warning restore 612, 618
        }
    }
}
