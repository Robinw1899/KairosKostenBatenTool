using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KairosWeb_Groep6.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gebruiker> Gebruikers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Gebruiker>(MapGebruiker);
            builder.Entity<Organisatie>(MapOrganisatie);
            builder.Ignore<Kost>();
            builder.Ignore<Baat>();
            builder.Ignore<DomeinController>();
            builder.Ignore<Analyse>();
            builder.Ignore<Functie>();
            builder.Ignore<Werkgever>();
        }

        private void MapOrganisatie(EntityTypeBuilder<Organisatie> o)
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

        private void MapGebruiker(EntityTypeBuilder<Gebruiker> g)
        {
            g.ToTable("Gebruiker");

            g.HasKey(t => t.GebruikerId);

            g.Property(t => t.Naam)
                .HasMaxLength(50)
                .IsRequired();

            g.Property(t => t.Voornaam)
                .HasMaxLength(50)
                .IsRequired();

            g.Property(t => t.Emailadres)
                .HasMaxLength(100)
                .IsRequired();

            g.Property(t => t.IsAdmin)
                .IsRequired();

            g.Property(t => t.AlAangemeld)
                .IsRequired();

            g.HasOne(t => t.Organisatie)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            g.Ignore(t => t.Analyses);
        }
    }
}
