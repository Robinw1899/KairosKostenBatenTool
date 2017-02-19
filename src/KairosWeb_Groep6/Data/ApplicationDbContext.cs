using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public Microsoft.EntityFrameworkCore.DbSet<Jobcoach> Jobcoaches { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<Jobcoach>(MapJobcoach);
        }

        //private static void MapJobcoach(EntityTypeBuilder<Jobcoach> j)
        //{
        //    j.ToTable("Jobcoach");
        //    j.HasKey(t => t.JobcoachId);

        //    j.Property(t => t.Naam).HasMaxLength(50).IsRequired();
        //    j.Property(t => t.Emailadres).HasMaxLength(50).IsRequired();
        //    j.Property(t => t.Voornaam).HasMaxLength(50).IsRequired();

        //    j.HasOne(t => t.Organisatie).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
        //    j.HasMany(t => t.analyses).WithOne().IsRequired().OnDelete(DeleteBehavior.Restrict);

        //}
    }
}
