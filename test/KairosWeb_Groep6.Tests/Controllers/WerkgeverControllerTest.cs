using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class WerkgeverControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IDepartementRepository> _departementRepository;
        private readonly Mock<IWerkgeverRepository> _werkgeverRepository;
        private readonly WerkgeverController _controller;
        #endregion

        #region Constructors
        public WerkgeverControllerTest()
        {
            _analyseRepository = new Mock<IAnalyseRepository>();
            _departementRepository = new Mock<IDepartementRepository>();
            _werkgeverRepository = new Mock<IWerkgeverRepository>();

            _controller = new WerkgeverController(_analyseRepository.Object,
                _departementRepository.Object, _werkgeverRepository.Object);
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact(Skip = "Not implemented yet")]
        public void TestIndex_DepartementNull_RedirectsToSelecteerWerkgever()
        {
            
        }

        [Fact(Skip = "Not implemented yet")]
        public void TestIndex_Succes()
        {

        }
        #endregion

        #region Opslaan
        [Fact(Skip = "Not implemented yet")]
        public void TestOpslaan_RepositoryGooitException_MethodeFaaltNiet()
        {
            
        }

        [Fact(Skip = "Not implemented yet")]
        public void TestOpslaan_Succes()
        {

        }
        #endregion

        #region SelecteerWerkgever
        [Fact]
        public void TestSelecteerWerkgever()
        {
            var result = _controller.SelecteerWerkgever() as ViewResult;
            
            Assert.Equal("SelecteerWerkgever", result?.ViewName);
        }
        #endregion

        #region Nieuwe werkgever -- GET --
        [Fact]
        public void TestNieuweWerkgeverGET_ReturnsViewWithModel()
        {
            var result = _controller.NieuweWerkgever() as ViewResult;
            var model = result?.Model as WerkgeverViewModel;

            Assert.Equal(typeof(WerkgeverViewModel), model?.GetType());
            Assert.Equal(35, model?.PatronaleBijdrage);
        }
        #endregion

        #region Nieuwe werkgever -- POST --
        // To do
        #endregion
    }
}
