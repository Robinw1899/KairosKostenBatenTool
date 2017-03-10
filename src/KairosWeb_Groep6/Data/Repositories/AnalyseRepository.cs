using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class AnalyseRepository : IAnalyseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Analyse> _analyses;

        public AnalyseRepository()
        {
            // enkel voor Mock
        }

        public AnalyseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _analyses = dbContext.Analyses;
        }

        public IEnumerable<Analyse> GetAll()
        {
            return _analyses
                .Include(a => a.Werkgever)
                .Include(a => a.BegeleidingsKosten)
                .Include(a => a.EnclaveKosten)
                .Include(a => a.ExterneInkopen)
                .Include(a => a.ExtraBesparingen)
                .Include(a => a.ExtraOmzet)
                .Include(a => a.ExtraProductiviteit)
                .Include(a => a.GereedschapsKosten)
                .Include(a => a.InfrastructuurKosten)
                .Include(a => a.Loonkosten)
                .Include(a => a.MedewerkersHogerNiveauBaat)
                .Include(a => a.MedewerkersZelfdeNiveauBaat)
                .Include(a => a.OpleidingsKosten)
                .Include(a => a.OverurenBesparing)
                .Include(a => a.Subsidies)
                .Include(a => a.UitzendKrachtBesparingen)
                .Include(a => a.VoorbereidingsKosten)
                .AsNoTracking()
                .ToList();
        }

        public Analyse GetById(int id)
        {
            return _analyses
                .Include(a => a.Werkgever)
                .Include(a => a.BegeleidingsKosten)
                .Include(a => a.EnclaveKosten)
                .Include(a => a.ExterneInkopen)
                .Include(a => a.ExtraBesparingen)
                .Include(a => a.ExtraOmzet)
                .Include(a => a.ExtraProductiviteit)
                .Include(a => a.GereedschapsKosten)
                .Include(a => a.InfrastructuurKosten)
                .Include(a => a.Loonkosten)
                .Include(a => a.MedewerkersHogerNiveauBaat)
                .Include(a => a.MedewerkersZelfdeNiveauBaat)
                .Include(a => a.OpleidingsKosten)
                .Include(a => a.OverurenBesparing)
                .Include(a => a.Subsidies)
                .Include(a => a.UitzendKrachtBesparingen)
                .Include(a => a.VoorbereidingsKosten)
                .SingleOrDefault(a => a.AnalyseId == id);
        }

        public void Add(Analyse analyse)
        {
            _analyses.Add(analyse);
        }

        public void Remove(Analyse analyse)
        {
            _analyses.Remove(analyse);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
