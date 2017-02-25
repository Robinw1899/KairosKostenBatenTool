using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class JobcoachRepository : IJobcoachRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Jobcoach> _jobcoaches;

        public JobcoachRepository(ApplicationDbContext context)
        {
            _dbContext = context;
            _jobcoaches = context.Jobcoaches;
        }
        public void AddJobcoach(Jobcoach jobcoach)
        {
            _jobcoaches.Add(jobcoach);
        }

        public IEnumerable<Jobcoach> GetAll()
        {
            return _jobcoaches.Include(j=>j.Organisatie).ToList(); //Hier moeten nog meer includes bij naarmate we andere kolommen hebben in de DB
        }

        public Jobcoach GetById(int id)
        {
            return _jobcoaches.Include(j=>j.Organisatie).SingleOrDefault(b => b.JobcoachId == id);//include
        }

        public Jobcoach GetByEmail(string email)
        {
            return _jobcoaches.Include(j=>j.Organisatie).SingleOrDefault(b => b.Emailadres == email);//include
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
