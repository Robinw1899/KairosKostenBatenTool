using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Controllers.Kosten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    public class LoonkostControllerTest
    {
        private readonly LoonkostenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepo;
        private readonly DummyApplicationDbContext _dbContext;
        private readonly Mock<IJobcoachRepository> _jobcoachRepo;
        public LoonkostControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<AnalyseRepository>();
            _controller = new LoonkostenController(_analyseRepo.Object, _jobcoachRepo.Object);
            _analyse = new Analyse { MedewerkersZelfdeNiveauBaat = _dbContext.GeefMedewerkerNiveauBaten() };
            _jobcoachRepo = new Mock<IJobcoachRepository>();
        }

        #region Index -- GET --
        [Fact]
        public void TestIndexShouldReturnMedewerkerNiveauIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            LoonkostenIndexViewModel model = result?.Model as LoonkostenIndexViewModel;

            Assert.Equal(3, model?.ViewModels.Count());
        }

        #endregion

        #region VoegToe -- POST --

        [Fact]
        public void TestVoegToe_ModelError_ReturnsToIndexWithModel()
        {
            _controller.ModelState.AddModelError("", "Model error");
            LoonkostenIndexViewModel model = new LoonkostenIndexViewModel();
            Jobcoach jobcoach = new Jobcoach();
            var result = _controller.VoegToe(_analyse, jobcoach, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_ReturnsPartialView()
        {
            LoonkostenIndexViewModel model = new LoonkostenIndexViewModel()
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.Loonkost,
                AantalMaandenIBO = 2,
                AantalUrenPerWeek = 25,
                Bedrag = 2000,
                Beschrijving = "test",
                Doelgroep = Doelgroep.MiddengeschooldTot25,
                IBOPremie  = 1000,
                Ondersteuningspremie = 500,
                
                BrutoMaandloonFulltime = 2500,
                ViewModels = _dbContext.GeefLoonkosten()
                                        .Select(b => new LoonkostViewModel(b))
            };
            Jobcoach jobcoach = new Jobcoach();
            var result = _controller.VoegToe(_analyse, jobcoach, model) as PartialViewResult;

            Assert.Equal("_OverzichtTabel", result?.ViewName);
        }

        #endregion

        #region Bewerk -- GET --

        [Fact]
        public void TestBewerk_ReturnsIndexView()
        {
            var result = _controller.Bewerk(_analyse, 1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestBewerkGET_BaatNull_ReturnsIndexView()
        {
            var result = _controller.Bewerk(_analyse, -1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }
        #endregion

        #region Bewerk -- POST --
        [Fact]
        public void TestBewerk_ModelError_ReturnsIndexView()
        {
            _controller.ModelState.AddModelError("", "Model error");

            LoonkostenIndexViewModel model = new LoonkostenIndexViewModel();
            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestBewerkPOST_BaatNull_ReturnsIndexView()
        {
            LoonkostenIndexViewModel model = new LoonkostenIndexViewModel()
            {
                Id = -1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.Loonkost,
                AantalMaandenIBO = 2,
                AantalUrenPerWeek = 25,
                Bedrag = 2000,
                Beschrijving = "test",
                Doelgroep = Doelgroep.MiddengeschooldTot25,
                IBOPremie = 1000,
                Ondersteuningspremie = 500,
                BrutoMaandloonFulltime = 2500,

                ViewModels = _dbContext.GeefLoonkosten()
                                        .Select(b => new LoonkostViewModel(b))
            };

            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            var modelResult = result?.Model;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(model, modelResult);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            LoonkostenIndexViewModel model = new LoonkostenIndexViewModel()
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.Loonkost,
                AantalMaandenIBO = 2,
                AantalUrenPerWeek = 25,
                Bedrag = 2000,
                Beschrijving = "test",
                Doelgroep = Doelgroep.MiddengeschooldTot25,
                IBOPremie = 1000,
                Ondersteuningspremie = 500,
                BrutoMaandloonFulltime = 2500,
                ViewModels = _dbContext.GeefLoonkosten()
                                        .Select(b => new LoonkostViewModel(b))
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        #endregion

        #region Verwijder -- POST --

        [Fact]
        public void TestVerwijder_GetByGeeftNull_MethodeFaaltNiet()
        {
            var result = _controller.Verwijder(_analyse, -1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestVerwijder_Succes_RedirectsToIndex()
        {
            var result = _controller.Verwijder(_analyse, 1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }
        #endregion

    }
}
