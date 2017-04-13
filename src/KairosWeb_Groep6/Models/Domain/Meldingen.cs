namespace KairosWeb_Groep6.Models.Domain
{
    public class Meldingen
    {
        private static string einde = ", probeer later opnieuw";

        #region VoegToe meldingen
        public static string VoegToeFoutmeldingKost = "Er ging iets mis tijdens het toevoegen van de kost" + einde;
        public static string VoegToeFoutmeldingBaat = "Er ging iets mis tijdens het toevoegen van de baat" + einde;
        #endregion

        #region Ophalen meldingen
        public static string OphalenFoutmeldingKost = "Er ging iets mis tijdens het ophalen van de kost" + einde;
        public static string OphalenFoutmeldingBaat = "Er ging iets mis tijdens het ophalen van de baat" + einde;
        #endregion

        #region Opslaan meldingen
        public static string OpslaanFoutmeldingKost = "Er ging iets mis tijdens het opslaan van de kost" + einde;
        public static string OpslaanFoutmeldingBaat = "Er ging iets mis tijdens het opslaan van de baat" + einde;
        #endregion

        #region Verwijder meldingen
        public static string VerwijderFoutmeldingKost = "Er ging iets mis tijdens het verwijderen van de kost" + einde;
        public static string VerwijderFoutmeldingBaat = "Er ging iets mis tijdens het verwijderen van de baat" + einde;
        #endregion
    }
}
