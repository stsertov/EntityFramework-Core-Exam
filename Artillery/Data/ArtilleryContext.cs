﻿namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() { }

        public ArtilleryContext(DbContextOptions options)
            : base(options) { }


        public DbSet<Country> Countries { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Shell> Shells { get; set; }

        public DbSet<Gun> Guns { get; set; }

        public DbSet<CountryGun> CountriesGuns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>()
                .HasIndex(m => m.ManufacturerName)
                .IsUnique();

            modelBuilder.Entity<CountryGun>()
                .HasKey(k => new { k.CountryId, k.GunId });

            modelBuilder.Entity<CountryGun>(e =>
            {
                e.HasOne("Country")
                .WithMany("CountriesGuns")
                .HasForeignKey("CountryId")
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne("Gun")
                .WithMany("CountriesGuns")
                .HasForeignKey("GunId")
                .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
