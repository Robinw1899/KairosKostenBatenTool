using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class SoortExtensionsTest
    {
        #region Omschrijving
        [Theory]
        // KOSTEN
        [InlineData(Soort.Loonkost, "Loonkost / functies")]
        [InlineData(Soort.EnclaveKost, "Kosten voor maatwerkbedrijf of enclave of onderaanneming")]
        [InlineData(Soort.VoorbereidingsKost, "Voorbereidingskosten")]
        [InlineData(Soort.InfrastructuurKost, "Infrastructuurkosten")]
        [InlineData(Soort.GereedschapsKost, "Materiële kosten (bv. kosten van aangepaste werkkledij en -gereedschap)")]
        [InlineData(Soort.OpleidingsKost, "Opleidingskosten")]
        [InlineData(Soort.BegeleidingsKost, "Kosten voor administratie en interne begeleiding")]
        [InlineData(Soort.ExtraKost, "Extra kosten")]

        // BATEN
        [InlineData(Soort.MedewerkersZelfdeNiveau, "Medewerkers op zelfde niveau vervangen")]
        [InlineData(Soort.MedewerkersHogerNiveau, "Medewerkers op hoger niveau vervangen")]
        [InlineData(Soort.UitzendkrachtBesparing, "Besparingen op uitzendkrachten")]
        [InlineData(Soort.ExtraOmzet, "Extra omzet")]
        [InlineData(Soort.ExtraProductiviteit, "Extra productiviteit")]
        [InlineData(Soort.OverurenBesparing, "Besparingen op overuren")]
        [InlineData(Soort.ExterneInkoop, "Externe inkopen, transportskosten en logistieke handlingskosten")]
        [InlineData(Soort.Subsidie, "Subsidie")]
        [InlineData(Soort.LogistiekeBesparing, "Logistieke besparing")]
        [InlineData(Soort.ExtraBesparing, "Extra besparingen")]
        public void TestGeefOmschrijving(Soort soort, string expected)
        {
            string omschrijving = soort.GeefOmschrijving();

            Assert.Equal(expected, omschrijving);
        }
        #endregion
    }
}
