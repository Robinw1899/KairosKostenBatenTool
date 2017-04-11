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
                    return "Loonkost / functies";
                case Soort.EnclaveKost:
                    return "Kosten voor maatwerkbedrijf of enclave of onderaanneming";
                case Soort.VoorbereidingsKost:
                    return "Voorbereidingskosten";
                case Soort.InfrastructuurKost:
                    return "Infrastructuurkosten";
                case Soort.GereedschapsKost:
                    return "Materiële kosten (bv. kosten van aangepaste werkkledij en -gereedschap)";
                case Soort.OpleidingsKost:
                    return "Opleidingskosten";
                case Soort.BegeleidingsKost:
                    return "Kosten voor administratie en interne begeleiding";
                case Soort.ExtraKost:
                    return "Extra kosten";

                // Baten
                case Soort.MedewerkersZelfdeNiveau:
                    return "Medewerkers op zelfde niveau vervangen";
                case Soort.MedewerkersHogerNiveau:
                    return "Medewerkers op hoger niveau vervangen";
                case Soort.UitzendkrachtBesparing:
                    return "Besparingen op uitzendkrachten";
                case Soort.ExtraOmzet:
                    return "Extra omzet";
                case Soort.ExtraProductiviteit:
                    return "Extra productiviteit";
                case Soort.OverurenBesparing:
                    return "Besparingen op overuren";
                case Soort.ExterneInkoop:
                    return "Externe inkopen, transportskosten en logistieke handlingskosten";
                case Soort.Subsidie:
                    return "Subsidie";
                case Soort.LogistiekeBesparing:
                    return "Logistieke besparing";
                case Soort.ExtraBesparing:
                    return "Extra besparingen";
                
                default:
                    return "";
            }
        }
    }
}
