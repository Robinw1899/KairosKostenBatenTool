using KairosWeb_Groep6.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace KairosWeb_Groep6.Tests.Managers
{
    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        #region Constructors
        // lege constructor met allemaal Mocks om de SignInManager aan te kunnen maken 
        public FakeSignInManager()
            : base(new Mock<FakeUserManager>().Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object)
        { }
        #endregion
    }
}
