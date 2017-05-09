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
           
        }

        public AnalyseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _analyses = dbContext.Analyses;
          
        }

        public IEnumerable<Analyse> GetAllZonderIncludes()
        {
            return _analyses
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Analyse> GetAnalysesNietInArchief()
        {          
            return _analyses
                .Include(a => a.ContactPersooon)
                .Include(a => a.Departement)
                    .ThenInclude(d => d.Werkgever)
                    .ThenInclude(w => w.ContactPersonen)
                .Include(a => a.BegeleidingsKosten)
                .Include(a => a.EnclaveKosten)
                .Include(a => a.ExterneInkopen)
                .Include(a => a.ExtraBesparingen)
                .Include(a => a.ExtraOmzet)
                .Include(a => a.ExtraProductiviteit)
                .Include(a => a.GereedschapsKosten)
                .Include(a => a.PersoneelsKosten)
                .Include(a => a.Loonkosten)
                    .ThenInclude(l => l.Doelgroep)
                .Include(a => a.ExtraKosten)
                .Include(a => a.MedewerkersHogerNiveauBaat)
                .Include(a => a.MedewerkersZelfdeNiveauBaat)
                .Include(a => a.OpleidingsKosten)
                .Include(a => a.OverurenBesparing)
                .Include(a => a.Subsidie)
                .Include(a => a.LogistiekeBesparing)
                .Include(a => a.UitzendKrachtBesparingen)
                .Include(a => a.VoorbereidingsKosten)
                .Where(a => a.InArchief == false )
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Analyse> GetAnalysesUitArchief()
        {
            return _analyses
                .Include(a => a.ContactPersooon)
                .Include(a => a.Departement)
                    .ThenInclude(d => d.Werkgever)
                .Include(a => a.BegeleidingsKosten)
                .Include(a => a.EnclaveKosten)
                .Include(a => a.ExterneInkopen)
                .Include(a => a.ExtraBesparingen)
                .Include(a => a.ExtraOmzet)
                .Include(a => a.ExtraProductiviteit)
                .Include(a => a.GereedschapsKosten)
                .Include(a => a.PersoneelsKosten)
                .Include(a => a.Loonkosten)
                    .ThenInclude(l => l.Doelgroep)
                .Include(a => a.ExtraKosten)
                .Include(a => a.MedewerkersHogerNiveauBaat)
                .Include(a => a.MedewerkersZelfdeNiveauBaat)
                .Include(a => a.OpleidingsKosten)
                .Include(a => a.OverurenBesparing)
                .Include(a => a.Subsidie)
                .Include(a => a.LogistiekeBesparing)
                .Include(a => a.UitzendKrachtBesparingen)
                .Include(a => a.VoorbereidingsKosten)
                .Where(a => a.InArchief) // == true mag weggelaten worden
                .AsNoTracking()
                .ToList();
        }

        public Analyse GetById(int id)
        {
            return _analyses
                .Include(a => a.ContactPersooon)
                .Include(a => a.Departement)
                    .ThenInclude(d => d.Werkgever)
                .Include(a => a.BegeleidingsKosten)
                .Include(a => a.EnclaveKosten)
                .Include(a => a.ExterneInkopen)
                .Include(a => a.ExtraBesparingen)
                .Include(a => a.ExtraOmzet)
                .Include(a => a.ExtraProductiviteit)
                .Include(a => a.GereedschapsKosten)
                .Include(a => a.PersoneelsKosten)
                .Include(a => a.Loonkosten)
                    .ThenInclude(l => l.Doelgroep)
                .Include(a => a.ExtraKosten)
                .Include(a => a.MedewerkersHogerNiveauBaat)
                .Include(a => a.MedewerkersZelfdeNiveauBaat)
                .Include(a => a.OpleidingsKosten)
                .Include(a => a.OverurenBesparing)
                .Include(a => a.Subsidie)
                .Include(a => a.LogistiekeBesparing)
                .Include(a => a.UitzendKrachtBesparingen)
                .Include(a => a.VoorbereidingsKosten)
                .SingleOrDefault(a => a.AnalyseId == id);
        }
        public IEnumerable<Analyse> GetAnalyses(Jobcoach jobcoach, int index, int aantal)
        {
            List<Analyse> legeAnalyses = jobcoach
                .Analyses
                .Skip(index)
                .Take(aantal)
                .ToList();

            return legeAnalyses;
        }

        public void SetAnalysesJobcoach(Jobcoach jobcoach, bool archief)
        {
            jobcoach.Analyses = jobcoach
                        .Analyses
                        .Where(j => j.InArchief == archief)
                        .OrderByDescending(t => t.DatumLaatsteAanpassing)
                        .ToList();
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
        public void Save(int id)
        {
            _dbContext.SaveChangesAsync();
        }

       
    }
}
