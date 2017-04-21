namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class SoortExtensions
    {
        public static string GeefOmschrijving(this Soort soort)
        {
            switch(soort)
            {
                // Kosten
                case Soort.Loonkost:
                    return "Loonkosten medewerkers met grote afstand tot arbeidsmarkt";
                case Soort.EnclaveKost:
                    return "Kost overname werk door maatwerkbedrijf via enclave of onderaanneming";
                case Soort.VoorbereidingsKost:
                    return "Voorbereiding start medewerker met grote afstand tot de arbeidsmarkt";
                case Soort.PersoneelsKost:
                    return "Extra kosten werkkleding e.a. personeelskosten";
                case Soort.GereedschapsKost:
                    return "Extra kosten voor aanpassingen werkomgeving/aangepast gereedschap";
                case Soort.OpleidingsKost:
                    return "Extra kosten opleiding";
                case Soort.BegeleidingsKost:
                    return "Extra kosten administratie en begeleiding";
                case Soort.ExtraKost:
                    return "Andere kosten";

                // Baten
                case Soort.LoonkostSubsidies:
                    return "Totale loonkostsubsidies (VOP, IBO en doelgroepvermindering)";
                case Soort.MedewerkersZelfdeNiveau:
                    return "Besparing reguliere medew. op hetzelfde niveau";
                case Soort.MedewerkersHogerNiveau:
                    return "Besparing reguliere medew. op hoger niveau";
                case Soort.UitzendkrachtBesparing:
                    return "Besparing (extra) uitzendkrachten";
                case Soort.ExtraOmzet:
                    return "Inperking omzetverlies";
                case Soort.ExtraProductiviteit:
                    return "Productiviteitswinst";
                case Soort.OverurenBesparing:
                    return "Besparingen op overuren";
                case Soort.ExterneInkoop:
                    return "Besparing op outsourcing";
                case Soort.Subsidie:
                    return "Tegemoetkoming in de kosten voor aanpassingen werkomgeving/aangepast gereedschap";
                case Soort.LogistiekeBesparing:
                    return "Logistieke besparing";
                case Soort.ExtraBesparing:
                    return "Andere besparingen";
                
                default:
                    return "";
            }
        }

        public static string GeefVraag(this Soort soort)
        {
            switch (soort)
            {
                // Kosten
                case Soort.Loonkost:
                    return "Welke nieuwe functies zet u in?";
                case Soort.EnclaveKost:
                    return "Hoeveel betaalt u als u werk laten uitvoeren door maatwerkbedrijf via enclave of onderaanneming?";
                case Soort.VoorbereidingsKost:
                    return "Welke kosten moet u ter voorbereiding maken om dit te implementeren?";
                case Soort.PersoneelsKost:
                    return "Welke extra kosten moet u structureel extra maken voor deze inhuur? (alleen kosten zoals werkkleding bij normale inhuur en bij reshoring alle kosten)";
                case Soort.GereedschapsKost:
                    return "Welke kosten moet u structureel jaarlijks extra maken voor deze inhuur? (vul hier de gereedschapskosten ed in)";
                case Soort.OpleidingsKost:
                    return "Welke opleidingskosten moeten er gemaakt worden om de nieuwe kraachten op het gewenste niveau te krijgen?";
                case Soort.BegeleidingsKost:
                    return "Hoeveel uren spendeert u jaarlijks aan extra administratie en interne begeleiding? (voor toelichting, zie knop rechts boven)";
                case Soort.ExtraKost:
                    return "Aan welke kosten kan nog meer worden gedacht?";

                // Baten
                case Soort.MedewerkersZelfdeNiveau:
                    return "Vul hier de lonen op hetzelfde niveau in die mogelijk vervangen worden, met het geschatte aantal uren dat per week weggehaald wordt.";
                case Soort.MedewerkersHogerNiveau:
                    return "Vul hier de lonen op een hoger niveau in die mogelijk vervangen worden, met het geschatte aantal uren dat per week weggehaald wordt.";
                case Soort.UitzendkrachtBesparing:
                    return "Welke besparing denkt u te kunnen maken op uitzendkrachten?";
                case Soort.ExtraOmzet:
                    return "Hoeveel omzet mist u doordat u personeel tekort komt?";
                case Soort.ExtraProductiviteit:
                    return "Hoeveel extra productiviteit denkt u te kunnen maken door inzet van mensen met een grote afstand tot de arbeidsmarkt?";
                case Soort.OverurenBesparing:
                    return "Hoeveel denkt u te kunnen besparen op overuren?";
                case Soort.ExterneInkoop:
                    return "Welke zaken worden extern ingekocht en kan op worden bespaard? Denk hierbij aan bijvoorbeeld uitbesteedde productie, schoonmaak, hoveniers enz.";
                case Soort.Subsidie:
                    return "Welk bedrag krijgt u aan subsidie voor eventuele aanpassingen aan de werkomgeving?";
                case Soort.LogistiekeBesparing:
                    return "Wat zijn de transport- en logistieke handlingskosten van eventueel uitbesteedde zaken?";
                case Soort.ExtraBesparing:
                    return "Aan welke besparingen kan er nog meer gedacht worden? Vul de bedragen in per jaar.";

                default:
                    return "";
            }
        }
    }
}
