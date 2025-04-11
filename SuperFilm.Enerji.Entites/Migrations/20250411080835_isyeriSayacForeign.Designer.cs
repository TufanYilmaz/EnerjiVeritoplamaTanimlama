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
    [Migration("20250411080835_isyeriSayacForeign")]
    partial class isyeriSayacForeign
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

                    b.Property<int>("IsletmeTanimlariId")
                        .HasColumnType("int");

                    b.Property<string>("Kod")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("IsletmeTanimlariId");

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

                    b.Property<string>("IsletmeAdi")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("IsletmeKodu")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("ISLETME_TANIMLARI");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.OpcNodes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NodeId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("NodeNameSpace")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("OPC_NODES");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.SayacTanimlari", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IsYeriId")
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

            modelBuilder.Entity("SuperFilm.Enerji.Entites.SayacVeri", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Ay")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Deger")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("NormalizeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SayacId")
                        .HasColumnType("int");

                    b.Property<string>("Yil")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<string>("Zaman")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.HasKey("Id");

                    b.HasIndex("SayacId");

                    b.ToTable("SAYAC_VERI");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsYeri", b =>
                {
                    b.HasOne("SuperFilm.Enerji.Entites.IsletmeTanimlari", "Isletme")
                        .WithMany("IsYeri")
                        .HasForeignKey("IsletmeTanimlariId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Isletme");
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

            modelBuilder.Entity("SuperFilm.Enerji.Entites.SayacTanimlari", b =>
                {
                    b.HasOne("SuperFilm.Enerji.Entites.IsYeri", "Isyeri")
                        .WithMany("SayacTanimlari")
                        .HasForeignKey("IsYeriId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Isyeri");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.SayacVeri", b =>
                {
                    b.HasOne("SuperFilm.Enerji.Entites.SayacTanimlari", "Sayac")
                        .WithMany()
                        .HasForeignKey("SayacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sayac");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsYeri", b =>
                {
                    b.Navigation("SayacTanimlari");
                });

            modelBuilder.Entity("SuperFilm.Enerji.Entites.IsletmeTanimlari", b =>
                {
                    b.Navigation("IsYeri");
                });
#pragma warning restore 612, 618
        }
    }
}
