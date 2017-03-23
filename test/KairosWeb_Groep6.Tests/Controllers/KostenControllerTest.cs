using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Controllers;
using Microsoft.AspNetCore.Mvc;
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
            var result = _controller.InfrastructuurKosten() as RedirectToActionResult;
            Assert.Equal("InfrastructuurKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestLoonKosten_RedirectsToLoonKostenController()
        {
            var result = _controller.Loonkosten() as RedirectToActionResult;
            Assert.Equal("LoonKosten", result?.ControllerName);
            Assert.Equal("Index", result?.ActionName);
        }
        [Fact]
        public void TestOpleidingsKosten_RedirectsToLoonKostenController()
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
