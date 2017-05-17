using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IAnalyseRepository
    {
        IEnumerable<Analyse> GetAnalysesNietInArchief();
        IEnumerable<Analyse> GetAnalysesUitArchief();
        IEnumerable<Analyse> GetAnalyses(Jobcoach jobcoach, int Index, int aantal);
        IEnumerable<Analyse> GetAllZonderIncludes();
        void SetAnalysesJobcoach(Jobcoach jobcoach, bool archief);
        Analyse GetById(int id, Soort soort); // haalt enkel de nodige gegevens op
        Analyse GetById(int id); // met werknemer en departementen erbij -> deze methode wordt gebruikt voor de AnalyseSessionfilter
        Analyse GetByIdAll(int id);
      //  Analyse GetByIdNoInclude(int id);
        void Add(Analyse analyse);
        void Remove(Analyse analyse);
        void Save();

    }
}
