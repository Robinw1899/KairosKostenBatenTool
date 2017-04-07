using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Introductietekst
    {
        #region Properties
        public int IntroductietekstId { get; set; }

        public string Titel { get; set; }

        // dit is de vraag die getoond wordt om de tekst terug te laten verschijnen:
        public string Vraag { get; set; }

        public IList<Paragraaf> Paragrafen { get; private set; } = new List<Paragraaf>();
        #endregion
    }
}
