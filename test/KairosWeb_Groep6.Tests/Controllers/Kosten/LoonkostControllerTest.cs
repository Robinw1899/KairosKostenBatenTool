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
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    public class LoonkostControllerTest
    {
        private readonly LoonkostenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepository;
        private readonly DummyApplicationDbContext _dbContext;

        public LoonkostControllerTest()
        {
            _controller = new LoonkostenController(_analyseRepository.Object);
            _analyse = new Analyse { Loonkosten = _dbContext.Loonkosten};
            _analyseRepository = new Mock<AnalyseRepository>();
            _dbContext = new DummyApplicationDbContext();
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }

        [Fact]
        public void TestIndexShouldReturnLoonkostIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;

            LoonkostenIndexViewModel model = result?.Model as LoonkostenIndexViewModel;
            Assert.Equal(3, model?.ViewModels.Count());
        }
    }
}
