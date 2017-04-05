
using System;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KairosWeb_Groep6.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Jobcoach> Jobcoaches { get; set; }

        public DbSet<Departement> Departementen { get; set; }

        public DbSet<Analyse> Analyses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Persoon>(MapPersoon);
            builder.Entity<Jobcoach>(MapJobcoach);
            builder.Entity<Organisatie>(MapOrganisatie);
            builder.Entity<Departement>(MapDepartement);
            builder.Entity<Werkgever>(MapWerkgever);
            builder.Entity<Analyse>(MapAnalyse);
        }

        private void MapDepartement(EntityTypeBuilder<Departement> d)
        {
            d.ToTable("Departement");

            d.HasKey(t => t.DepartementId);

            d.Property(t => t.Naam)
                .IsRequired();

            d.HasOne(t => t.Werkgever)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void MapAnalyse(EntityTypeBuilder<Analyse> a)
        {
            a.ToTable("Analyse");

            a.HasKey(t => t.AnalyseId);

            a.Property(t => t.InArchief)
                .HasDefaultValue(false)
                .IsRequired();

            a.HasOne(t => t.Departement)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            // Kosten
            a.HasMany(t => t.Loonkosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.EnclaveKosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.VoorbereidingsKosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.InfrastructuurKosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.GereedschapsKosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.OpleidingsKosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.BegeleidingsKosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.ExtraKosten)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Baten
            a.HasMany(t => t.MedewerkersZelfdeNiveauBaat)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.MedewerkersHogerNiveauBaat)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.UitzendKrachtBesparingen)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.ExterneInkopen)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.ExtraBesparingen)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void MapOrganisatie(EntityTypeBuilder<Organisatie> o)
        {
            o.ToTable("Organisatie");

            o.HasKey(t => t.OrganisatieId);

            o.Property(t => t.Naam)
                .IsRequired();

            o.Property(t => t.Straat)
                .IsRequired();

            o.Property(t => t.Gemeente)
                .IsRequired();

            o.Property(t => t.Postcode)
                .IsRequired();

            o.Property(t => t.Nummer)
                .IsRequired();
        }

        private static void MapPersoon(EntityTypeBuilder<Persoon> p)
        {
            p.ToTable("Persoon");

            p.HasKey(t => t.PersoonId);

            p.Property(t => t.Naam)
                .IsRequired();

            p.Property(t => t.Voornaam)
                .IsRequired();

            p.Property(t => t.Emailadres)
                .IsRequired();

            p.HasDiscriminator<string>("Discriminator")
                .HasValue<Persoon>("Persoon")
                .HasValue<ContactPersoon>("ContactPersoon")
                .HasValue<Jobcoach>("Jobcoach");
        }

        private static void MapJobcoach(EntityTypeBuilder<Jobcoach> j)
        {
            j.ToTable("Jobcoach");

            j.Property(t => t.AlAangemeld)
                .IsRequired();

            j.Property(t => t.Wachtwoord)
                .IsRequired();

            j.HasOne(t => t.Organisatie)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            j.HasMany(t => t.Analyses)
                .WithOne()
                .IsRequired(false);
        }

        private static void MapWerkgever(EntityTypeBuilder<Werkgever> w)
        {
            w.ToTable("Werkgever");

            w.HasKey(t => t.WerkgeverId);

            w.Property(t => t.Naam)
                .IsRequired();

            w.Property(t => t.Straat)
                .IsRequired(false);

            w.Property(t => t.Nummer)
                .IsRequired(false);

            w.Property(t => t.Postcode)
                .IsRequired();

            w.Property(t => t.Gemeente)
                .IsRequired();
        }
    }
}
