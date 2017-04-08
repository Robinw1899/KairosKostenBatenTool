using KairosWeb_Groep6.Models.Domain;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain
{
    public class PasswordGeneratorTest
    {
        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(10)]
        [InlineData(16)]
        public void TestGeneratePassword(int lengte)
        {
            string password = PasswordGenerator.GeneratePassword(lengte);

            Assert.Equal(lengte, password.Length);
        }
    }
}
