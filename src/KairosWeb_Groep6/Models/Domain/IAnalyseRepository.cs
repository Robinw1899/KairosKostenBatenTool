using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IAnalyseRepository
    {
        IEnumerable<Analyse> GetAnalysesNietInArchief();
        IEnumerable<Analyse> GetAnalysesUitArchief();
        Analyse GetById(int id);
        void Add(Analyse analyse);
        void Remove(Analyse analyse);
        void Save();
        void Save(int id);
    }
}
