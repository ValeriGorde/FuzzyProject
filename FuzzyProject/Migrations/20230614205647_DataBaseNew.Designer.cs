﻿// <auto-generated />
using System;
using FuzzyProject.DB_EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FuzzyProject.Migrations
{
    [DbContext(typeof(AppContextDB))]
    [Migration("20230614205647_DataBaseNew")]
    partial class DataBaseNew
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("FuzzyProject.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FuzzyProject.Models.Colorant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Colorants");
                });

            modelBuilder.Entity("FuzzyProject.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ColorantId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<bool>("IsReference")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParametersId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PolymerTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ColorantId");

                    b.HasIndex("ParametersId");

                    b.HasIndex("PolymerTypeId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("FuzzyProject.Models.Parameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("_A")
                        .HasColumnType("REAL");

                    b.Property<double>("_B")
                        .HasColumnType("REAL");

                    b.Property<double>("_L")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("FuzzyProject.Models.PolymerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PolymerTypes");
                });

            modelBuilder.Entity("FuzzyProject.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ColorantId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParametersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ColorantId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("ParametersId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("FuzzyProject.Models.Material", b =>
                {
                    b.HasOne("FuzzyProject.Models.Colorant", "Colorants")
                        .WithMany("Materials")
                        .HasForeignKey("ColorantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuzzyProject.Models.Parameter", "Parameters")
                        .WithMany("Materials")
                        .HasForeignKey("ParametersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuzzyProject.Models.PolymerType", "PolymerTypes")
                        .WithMany("Materials")
                        .HasForeignKey("PolymerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colorants");

                    b.Navigation("Parameters");

                    b.Navigation("PolymerTypes");
                });

            modelBuilder.Entity("FuzzyProject.Models.Report", b =>
                {
                    b.HasOne("FuzzyProject.Models.Account", null)
                        .WithMany("Reports")
                        .HasForeignKey("AccountId");

                    b.HasOne("FuzzyProject.Models.Colorant", "Colorants")
                        .WithMany()
                        .HasForeignKey("ColorantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuzzyProject.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuzzyProject.Models.Parameter", "Parameters")
                        .WithMany()
                        .HasForeignKey("ParametersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colorants");

                    b.Navigation("Material");

                    b.Navigation("Parameters");
                });

            modelBuilder.Entity("FuzzyProject.Models.Account", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("FuzzyProject.Models.Colorant", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("FuzzyProject.Models.Parameter", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("FuzzyProject.Models.PolymerType", b =>
                {
                    b.Navigation("Materials");
                });
#pragma warning restore 612, 618
        }
    }
}