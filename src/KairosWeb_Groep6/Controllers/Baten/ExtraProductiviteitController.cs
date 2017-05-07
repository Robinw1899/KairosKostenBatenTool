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
    public class ExtraProductiviteitController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public ExtraProductiviteitController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            var model = MaakModel(analyse);

            return View(model);
        }
        #endregion

        #region Opslaan
        public IActionResult Opslaan(Analyse analyse, ExtraProductiviteitViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DecimalConverter dc = new DecimalConverter();
                    ExtraProductiviteit baat = new ExtraProductiviteit
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.ExtraProductiviteit = baat;
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
        private ExtraProductiviteitViewModel MaakModel(Analyse analyse)
        {
            if (analyse.ExtraProductiviteit == null)
            {
                return new ExtraProductiviteitViewModel();
            }
           
            return new ExtraProductiviteitViewModel(analyse.ExtraProductiviteit);
        }
        #endregion
    }
}
