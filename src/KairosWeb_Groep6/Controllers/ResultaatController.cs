using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Controllers
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ResultaatController : Controller
    {
        public IActionResult Index(Analyse analyse)
        {
            ResultaatViewModel model = new ResultaatViewModel();

            if (analyse.Departement != null)
            {
                IDictionary<Soort, double> kostenResultaat = analyse.GeefTotalenKosten();
                IDictionary<Soort, double> batenResultaat = analyse.GeefTotalenBaten();

                double kostenTotaal = kostenResultaat.Sum(t => t.Value);
                double batenTotaal = batenResultaat.Sum(t => t.Value);

                model.Resultaten = kostenResultaat;
                foreach(var rij in batenResultaat)
                {
                    model.Resultaten.Add(rij);
                }

                model.KostenTotaal = kostenTotaal;
                model.BatenTotaal = batenTotaal;
                model.Totaal = kostenTotaal + batenTotaal;
            }
            else
            {
                TempData["error"] =
                        "Opgelet! U heeft nog geen werkgever geselecteerd. Er zal dus nog geen resultaat " +
                        "berekend worden voor deze analyse.";
            }
            
            return View(model);
        }
    }
}
