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
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1");

            modelBuilder.Entity("AssetWeights", b =>
                {
                    b.Property<long>("AssetId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("WeightsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AssetId", "WeightsId");

                    b.HasIndex("WeightsId");

                    b.ToTable("WeightsAssets", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Asset", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DataSetId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Usage")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DataSetId");

                    b.ToTable("Assets", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Game", b =>
                {
                    b.Property<long>("Id")
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
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("Images", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Color")
                        .HasColumnType("INTEGER");

                    b.Property<long>("DataSetId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DataSetId");

                    b.ToTable("ItemClasses", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.DataSet", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ushort>("Resolution")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("DataSets");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorItem", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<long>("AssetId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ItemClassId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("ItemClassId");

                    b.ToTable("DetectorItems", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Profile", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("DetectionThreshold")
                        .HasColumnType("REAL");

                    b.Property<float>("MouseSensitivity")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ushort>("PostProcessDelay")
                        .HasColumnType("INTEGER");

                    b.Property<long>("WeightsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("WeightsId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ProfileItemClass", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActivationCondition")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Index")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ItemClassId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ItemClassId");

                    b.HasIndex("ProfileId", "Index")
                        .IsUnique();

                    b.HasIndex("ProfileId", "ItemClassId")
                        .IsUnique();

                    b.ToTable("ProfileItemClasses", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Screenshot", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("LibraryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.ToTable("Screenshots", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ScreenshotsLibrary", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasAnyScreenshots")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("MaxQuantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ScreenshotsLibraries", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Weights", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<float>("BoundingLoss")
                        .HasColumnType("REAL");

                    b.Property<float>("ClassificationLoss")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<float>("DeformationLoss")
                        .HasColumnType("REAL");

                    b.Property<uint>("Epoch")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LibraryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.ToTable("Weights");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.WeightsData", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("WeightsData");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.WeightsLibrary", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("WeightsLibraries", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ONNXData", b =>
                {
                    b.HasBaseType("SightKeeper.Domain.Model.WeightsData");

                    b.Property<long>("WeightsId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("WeightsId")
                        .IsUnique();

                    b.ToTable("ONNXData", (string)null);
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.PTData", b =>
                {
                    b.HasBaseType("SightKeeper.Domain.Model.WeightsData");

                    b.Property<long>("WeightsId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("WeightsId")
                        .IsUnique();

                    b.ToTable("PTData", (string)null);
                });

            modelBuilder.Entity("AssetWeights", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.Asset", null)
                        .WithMany()
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Weights", null)
                        .WithMany()
                        .HasForeignKey("WeightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Asset", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithMany("Assets")
                        .HasForeignKey("DataSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Screenshot", "Screenshot")
                        .WithOne("Asset")
                        .HasForeignKey("SightKeeper.Domain.Model.Common.Asset", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");

                    b.Navigation("Screenshot");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Image", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Screenshot", null)
                        .WithOne("Image")
                        .HasForeignKey("SightKeeper.Domain.Model.Common.Image", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithMany("ItemClasses")
                        .HasForeignKey("DataSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.DataSet", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Detector.DetectorItem", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.Asset", "Asset")
                        .WithMany("Items")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Common.ItemClass", "ItemClass")
                        .WithMany("Items")
                        .HasForeignKey("ItemClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("SightKeeper.Domain.Model.Detector.Bounding", "Bounding", b1 =>
                        {
                            b1.Property<long>("DetectorItemId")
                                .HasColumnType("INTEGER");

                            b1.Property<double>("Bottom")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingBottom");

                            b1.Property<double>("Left")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingLeft");

                            b1.Property<double>("Right")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingRight");

                            b1.Property<double>("Top")
                                .HasColumnType("REAL")
                                .HasColumnName("BoundingTop");

                            b1.HasKey("DetectorItemId");

                            b1.ToTable("DetectorItems");

                            b1.WithOwner()
                                .HasForeignKey("DetectorItemId");
                        });

                    b.Navigation("Asset");

                    b.Navigation("Bounding")
                        .IsRequired();

                    b.Navigation("ItemClass");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Profile", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Weights", "Weights")
                        .WithMany()
                        .HasForeignKey("WeightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("SightKeeper.Domain.Model.PreemptionSettings", "PreemptionSettings", b1 =>
                        {
                            b1.Property<long>("ProfileId")
                                .HasColumnType("INTEGER");

                            b1.Property<float>("HorizontalFactor")
                                .HasColumnType("REAL")
                                .HasColumnName("PreemptionHorizontalFactor");

                            b1.Property<float>("VerticalFactor")
                                .HasColumnType("REAL")
                                .HasColumnName("PreemptionVerticalFactor");

                            b1.HasKey("ProfileId");

                            b1.ToTable("Profiles");

                            b1.WithOwner()
                                .HasForeignKey("ProfileId");

                            b1.OwnsOne("SightKeeper.Domain.Model.PreemptionStabilizationSettings", "StabilizationSettings", b2 =>
                                {
                                    b2.Property<long>("PreemptionSettingsProfileId")
                                        .HasColumnType("INTEGER");

                                    b2.Property<byte>("BufferSize")
                                        .HasColumnType("INTEGER")
                                        .HasColumnName("PreemptionStabilizationBufferSize");

                                    b2.Property<int>("Method")
                                        .HasColumnType("INTEGER")
                                        .HasColumnName("PreemptionStabilizationMethod");

                                    b2.HasKey("PreemptionSettingsProfileId");

                                    b2.ToTable("Profiles");

                                    b2.WithOwner()
                                        .HasForeignKey("PreemptionSettingsProfileId");
                                });

                            b1.Navigation("StabilizationSettings")
                                .IsRequired();
                        });

                    b.Navigation("PreemptionSettings");

                    b.Navigation("Weights");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ProfileItemClass", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.Common.ItemClass", "ItemClass")
                        .WithMany()
                        .HasForeignKey("ItemClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Profile", null)
                        .WithMany("ItemClasses")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemClass");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Screenshot", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.ScreenshotsLibrary", "Library")
                        .WithMany("Screenshots")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ScreenshotsLibrary", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithOne("ScreenshotsLibrary")
                        .HasForeignKey("SightKeeper.Domain.Model.ScreenshotsLibrary", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Weights", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.WeightsLibrary", "Library")
                        .WithMany("Weights")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.WeightsLibrary", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.DataSet", "DataSet")
                        .WithOne("WeightsLibrary")
                        .HasForeignKey("SightKeeper.Domain.Model.WeightsLibrary", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataSet");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ONNXData", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.WeightsData", null)
                        .WithOne()
                        .HasForeignKey("SightKeeper.Domain.Model.ONNXData", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Weights", null)
                        .WithOne("ONNXData")
                        .HasForeignKey("SightKeeper.Domain.Model.ONNXData", "WeightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.PTData", b =>
                {
                    b.HasOne("SightKeeper.Domain.Model.WeightsData", null)
                        .WithOne()
                        .HasForeignKey("SightKeeper.Domain.Model.PTData", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SightKeeper.Domain.Model.Weights", null)
                        .WithOne("PTData")
                        .HasForeignKey("SightKeeper.Domain.Model.PTData", "WeightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.Asset", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Common.ItemClass", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.DataSet", b =>
                {
                    b.Navigation("Assets");

                    b.Navigation("ItemClasses");

                    b.Navigation("ScreenshotsLibrary")
                        .IsRequired();

                    b.Navigation("WeightsLibrary")
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Profile", b =>
                {
                    b.Navigation("ItemClasses");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Screenshot", b =>
                {
                    b.Navigation("Asset");

                    b.Navigation("Image")
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.ScreenshotsLibrary", b =>
                {
                    b.Navigation("Screenshots");
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.Weights", b =>
                {
                    b.Navigation("ONNXData")
                        .IsRequired();

                    b.Navigation("PTData")
                        .IsRequired();
                });

            modelBuilder.Entity("SightKeeper.Domain.Model.WeightsLibrary", b =>
                {
                    b.Navigation("Weights");
                });
#pragma warning restore 612, 618
        }
    }
}
