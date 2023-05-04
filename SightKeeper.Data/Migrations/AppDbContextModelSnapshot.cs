﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SightKeeper.Data;

#nullable disable

namespace SightKeeper.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("SightKeeper.Domain.Model.Abstract.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ConfigId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConfigId");

                    b.HasIndex("GameId");

                    b.ToTable("Models");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProcessName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ItemClassGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ItemClassGroupId");

                    b.HasIndex("ModelId");

                    b.ToTable("ItemClasses");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClassGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ItemClassesGroups");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ModelConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ModelConfigs");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DetectorModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DetectorModelId");

                    b.HasIndex("GameId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemClassId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScreenshotId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ItemClassId");

                    b.HasIndex("ScreenshotId");

                    b.ToTable("DetectorItems");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorScreenshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAsset")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("ModelId");

                    b.ToTable("DetectorScreenshots");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorModel", b =>
                {
                    b.HasBaseType("SightKeeper.Domain.Model.Abstract.Model");

                    b.ToTable("DetectorModels");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Abstract.Model", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.ModelConfig", "Config")
                        .WithMany()
                        .HasForeignKey("ConfigId");

                    b.HasOne("SightKeeper.Domain.Model.Common.Game", "Game")
                        .WithMany("Models")
                        .HasForeignKey("GameId");

                    b.OwnsOne("SightKeeper.Domain.Model.Common.Resolution", "Resolution", b1 =>
                        {
                            b1.Property<int>("ModelId")
                                .HasColumnType("INTEGER");

                            b1.Property<ushort>("Height")
                                .HasColumnType("INTEGER");

                            b1.Property<ushort>("Width")
                                .HasColumnType("INTEGER");

                            b1.HasKey("ModelId");

                            b1.ToTable("Models");

                            b1.WithOwner()
                                .HasForeignKey("ModelId");
                        });

                    b.Navigation("Config");

                    b.Navigation("Game");

                    b.Navigation("Resolution")
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.ItemClassGroup", null)
                        .WithMany("ItemClasses")
                        .HasForeignKey("ItemClassGroupId");

                    b.HasOne("SightKeeper.Domain.Model.Abstract.Model", "Model")
                        .WithMany("ItemClasses")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Profile", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Detector.DetectorModel", "DetectorModel")
                        .WithMany()
                        .HasForeignKey("DetectorModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Common.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.Navigation("DetectorModel");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorItem", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.ItemClass", "ItemClass")
                        .WithMany()
                        .HasForeignKey("ItemClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Detector.DetectorScreenshot", "Screenshot")
                        .WithMany("Items")
                        .HasForeignKey("ScreenshotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("SightKeeper.Domain.Model.Detector.BoundingBox", "BoundingBox", b1 =>
                        {
                            b1.Property<int>("DetectorItemId")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("Height")
                                .HasColumnType("REAL");

                            b1.Property<double>("Width")
                                .HasColumnType("REAL");

                            b1.Property<double>("X")
                                .HasColumnType("REAL");

                            b1.Property<double>("Y")
                                .HasColumnType("REAL");

                            b1.HasKey("DetectorItemId");

                            b1.ToTable("DetectorItems");

                            b1.WithOwner()
                                .HasForeignKey("DetectorItemId");
                        });

                    b.Navigation("BoundingBox")
                        .IsRequired();

                    b.Navigation("ItemClass");

                    b.Navigation("Screenshot");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorScreenshot", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Detector.DetectorModel", "Model")
                        .WithMany("DetectorScreenshots")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Model");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorModel", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Abstract.Model", null)
                        .WithOne()
                        .HasForeignKey("SightKeeper.Domain.Model.Detector.DetectorModel", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Abstract.Model", b =>
                {
                    b.Navigation("ItemClasses");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Game", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClassGroup", b =>
                {
                    b.Navigation("ItemClasses");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorScreenshot", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorModel", b =>
                {
                    b.Navigation("DetectorScreenshots");
                });
#pragma warning restore 612, 618
        }
    }
}
