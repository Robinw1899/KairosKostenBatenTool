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
        [InlineData(Soort.Loonkost, "Loonkosten medewerkers met grote afstand tot arbeidsmarkt")]
        [InlineData(Soort.EnclaveKost, "Kost overname werk door maatwerkbedrijf via enclave of onderaanneming")]
        [InlineData(Soort.VoorbereidingsKost, "Voorbereiding start medewerker met grote afstand tot de arbeidsmarkt")]
        [InlineData(Soort.InfrastructuurKost, "Extra kosten werkkleding e.a. personeelskosten")]
        [InlineData(Soort.GereedschapsKost, "Extra kosten voor aanpassingen werkomgeving/aangepast gereedschap")]
        [InlineData(Soort.OpleidingsKost, "Extra kosten opleiding")]
        [InlineData(Soort.BegeleidingsKost, "Extra kosten administratie en begeleiding")]
        [InlineData(Soort.ExtraKost, "Andere kosten")]

        // BATEN
        [InlineData(Soort.LoonkostSubsidies, "Totale loonkostsubsidies (VOP, IBO en doelgroepvermindering)")]
        [InlineData(Soort.MedewerkersZelfdeNiveau, "Besparing reguliere medew. op hetzelfde niveau")]
        [InlineData(Soort.MedewerkersHogerNiveau, "Besparing reguliere medew. op hoger niveau")]
        [InlineData(Soort.UitzendkrachtBesparing, "Besparing (extra) uitzendkrachten")]
        [InlineData(Soort.ExtraOmzet, "Inperking omzetverlies")]
        [InlineData(Soort.ExtraProductiviteit, "Productiviteitswinst")]
        [InlineData(Soort.OverurenBesparing, "Besparingen op overuren")]
        [InlineData(Soort.ExterneInkoop, "Besparing op outsourcing")]
        [InlineData(Soort.Subsidie, "Tegemoetkoming in de kosten voor aanpassingen werkomgeving/aangepast gereedschap")]
        [InlineData(Soort.LogistiekeBesparing, "Logistieke besparing")]
        [InlineData(Soort.ExtraBesparing, "Andere besparingen")]
        public void TestGeefOmschrijving(Soort soort, string expected)
        {
            string omschrijving = soort.GeefOmschrijving();

            Assert.Equal(expected, omschrijving);
        }
        #endregion

        #region Vraag
        [Theory]
        // KOSTEN
        [InlineData(Soort.Loonkost, "Welke nieuwe functies zet u in?")]
        [InlineData(Soort.EnclaveKost, "Hoeveel betaalt u als u werk laten uitvoeren door maatwerkbedrijf via enclave of onderaanneming?")]
        [InlineData(Soort.VoorbereidingsKost, "Welke kosten moet u ter voorbereiding maken om dit te implementeren?")]
        [InlineData(Soort.InfrastructuurKost, "Welke extra kosten moet u structureel extra maken voor deze inhuur? (alleen kosten zoals werkkleding bij normale inhuur en bij reshoring alle kosten)")]
        [InlineData(Soort.GereedschapsKost, "Welke kosten moet u structureel jaarlijks extra maken voor deze inhuur? (vul hier de gereedschapskosten ed in)")]
        [InlineData(Soort.OpleidingsKost, "Welke opleidingskosten moeten er gemaakt worden om de nieuwe kraachten op het gewenste niveau te krijgen?")]
        [InlineData(Soort.BegeleidingsKost, "Hoeveel uren spendeert u jaarlijks aan extra administratie en interne begeleiding? (voor toelichting, zie knop rechts boven)")]
        [InlineData(Soort.ExtraKost, "Aan welke kosten kan nog meer worden gedacht?")]

        // BATEN
        [InlineData(Soort.LoonkostSubsidies, "")]
        [InlineData(Soort.MedewerkersZelfdeNiveau, "Vul hier de lonen op hetzelfde niveau in die mogelijk vervangen worden, met het geschatte aantal uren dat per week weggehaald wordt.")]
        [InlineData(Soort.MedewerkersHogerNiveau, "Vul hier de lonen op een hoger niveau in die mogelijk vervangen worden, met het geschatte aantal uren dat per week weggehaald wordt.")]
        [InlineData(Soort.UitzendkrachtBesparing, "Welke besparing denkt u te kunnen maken op uitzendkrachten?")]
        [InlineData(Soort.ExtraOmzet, "Hoeveel omzet mist u doordat u personeel tekort komt?")]
        [InlineData(Soort.ExtraProductiviteit, "Hoeveel extra productiviteit denkt u te kunnen maken door inzet van mensen met een grote afstand tot de arbeidsmarkt?")]
        [InlineData(Soort.OverurenBesparing, "Hoeveel denkt u te kunnen besparen op overuren?")]
        [InlineData(Soort.ExterneInkoop, "Welke zaken worden extern ingekocht en kan op worden bespaard? Denk hierbij aan bijvoorbeeld uitbesteedde productie, schoonmaak, hoveniers enz.")]
        [InlineData(Soort.Subsidie, "Welk bedrag krijgt u aan subsidie voor eventuele aanpassingen aan de werkomgeving?")]
        [InlineData(Soort.LogistiekeBesparing, "Wat zijn de transport- en logistieke handlingskosten van eventueel uitbesteedde zaken?")]
        [InlineData(Soort.ExtraBesparing, "Aan welke besparingen kan er nog meer gedacht worden? Vul de bedragen in per jaar.")]
        public void TestGeefVraag(Soort soort, string expected)
        {
            string vraag = soort.GeefVraag();

            Assert.Equal(expected, vraag);
        }
        #endregion
    }
}
