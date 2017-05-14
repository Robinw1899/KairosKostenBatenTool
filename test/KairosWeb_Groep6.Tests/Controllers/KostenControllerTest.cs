using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class KostenControllerTest
    {
        private readonly KostenController _controller;

        public KostenControllerTest()
        {
            _controller = new KostenController();
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
                OpleidingsKosten = dbContext.OpleidingsKosten,
                PersoneelsKosten = dbContext.PersoneelsKosten,
                GereedschapsKosten = dbContext.GereedschapsKosten,
                VoorbereidingsKosten = dbContext.VoorbereidingsKosten,
                EnclaveKosten = dbContext.EnclaveKosten,
            };

            var result = _controller.Index(analyse) as ViewResult;
            var model = result?.Model as KostenIndexViewModel;

            Assert.Equal(model?.GetType(), result?.Model.GetType());

            Assert.True(model?.LoonkostenIngevuld);
            Assert.True(model?.ExtraKostenIngevuld);
            Assert.True(model?.BegeleidingsKostenIngevuld);
            Assert.True(model?.OpleidingsKostenIngevuld);
            Assert.True(model?.PersoneelsKostenIngevuld);
            Assert.True(model?.GereedschapsKostenIngevuld);
            Assert.True(model?.VoorbereidingsKostenIngevuld);
            Assert.True(model?.EnclaveKostenIngevuld);
        }

        [Fact]
        public void TestBegeleidingsKost_RedirectsToBegeleidingsController()
        {
            var result = _controller.BegeleidingsKosten() as RedirectToActionResult;
            Assert.Equal("BegeleidingsKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestEnlcaveKosten_RedirectsToEnclaveKostenController()
        {
            var result = _controller.EnclaveKosten() as RedirectToActionResult;
            Assert.Equal("EnclaveKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestExtraKosten_RedirectsToExtraKostenController()
        {
            var result = _controller.ExtraKosten() as RedirectToActionResult;
            Assert.Equal("ExtraKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestGereedschapsKosten_RedirectsToGereedschapsKostenController()
        {
            var result = _controller.GereedschapsKosten() as RedirectToActionResult;
            Assert.Equal("GereedschapsKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestInfrastructuurKosten_RedirectsToInfrastructuurKostenController()
        {
            var result = _controller.PersoneelsKosten() as RedirectToActionResult;
            Assert.Equal("PersoneelsKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestLoonKosten_RedirectsToLoonkostenController()
        {
            var result = _controller.Loonkosten() as RedirectToActionResult;
            Assert.Equal("Loonkosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestOpleidingsKosten_RedirectsToOpleidingsKostenController()
        {
            var result = _controller.OpleidingsKosten() as RedirectToActionResult;
            Assert.Equal("OpleidingsKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestVoorbereiidngsKosten_RedirectsToLoonKostenController()
        {
            var result = _controller.VoorbereidingsKosten() as RedirectToActionResult;
            Assert.Equal("VoorbereidingsKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
    }
}
