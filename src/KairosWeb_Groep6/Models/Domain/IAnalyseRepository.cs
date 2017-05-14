using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IAnalyseRepository
    {
        IEnumerable<Analyse> GetAnalysesNietInArchief();
        IEnumerable<Analyse> GetAnalysesUitArchief();
        IEnumerable<Analyse> GetAnalyses(Jobcoach jobcoach, int Index, int aantal);     
        void SetAnalysesJobcoach(Jobcoach jobcoach, bool archief);
        Analyse GetById(int id);
        void Add(Analyse analyse);
        void Remove(Analyse analyse);
        void Save();
    }
}
