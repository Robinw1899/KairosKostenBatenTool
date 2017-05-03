namespace KairosWeb_Groep6.Tests.Managers
{
    public class FakeSignInResult : Microsoft.AspNetCore.Identity.SignInResult
    {
        // dit wordt enkel gebruikt om Succeede op true te zetten
        public FakeSignInResult()
        {
            Succeeded = true;
        }
    }
}
