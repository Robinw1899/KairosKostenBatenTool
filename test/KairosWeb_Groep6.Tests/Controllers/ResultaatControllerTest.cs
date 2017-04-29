using System;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class ResultaatControllerTest
    {
        #region Properties
        private readonly ResultaatController _controller;

        private readonly Mock<IAnalyseRepository> _analyseRepo;

        private readonly DummyApplicationDbContext _dbContext;

        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public ResultaatControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();

            _analyse = new Analyse
            {
                Departement = _dbContext.Aldi,

                Loonkosten = _dbContext.Loonkosten,
                ExtraKosten = _dbContext.ExtraKosten,
                BegeleidingsKosten = _dbContext.BegeleidingsKosten,
                MedewerkersZelfdeNiveauBaat = _dbContext.MedewerkerNiveauBaten,
                UitzendKrachtBesparingen = _dbContext.UitzendKrachtBesparingen,
                ExterneInkopen = _dbContext.ExterneInkopen,
                OpleidingsKosten = _dbContext.OpleidingsKosten,
                PersoneelsKosten = _dbContext.PersoneelsKosten,
                GereedschapsKosten = _dbContext.GereedschapsKosten,
                VoorbereidingsKosten = _dbContext.VoorbereidingsKosten,
                EnclaveKosten = _dbContext.EnclaveKosten,
                Subsidie = _dbContext.Subsidie,
                LogistiekeBesparing = _dbContext.LogistiekeBesparing
            };

            _analyseRepo = new Mock<IAnalyseRepository>();

            _controller = new ResultaatController(_analyseRepo.Object);

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_ReturnsViewWithModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            var model = result?.Model as ResultaatViewModel;

            if (model != null)
            {
                Assert.Equal(321679.95M, Math.Round(model.BatenTotaal, 2));
                Assert.Equal(212117.92M, Math.Round(model.KostenTotaal, 2));
                Assert.Equal(109562.03M, Math.Round(model.Totaal, 2));

                Assert.Equal(typeof(ResultaatViewModel), model.GetType());
            }
            else
            {
                Assert.True(false);
            }
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_Succes_RedirectsToIndex()
        {
            _analyseRepo.Setup(a => a.GetById(1)).Returns(_analyse);

            var result = _controller.Opslaan(1) as RedirectToActionResult;

            _analyseRepo.Verify(a => a.Save(), Times.Once);
            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestOpslaan_Faalt_RedirectsToIndex()
        {
            _analyseRepo.Setup(a => a.GetById(1)).Throws(new Exception());

            var result = _controller.Opslaan(1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion

        #region MaakExcel
        [Fact]
        public void TestMaakExcel_Faalt_RedirectsToIndex()
        {
            _analyseRepo.Setup(a => a.GetById(1)).Throws(new Exception());

            var result = _controller.MaakExcel(1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion

        #region Mail -- GET
        [Fact]
        public void TestMail_Succes_ReturnsView()
        {
            var result = _controller.Mail(1) as ViewResult;

            Assert.Equal(typeof(ResultaatMailViewModel), result?.Model.GetType());
        }
        #endregion

        #region Mail -- POST
        [Fact]
        public async void TestMail_Faalt_RedirectsToIndex()
        {
            _analyseRepo.Setup(a => a.GetById(1)).Throws(new Exception());

            var result = await _controller.Mail(new ResultaatMailViewModel(){AnalyseId = 1}) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}
