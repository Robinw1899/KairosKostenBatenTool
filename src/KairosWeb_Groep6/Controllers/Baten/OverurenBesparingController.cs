using System;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [AutoValidateAntiforgeryToken]
    public class OverurenBesparingController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public OverurenBesparingController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            if (analyse.Klaar)
            {
                TempData["error"] = Meldingen.AnalyseKlaar;
                return RedirectToAction("Index", "Resultaat");
            }

            OverurenBesparingViewModel model = MaakModel(analyse);

            return View(model);
        }
        #endregion

        #region Opslaan
        public IActionResult Opslaan(Analyse analyse, OverurenBesparingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DecimalConverter dc = new DecimalConverter();
                    OverurenBesparing baat = new OverurenBesparing
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.OverurenBesparing = baat;
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OpslaanFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion      

        #region Helpers
        private OverurenBesparingViewModel MaakModel(Analyse analyse)
        {
            if (analyse.OverurenBesparing == null)
            {
                return new OverurenBesparingViewModel();
            }

            return new OverurenBesparingViewModel(analyse.OverurenBesparing);
        }
        #endregion
    }
}
