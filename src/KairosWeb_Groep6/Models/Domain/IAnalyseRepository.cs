using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IAnalyseRepository
    {
        IEnumerable<Analyse> GetAnalysesNietInArchief();
        IEnumerable<Analyse> GetAnalysesUitArchief();
        IEnumerable<Analyse> GetAnalyses(bool archief, int beginIndex, int eindIndex);
        Analyse GetById(int id);
        void Add(Analyse analyse);
        void Remove(Analyse analyse);
        void Save();
    }
}
