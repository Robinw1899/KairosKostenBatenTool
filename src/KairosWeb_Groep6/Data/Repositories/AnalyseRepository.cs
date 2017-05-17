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
                .Where(a => a.Verwijderd == false)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Analyse> GetAnalysesNietInArchief()
        {          
            return _analyses
                .Where(a => a.Verwijderd == false)
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
                .Include(a => a.MedewerkersHogerNiveauBaten)
                .Include(a => a.MedewerkersZelfdeNiveauBaten)
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
        public Analyse GetById(int id, Soort soort)
        {
            IQueryable<Analyse> query = null;


            switch (soort)
            {
                //kosten
                case Soort.Loonkost:
                    query = _analyses
                     .Where(a => a.AnalyseId == id)
                        .Include(a => a.Departement)
                             .ThenInclude(d => d.Werkgever)
                        .Include(a => a.Loonkosten).ThenInclude(l => l.Doelgroep);
                    break;

                case Soort.BegeleidingsKost:
                    query = _analyses
                    .Where(a => a.AnalyseId == id)
                       .Include(a => a.Departement)
                            .ThenInclude(d => d.Werkgever)
                       .Include(a => a.BegeleidingsKosten);
                    break;

                case Soort.EnclaveKost:
                    query = _analyses
                   .Where(a => a.AnalyseId == id)
                      .Include(a => a.Departement)
                           .ThenInclude(d => d.Werkgever)
                      .Include(a => a.EnclaveKosten);
                    break;

                case Soort.ExtraKost:
                    query = _analyses
                    .Where(a => a.AnalyseId == id)
                       .Include(a => a.Departement)
                            .ThenInclude(d => d.Werkgever)
                       .Include(a => a.ExtraKosten);
                    break;

                case Soort.GereedschapsKost:
                    query = _analyses
                    .Where(a => a.AnalyseId == id)
                       .Include(a => a.Departement)
                            .ThenInclude(d => d.Werkgever)
                       .Include(a => a.GereedschapsKosten);
                    break;

                case Soort.PersoneelsKost:
                    query = _analyses
                      .Where(a => a.AnalyseId == id)
                         .Include(a => a.Departement)
                              .ThenInclude(d => d.Werkgever)
                         .Include(a => a.PersoneelsKosten);
                    break;

                case Soort.VoorbereidingsKost:
                    query = _analyses
                       .Where(a => a.AnalyseId == id)
                          .Include(a => a.Departement)
                               .ThenInclude(d => d.Werkgever)
                          .Include(a => a.VoorbereidingsKosten);
                    break;

                case Soort.OpleidingsKost:
                    query = _analyses
                       .Where(a => a.AnalyseId == id)
                          .Include(a => a.Departement)
                               .ThenInclude(d => d.Werkgever)
                          .Include(a => a.OpleidingsKosten);
                    break;

                //baten
                case Soort.ExterneInkoop:
                    query = _analyses
                       .Where(a => a.AnalyseId == id)
                          .Include(a => a.Departement)
                               .ThenInclude(d => d.Werkgever)
                          .Include(a => a.ExterneInkopen);
                    break;

                case Soort.ExtraBesparing:
                    query = _analyses
                      .Where(a => a.AnalyseId == id)
                         .Include(a => a.Departement)
                              .ThenInclude(d => d.Werkgever)
                         .Include(a => a.ExtraBesparingen);
                    break;

                case Soort.ExtraOmzet:
                    query = _analyses
                       .Where(a => a.AnalyseId == id)
                          .Include(a => a.Departement)
                               .ThenInclude(d => d.Werkgever)
                          .Include(a => a.ExtraOmzet);
                    break;

                case Soort.ExtraProductiviteit:
                    query = _analyses
                      .Where(a => a.AnalyseId == id)
                         .Include(a => a.Departement)
                              .ThenInclude(d => d.Werkgever)
                         .Include(a => a.ExtraProductiviteit);
                    break;

                case Soort.LogistiekeBesparing:
                    query = _analyses
                      .Where(a => a.AnalyseId == id)
                         .Include(a => a.Departement)
                              .ThenInclude(d => d.Werkgever)
                         .Include(a => a.LogistiekeBesparing);
                    break;

                case Soort.MedewerkersHogerNiveau:
                    query = _analyses
                     .Where(a => a.AnalyseId == id)
                        .Include(a => a.Departement)
                             .ThenInclude(d => d.Werkgever)
                        .Include(a => a.MedewerkersHogerNiveauBaten);
                    break;

                case Soort.MedewerkersZelfdeNiveau:
                    query = _analyses
                     .Where(a => a.AnalyseId == id)
                        .Include(a => a.Departement)
                             .ThenInclude(d => d.Werkgever)
                        .Include(a => a.MedewerkersZelfdeNiveauBaten);
                    break;

                case Soort.OverurenBesparing:
                    query = _analyses
                      .Where(a => a.AnalyseId == id)
                         .Include(a => a.Departement)
                              .ThenInclude(d => d.Werkgever)
                         .Include(a => a.OverurenBesparing);
                    break;

                case Soort.Subsidie:
                    query = _analyses
                     .Where(a => a.AnalyseId == id)
                        .Include(a => a.Departement)
                             .ThenInclude(d => d.Werkgever)
                        .Include(a => a.Subsidie);
                    break;

                case Soort.UitzendkrachtBesparing:
                    query = _analyses
                      .Where(a => a.AnalyseId == id)
                         .Include(a => a.Departement)
                              .ThenInclude(d => d.Werkgever)
                         .Include(a => a.UitzendKrachtBesparingen);
                    break;

            }

            // execute
            return query.Select(a => a).FirstOrDefault();

        }
        public Analyse GetById(int id)
        {
            return _analyses
                .Where(a => a.AnalyseId == id)
                .Include(a => a.Departement)
                .ThenInclude(d => d.Werkgever)
                .SingleOrDefault();
        }


        public IEnumerable<Analyse> GetAnalysesUitArchief()
        {
            return _analyses
                .Where(a => a.Verwijderd == false)
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
                .Include(a => a.MedewerkersHogerNiveauBaten)
                .Include(a => a.MedewerkersZelfdeNiveauBaten)
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

        public Analyse GetByIdAll(int id)
        {
            return _analyses
                .Where(a => a.Verwijderd == false)
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
                .Include(a => a.MedewerkersHogerNiveauBaten)
                .Include(a => a.MedewerkersZelfdeNiveauBaten)
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
            List<Analyse> analyses = jobcoach
                .Analyses
                .Where(a => a.Verwijderd == false)
                .Skip(index)
                .Take(aantal)
                .ToList();

            return analyses;
        }


        public void SetAnalysesJobcoach(Jobcoach jobcoach, bool archief)
        {
            jobcoach.Analyses = jobcoach
                        .Analyses
                        .Where(a => a.Verwijderd == false)
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
