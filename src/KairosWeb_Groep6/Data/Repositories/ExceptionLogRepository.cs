using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        #region Properties
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ExceptionLog> _exceptions;
        #endregion

        #region Constructors
        public ExceptionLogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public void Add(ExceptionLog e)
        {
            _exceptions.Add(e);
        }

        public void Remove(ExceptionLog e)
        {
            _exceptions.Remove(e);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
