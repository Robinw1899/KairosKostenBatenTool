﻿using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class JobcoachRepository : IJobcoachRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Jobcoach> _jobcoaches;

        public JobcoachRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
            _jobcoaches = _context.Jobcoaches;
        }
        public void AddJobcoach(Jobcoach jobcoach)
        {
            _jobcoaches.Add(jobcoach);
        }

        public IEnumerable<Jobcoach> GetAll()
        {
            return _jobcoaches.ToList(); //Hier moeten nog meer includes bij naarmate we andere kolommen hebben in de DB
        }

        public Jobcoach GetById(int id)
        {
            return _jobcoaches.SingleOrDefault(b => b.JobcoachId == id);//include
        }

        public void RemoveJobcoach(Jobcoach jobcoach)
        {
            _jobcoaches.Remove(jobcoach);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
