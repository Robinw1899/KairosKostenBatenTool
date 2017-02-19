using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.Data.MySqlClient;
using System.Data;

namespace KairosWeb_Groep6.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Jobcoach> Jobcoaches { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=12345;database=test;";
            MySqlConnection conn = new MySqlConnection(myConnectionString);
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySqlException ex)
            {
                
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Jobcoach>(MapJobcoach);
            
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        private void MapJobcoach(EntityTypeBuilder<Jobcoach> j)
        {
            j.ToTable("Jobcoach");
            j.HasKey(t => t.JobcoachId);

            j.Property(t => t.Naam).HasMaxLength(50).IsRequired();
            j.Property(t => t.Emailadres).HasMaxLength(50).IsRequired();
            j.Property(t => t.Voornaam).HasMaxLength(50).IsRequired();

            j.HasOne(t => t.Organisatie).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
            j.HasMany(t => t.analyses).WithOne().IsRequired().OnDelete(DeleteBehavior.Restrict);

        }
    }
}
