using System;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [ServiceFilter(typeof(JobcoachFilter))]
    public class ArchiefController : Controller
    {
        #region Properties

        private readonly IJobcoachRepository _jobcoachRepository;
        #endregion

        #region Constructors
        public ArchiefController(IJobcoachRepository jobcoachRepository)
        {
            _jobcoachRepository = jobcoachRepository;
        }
        #endregion

        #region Methods
        public IActionResult Index(Jobcoach jobcoach)
        {
            if (jobcoach != null)
            {
                
            }

            return View();
        }

        public IActionResult HaalAnalyseUitArchief()
        {
            
        }

        public IActionResult OpenAnalyse()
        {

        }

        public IActionResult VerwijderAnalyse()
        {

        }

        public IActionResult MaakExcelAnalyse()
        {
            throw new NotImplementedException("Archief/MaakExcelAnalyse");
        }

        public IActionResult MaakPdfAnalyse()
        {
            throw new NotImplementedException("Archief/MaakPdfAnalyse");
        }

        public IActionResult AfdrukkenAnalyse()
        {
            throw new NotImplementedException("Archief/AfdrukkenAnalyse");
        }

        public IActionResult MailAnalyse()
        {
            throw new NotImplementedException("Archief/MailAnalyse");
        }
        #endregion
    }
}
