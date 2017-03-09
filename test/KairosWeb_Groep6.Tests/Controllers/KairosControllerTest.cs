using System;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class KairosControllerTest
    {
        private readonly KairosController _controller;
        private readonly Mock<IGebruikerRepository> _gebruikerRepository;

        private readonly DummyApplicationDbContext _dbContext;

        private EersteKeerAanmeldenViewModel _aanmeldenViewModel;
        private EersteKeerAanmeldenViewModel _foutAanmeldenViewModel;

        private string Wachtwoord = "test";
        private string FoutWachtwoord = "test2";
        private string Email = "thomas.ae@test.be";
        private string FouteEmail = "dimmy.m@test.be";
        

        #region Controller
        
        public KairosControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _gebruikerRepository = new Mock<IGebruikerRepository>();
            
            //eens vragen            //_accountController = new AccountController(new UserManager<ApplicationUser>(), );
            _controller.TempData = new Mock<ITempDataDictionary>().Object;

            _aanmeldenViewModel = new EersteKeerAanmeldenViewModel()
            {
                Email = Email,
                Password = Wachtwoord,
                ConfirmPassword = Wachtwoord
            };

            _foutAanmeldenViewModel = new EersteKeerAanmeldenViewModel()
            {
                Email = FouteEmail,
                Password = Wachtwoord,
                ConfirmPassword = FoutWachtwoord
            };
            //_jobcoachThomas = new Jobcoach("Ae", "thomas", Email, new Organisatie());

        }

        #endregion

        #region 1steKeerAanmelden HttpGet
        [Fact]
        public void EesteKeerAanmelden()
        {

        }

        #endregion

        #region 1steKeerAanmelden HttpPost
        [Fact]
        public void EersteKeerAanmdelden_WachtwoordVeranderdIndienSuccesvol()
        {
            Mock<AccountController> _accountController = new Mock<AccountController>();
            _gebruikerRepository.Setup(m => m.GetByEmail(Email)).Returns(_dbContext.Thomas);
            EersteKeerAanmeldenViewModel EersteKeerAanmeldenVM = new EersteKeerAanmeldenViewModel();
            EersteKeerAanmeldenVM.Password = Wachtwoord;
            EersteKeerAanmeldenVM.ConfirmPassword = Wachtwoord;
            
            Assert.Equal(Wachtwoord, EersteKeerAanmeldenVM.Password);
            Assert.Equal(Wachtwoord, EersteKeerAanmeldenVM.ConfirmPassword);
            _gebruikerRepository.Verify(m => m.Save(), Times.Once);

        }

        #endregion

        #region Index

        [Fact]
        public void Index_()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region NieuweAnalyse
        [Fact]
        public void NieweAnalyse()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
