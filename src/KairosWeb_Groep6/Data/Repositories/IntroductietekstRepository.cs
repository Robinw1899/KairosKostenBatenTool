using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class IntroductietekstRepository : IIntroductietekstRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly DbSet<Introductietekst> _teksten;

        public IntroductietekstRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _teksten = dbContext.Introteksten;
        }

        public Introductietekst GetIntroductietekst()
        {
            Introductietekst tekst = _teksten.Include(t => t.Paragrafen).FirstOrDefault();
            
            if (tekst == null)
            {
                return new Introductietekst();
            }

            // als er een tekst is, de paragrafen terug in volgorde zetten
            tekst.Paragrafen = tekst.Paragrafen.OrderBy(p => p.Volgnummer).ToList();

            return tekst;
        }

        public void Add(Introductietekst tekst)
        {
            _teksten.Add(tekst);
        }

        public void Remove(Introductietekst tekst)
        {
            _teksten.Remove(tekst);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
