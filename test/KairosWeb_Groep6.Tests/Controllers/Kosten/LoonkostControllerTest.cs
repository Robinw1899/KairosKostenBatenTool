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

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    public class LoonkostControllerTest
    {
        private readonly LoonKostController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepository;
        private readonly DummyApplicationDbContext _dbContext;

        public LoonkostControllerTest()
        {
            _controller = new LoonKostController(_analyseRepository.Object);
            _analyse = new Analyse { Loonkost = _dbContext.GeefLoonkosten()};
            _analyseRepository = new Mock<AnalyseRepository>();
            _dbContext = new DummyApplicationDbContext();
        }

        [Fact]
        public void TestIndexShouldReturnLoonkostIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;

            LoonKostIndexViewModel model = result?.Model as LoonKostIndexViewModel;
            Assert.Equal(3, model?.ViewModels.Count());
        }
    }
}
