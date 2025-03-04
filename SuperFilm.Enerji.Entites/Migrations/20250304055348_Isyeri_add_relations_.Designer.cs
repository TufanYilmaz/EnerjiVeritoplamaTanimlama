﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuperFilm.Enerji.Entites;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    [DbContext(typeof(EnerjiDbContext))]
    [Migration("20250304055348_Isyeri_add_relations_")]
    partial class Isyeri_add_relations_
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsYeri", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Kod")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("IS_YERI");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsletmeSayacDagilimi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Carpan")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Islem")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("IsletmeId")
                        .HasColumnType("int");

                    b.Property<int>("SayacId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IsletmeId");

                    b.HasIndex("SayacId");

                    b.ToTable("ISLETME_SAYAC_DAGILIMI");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsletmeTanimlari", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Aciklama")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("IsYeriId")
                        .HasColumnType("int");

                    b.Property<string>("IsletmeAdi")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("IsletmeKodu")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("IsYeriId");

                    b.ToTable("ISLETME_TANIMLARI");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.SayacTanimlari", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("IsYeriId")
                        .HasColumnType("int");

                    b.Property<string>("SayacAciklama")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SayacKodu")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("SayacTanimi")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("SayacYeri")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("IsYeriId");

                    b.ToTable("SAYAC_TANIMLARI");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsletmeSayacDagilimi", b =>
                {
                    b.HasOne("SuperFilm.Enerji.Entites.IsletmeTanimlari", "Isletme")
                        .WithMany()
                        .HasForeignKey("IsletmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SuperFilm.Enerji.Entites.SayacTanimlari", "Sayac")
                        .WithMany()
                        .HasForeignKey("SayacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Isletme");

                    b.Navigation("Sayac");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsletmeTanimlari", b =>
                {
                    b.HasOne("SuperFilm.Enerji.Entites.IsYeri", "IsYeri")
                        .WithMany()
                        .HasForeignKey("IsYeriId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IsYeri");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.SayacTanimlari", b =>
                {
                    b.HasOne("SuperFilm.Enerji.Entites.IsYeri", null)
                        .WithMany("SayacTanimlari")
                        .HasForeignKey("IsYeriId");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsYeri", b =>
                {
                    b.Navigation("SayacTanimlari");
                });
#pragma warning restore 612, 618
        }
    }
}
