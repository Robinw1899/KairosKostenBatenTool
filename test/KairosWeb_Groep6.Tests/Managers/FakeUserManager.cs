using System;
using KairosWeb_Groep6.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace KairosWeb_Groep6.Tests.Managers
{
    public class FakeUserManager : UserManager<ApplicationUser>
    {
        #region Constructors
        // lege constructor met allemaal Mocks om de UserManager aan te kunnen maken 
        public FakeUserManager()
            : base(new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
        { }
        #endregion
    }
}
