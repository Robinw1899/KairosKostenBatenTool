using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Tests.Data;
using KairosWeb_Groep6.Tests.Data.Repositories;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

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
        private Jobcoach _jobcoachThomas;

        #region Controller

        public KairosControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _gebruikerRepository = new Mock<IGebruikerRepository>();
            _controller = new KairosController(new DummyGebruikerRepository(_dbContext));
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

        public void EesteKeerAanmelden()
        {

        }

        #endregion

        #region 1steKeerAanmelden HttpPost

        public void EersteKeerAanmdelden_WachtwoordVeranderdIndienSuccesvol()
        {
            Mock<AccountController> _accountController = new Mock<AccountController>();
            _gebruikerRepository.Setup(m => m.GetBy(Email)).Returns(_dbContext.CoachThomas);
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

        public void NieweAnalyse()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
