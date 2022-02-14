﻿// <auto-generated />
using System;
using Iz.Online.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    [DbContext(typeof(OnlineBackendDbContext))]
    [Migration("20220214052751_commissionratee")]
    partial class commissionratee
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Iz.Online.Entities.AppConfigs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppConfigs");
                });

            modelBuilder.Entity("Iz.Online.Entities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Iz.Online.Entities.CustomerHubs", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HubId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerHubs");
                });

            modelBuilder.Entity("Iz.Online.Entities.ExceptionsLog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ExceptionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StackTrace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Exceptions");
                });

            modelBuilder.Entity("Iz.Online.Entities.Instrument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BaseVolume")
                        .HasColumnType("bigint");

                    b.Property<int?>("BourseId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("InstrumentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Isin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SectorId")
                        .HasColumnType("int");

                    b.Property<int?>("SubSectorId")
                        .HasColumnType("int");

                    b.Property<string>("SymbolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Tick")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BourseId");

                    b.HasIndex("SectorId");

                    b.HasIndex("SubSectorId");

                    b.ToTable("Instruments", "Symbols");
                });

            modelBuilder.Entity("Iz.Online.Entities.InstrumentBourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BourseId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("borse")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("InstrumentBourse", "Symbols");
                });

            modelBuilder.Entity("Iz.Online.Entities.InstrumentSector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SectorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("InstrumentSectors", "Symbols");
                });

            modelBuilder.Entity("Iz.Online.Entities.InstrumentSubSector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubSectorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("InstrumentSubSectors", "Symbols");
                });

            modelBuilder.Entity("Iz.Online.Entities.Orders", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateOrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisclosedQuantity")
                        .HasColumnType("int");

                    b.Property<long>("InstrumentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Isr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OmsPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("OmsQty")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("OmsResponseDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<int>("OrderSide")
                        .HasColumnType("int");

                    b.Property<int>("OrderType")
                        .HasColumnType("int");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RegisterOrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidityDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ValidityType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Iz.Online.Entities.TokenStore", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Token");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("Iz.Online.Entities.WatchList", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WatchListName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("WathLists");
                });

            modelBuilder.Entity("Iz.Online.Entities.WatchListsInstruments", b =>
                {
                    b.Property<long>("InstrumentId")
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    b.Property<string>("WatchListId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(2);

                    b.HasKey("InstrumentId", "WatchListId");

                    b.HasIndex("WatchListId");

                    b.ToTable("WatchListsInstruments");
                });

            modelBuilder.Entity("Iz.Online.Entities.CustomerHubs", b =>
                {
                    b.HasOne("Iz.Online.Entities.Customer", null)
                        .WithMany("CustomersHubs")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Iz.Online.Entities.Instrument", b =>
                {
                    b.HasOne("Iz.Online.Entities.InstrumentBourse", "Bourse")
                        .WithMany()
                        .HasForeignKey("BourseId");

                    b.HasOne("Iz.Online.Entities.InstrumentSector", "Sector")
                        .WithMany()
                        .HasForeignKey("SectorId");

                    b.HasOne("Iz.Online.Entities.InstrumentSubSector", "SubSector")
                        .WithMany()
                        .HasForeignKey("SubSectorId");

                    b.Navigation("Bourse");

                    b.Navigation("Sector");

                    b.Navigation("SubSector");
                });

            modelBuilder.Entity("Iz.Online.Entities.WatchList", b =>
                {
                    b.HasOne("Iz.Online.Entities.Customer", "Customer")
                        .WithMany("WathLists")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Iz.Online.Entities.WatchListsInstruments", b =>
                {
                    b.HasOne("Iz.Online.Entities.Instrument", "Instrument")
                        .WithMany("WatchListsInstruments")
                        .HasForeignKey("InstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Iz.Online.Entities.WatchList", "WatchList")
                        .WithMany("WatchListsInstruments")
                        .HasForeignKey("WatchListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instrument");

                    b.Navigation("WatchList");
                });

            modelBuilder.Entity("Iz.Online.Entities.Customer", b =>
                {
                    b.Navigation("CustomersHubs");

                    b.Navigation("WathLists");
                });

            modelBuilder.Entity("Iz.Online.Entities.Instrument", b =>
                {
                    b.Navigation("WatchListsInstruments");
                });

            modelBuilder.Entity("Iz.Online.Entities.WatchList", b =>
                {
                    b.Navigation("WatchListsInstruments");
                });
#pragma warning restore 612, 618
        }
    }
}
