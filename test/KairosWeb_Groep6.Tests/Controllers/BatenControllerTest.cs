using KairosWeb_Groep6.Controllers;
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
        public void TestMedewerkerZelfdeNiveau_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.MedewerkerZelfdeNiveau() as RedirectToActionResult;

            Assert.Equal("MedewerkersZelfdeNiveau", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestMedewerkerHogerNiveau_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.MedewerkerHogerNiveau() as RedirectToActionResult;

            Assert.Equal("MedewerkersHogerNiveau", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestUitzendKrachtBesparingen_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.UitzendKrachtBesparingen() as RedirectToActionResult;

            Assert.Equal("UitzendKrachtBesparingen", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExtraOmzet_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.ExtraOmzet() as RedirectToActionResult;

            Assert.Equal("ExtraOmzet", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExtraProductiviteit_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.ExtraProductiviteit() as RedirectToActionResult;

            Assert.Equal("ExtraProductiviteit", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestOverurenBesparingen_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.OverurenBesparingen() as RedirectToActionResult;

            Assert.Equal("OverurenBesparingen", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExterneInkopen_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.ExterneInkopen() as RedirectToActionResult;

            Assert.Equal("ExterneInkopen", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestSubsidies_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.Subsidies() as RedirectToActionResult;

            Assert.Equal("Subsidies", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestExtraBesparingen_RedirectsToMedewerkerZelfdeNiveauController()
        {
            var result = _controller.ExtraBesparingen() as RedirectToActionResult;

            Assert.Equal("ExtraBesparingen", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
    }
}
