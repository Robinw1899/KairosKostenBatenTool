﻿using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class DoelgroepRepository : IDoelgroepRepository
    {
        #region Properties
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Doelgroep> doelgroepen;
        #endregion

        #region Constructors
        public DoelgroepRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            doelgroepen = dbContext.Doelgroepen;
        }
        #endregion

        #region Methods
        public Doelgroep GetById(int id)
        {
            return doelgroepen
                .FirstOrDefault(t => t.DoelgroepId == id);
        }

        public IEnumerable<Doelgroep> GetAll()
        {
            return doelgroepen
                .Where(d => d.Verwijderd == false) // de doelgroepen die niet verwijderd zijn
                .AsNoTracking()
                .ToList();
        }

        public void Add(Doelgroep doelgroep)
        {
            doelgroepen.Add(doelgroep);
        }

        public void Remove(Doelgroep doelgroep)
        {
            doelgroepen.Remove(doelgroep);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
