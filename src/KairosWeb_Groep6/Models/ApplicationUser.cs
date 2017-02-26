using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KairosWeb_Groep6.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Voornaam { get; set; }
        public string Naam { get; set; }
    }
}
