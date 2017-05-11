namespace KairosWeb_Groep6.Models.Domain
{
    public interface IExceptionLogRepository
    {
        void Add(ExceptionLog e);
        void Remove(ExceptionLog e);
        void Save();
    }
}
