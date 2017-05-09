using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class OrganisatieRepository : IOrganisatieRepository
    {
        #region Properties
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Organisatie> _organisaties;
        #endregion

        #region Constructors
        public OrganisatieRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _organisaties = dbContext.Organisaties;
        }
        #endregion

        #region Methods
        public IEnumerable<Organisatie> GetAllByNaam(string naam)
        {
            return _organisaties
                .Where(o => o.Naam.Equals(naam, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void Add(Organisatie organisatie)
        {
            _organisaties.Add(organisatie);
        }

        public void Remove(Organisatie organisatie)
        {
            _organisaties.Remove(organisatie);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
