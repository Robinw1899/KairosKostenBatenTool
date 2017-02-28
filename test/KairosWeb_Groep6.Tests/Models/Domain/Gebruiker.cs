using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain
{
    public class Gebruiker
    {

        [Fact]
        public void TestA()
        {
            Functie functie = new Functie();
            functie.AantalUrenPerWeek = 37.00;
            Assert.Equal(37.00, functie.AantalUrenPerWeek);
        }
    }
}
