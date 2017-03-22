
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
        public DbSet<Jobcoach> Gebruikers { get; set; }

        public DbSet<Departement> Departementen { get; set; }

        public DbSet<Analyse> Analyses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Jobcoach>(MapGebruiker);
            builder.Entity<Organisatie>(MapOrganisatie);
            builder.Ignore<DomeinController>();
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

            a.HasOne(t => t.Departement)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            a.HasMany(t => t.MedewerkersZelfdeNiveauBaat)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.MedewerkersHogerNiveauBaat)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            a.HasMany(t => t.UitzendKrachtBesparingen)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void MapOrganisatie(EntityTypeBuilder<Organisatie> o)
        {
            o.ToTable("Organisatie");

            o.HasKey(t => t.OrganisatieId);

            o.Property(t => t.Naam)
                .HasMaxLength(50)
                .IsRequired();

            o.Property(t => t.Straat)
                .HasMaxLength(50)
                .IsRequired();

            o.Property(t => t.Gemeente)
                .HasMaxLength(50)
                .IsRequired();

            o.Property(t => t.Postcode)
                .IsRequired();

            o.Property(t => t.Nummer)
                .IsRequired();
        }

        private static void MapGebruiker(EntityTypeBuilder<Jobcoach> j)
        {
            j.ToTable("Jobcoach");

            j.HasKey(t => t.JobcoachId);

            j.Property(t => t.Naam)
                .HasMaxLength(50)
                .IsRequired();

            j.Property(t => t.Voornaam)
                .HasMaxLength(50)
                .IsRequired();

            j.Property(t => t.Emailadres)
                .HasMaxLength(100)
                .IsRequired();

            j.Property(t => t.AlAangemeld)
                .IsRequired();

            j.Property(t => t.Wachtwoord)
                .IsRequired()
                .HasMaxLength(16);

            j.HasOne(t => t.Organisatie)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            j.Ignore(t => t.Analyses);
        }

        private static void MapWerkgever(EntityTypeBuilder<Werkgever> w)
        {
            w.ToTable("Werkgever");

            w.HasKey(t => t.WerkgeverId);

            w.Property(t => t.Naam)
                .HasMaxLength(50)
                .IsRequired();

            w.Property(t => t.Straat)
                .HasMaxLength(50);

            w.Property(t => t.Nummer)
                .HasMaxLength(5);

            w.Property(t => t.Postcode)
                .IsRequired();

            w.Property(t => t.Gemeente)
                .IsRequired();          
            //w.Property(t => Werkgever.AantalWerkuren)
            //    .IsRequired();

            //w.Property(t => Werkgever.PatronaleBijdrage);
        }
    }
}
