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
    public class SubsidieController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public SubsidieController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            analyse.UpdateTotalen(_analyseRepository);

            SubsidieViewModel model = MaakModel(analyse);

            return View(model);
        }
        #endregion

        #region Opslaan
        [HttpPost]
        public IActionResult Opslaan(Analyse analyse, SubsidieViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // de baat bestaat reeds:
                    DecimalConverter dc = new DecimalConverter();
                    Subsidie baat = new Subsidie
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.Subsidie = baat;
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
        private SubsidieViewModel MaakModel(Analyse analyse)
        {
            if (analyse.Subsidie == null)
            {
                return new SubsidieViewModel();
            }

            return new SubsidieViewModel(analyse.Subsidie);
        }
        #endregion
    }
}
