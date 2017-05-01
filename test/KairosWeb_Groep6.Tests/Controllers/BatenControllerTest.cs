using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class BatenControllerTest
    {
        private readonly BatenController _controller;

        public BatenControllerTest()
        {
            _controller = new BatenController();
        }

        [Fact]
        public void TestIndex_ReturnsViewWithModel()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            Analyse analyse = new Analyse
            {
                Departement = dbContext.Aldi,

                Loonkosten = dbContext.Loonkosten,
                ExtraKosten = dbContext.ExtraKosten,
                BegeleidingsKosten = dbContext.BegeleidingsKosten,
                MedewerkersZelfdeNiveauBaat = dbContext.MedewerkerNiveauBaten,
                UitzendKrachtBesparingen =  dbContext.UitzendKrachtBesparingen,
                ExterneInkopen = dbContext.ExterneInkopen,
                OpleidingsKosten = dbContext.OpleidingsKosten,
                PersoneelsKosten = dbContext.PersoneelsKosten,
                GereedschapsKosten = dbContext.GereedschapsKosten,
                VoorbereidingsKosten = dbContext.VoorbereidingsKosten,
                EnclaveKosten = dbContext.EnclaveKosten,
                Subsidie = dbContext.Subsidie,
                LogistiekeBesparing = dbContext.LogistiekeBesparing
            };

            var result = _controller.Index(analyse) as ViewResult;
            var model = result?.Model as BatenIndexViewModel;
            
            Assert.Equal(typeof(BatenIndexViewModel), result?.Model.GetType());
        }

        [Fact]
        public void TestMedewerkerZelfdeNiveau_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.MedewerkerZelfdeNiveau() as RedirectToActionResult;

            Assert.Equal("MedewerkersZelfdeNiveau", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestMedewerkerHogerNiveau_RedirectsToMedewerkerHogerNiveauController()
        {
            var result = _controller.MedewerkerHogerNiveau() as RedirectToActionResult;

            Assert.Equal("MedewerkersHogerNiveau", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestUitzendKrachtBesparingen_RedirectsToUitzendkrachtBesparingenController()
        {
            var result = _controller.UitzendKrachtBesparingen() as RedirectToActionResult;

            Assert.Equal("UitzendKrachtBesparingen", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExtraOmzet_RedirectsToExtraOmzetController()
        {
            var result = _controller.ExtraOmzet() as RedirectToActionResult;

            Assert.Equal("ExtraOmzet", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExtraProductiviteit_RedirectsToExtraProductiviteitController()
        {
            var result = _controller.ExtraProductiviteit() as RedirectToActionResult;

            Assert.Equal("ExtraProductiviteit", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestOverurenBesparingen_RedirectsToOverurenBesparingenController()
        {
            var result = _controller.OverurenBesparing() as RedirectToActionResult;

            Assert.Equal("OverurenBesparing", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExterneInkopen_RedirectsToExterneInkopenController()
        {
            var result = _controller.ExterneInkopen() as RedirectToActionResult;

            Assert.Equal("ExterneInkopen", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestSubsidies_RedirectsToSubsidieController()
        {
            var result = _controller.Subsidie() as RedirectToActionResult;

            Assert.Equal("Subsidie", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestLogistiekeBesparing_RedirectsToLogistiekeBesparingController()
        {
            var result = _controller.LogistiekeBesparing() as RedirectToActionResult;

            Assert.Equal("LogistiekeBesparing", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExtraBesparingen_RedirectsToExtraBesparingenController()
        {
            var result = _controller.ExtraBesparingen() as RedirectToActionResult;

            Assert.Equal("ExtraBesparingen", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
    }
}
