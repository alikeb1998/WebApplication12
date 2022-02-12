﻿using Microsoft.EntityFrameworkCore;
using Iz.Online.Entities;

namespace Iz.Online.DataAccess
{
    public class OnlineBackendDbContext : DbContext
    {

        public OnlineBackendDbContext(DbContextOptions<OnlineBackendDbContext> options) : base(options)
        {

        }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerHubs> CustomerHubs { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<WatchList> WathLists { get; set; }
        public DbSet<InstrumentSubSector> InstrumentSubSectors { get; set; }
        public DbSet<InstrumentSector> InstrumentSectors { get; set; }
        public DbSet<InstrumentBourse> InstrumentBourses { get; set; }
        public DbSet<ExceptionsLog> Exceptions { get; set; }
        public DbSet<AppConfigs> AppConfigs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WatchListsInstruments>()
                .HasKey(wi => new { wi.InstrumentId, wi.WatchListId });

            modelBuilder.Entity<WatchListsInstruments>()
                .HasOne(bc => bc.Instrument)
                .WithMany(b => b.WatchListsInstruments)
                .HasForeignKey(bc => bc.InstrumentId);

            modelBuilder.Entity<WatchListsInstruments>()
                .HasOne(bc => bc.WatchList)
                .WithMany(c => c.WatchListsInstruments)
                .HasForeignKey(bc => bc.WatchListId);

            //modelBuilder.Entity<Instrument>()
            //    .HasOne(b=>b.Bourse)
            //    .WithOne(i=>i.Id).

        }

    }
}