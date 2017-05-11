using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        #region Properties
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ExceptionLog> _exceptionLogs;
        #endregion

        #region Constructors
        public ExceptionLogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _exceptionLogs = dbContext.ExceptionLogs;
        }
        #endregion

        #region Methods
        public void Add(ExceptionLog e)
        {
            _exceptionLogs.Add(e);
        }

        public void Remove(ExceptionLog e)
        {
            _exceptionLogs.Remove(e);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
