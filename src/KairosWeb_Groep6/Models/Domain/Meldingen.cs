namespace KairosWeb_Groep6.Models.Domain
{
    public class Meldingen
    {
        private static string einde = ", probeer later opnieuw";

        public static string AnalyseKlaar { get; } =
            "U hebt de analyse reeds aangevinkt als 'Klaar', u kan niets meer bewerken. " +
            "Wilt u toch iets bewerken, vink het vinkje bij het resultaat uit.";

        #region VoegToe foutmeldingen
        public static string VoegToeFoutmeldingKost = "Er ging iets mis tijdens het toevoegen van de kost" + einde;
        public static string VoegToeFoutmeldingBaat = "Er ging iets mis tijdens het toevoegen van de baat" + einde;
        #endregion

        #region VoegToe succesvol meldingen
        public static string VoegToeSuccesvolKost = "De kost is succesvol toegevoegd";
        public static string VoegToeSuccesvolBaat = "De baat is succesvol toegevoegd";
        #endregion

        #region Ophalen foutmeldingen
        public static string OphalenFoutmeldingKost = "Er ging iets mis tijdens het ophalen van de kost" + einde;
        public static string OphalenFoutmeldingBaat = "Er ging iets mis tijdens het ophalen van de baat" + einde;
        #endregion

        #region Opslaan foutmeldingen
        public static string OpslaanFoutmeldingKost = "Er ging iets mis tijdens het opslaan van de kost" + einde;
        public static string OpslaanFoutmeldingBaat = "Er ging iets mis tijdens het opslaan van de baat" + einde;
        #endregion

        #region Opslaan succesvol meldingen
        public static string OpslaanSuccesvolKost = "De kost is succesvol opgeslaan";
        public static string OpslaanSuccesvolBaat = "De baat is succesvol opgeslaan";
        #endregion

        #region Verwijder foutmeldingen
        public static string VerwijderFoutmeldingKost = "Er ging iets mis tijdens het verwijderen van de kost" + einde;
        public static string VerwijderFoutmeldingBaat = "Er ging iets mis tijdens het verwijderen van de baat" + einde;
        #endregion

        #region Verwijder succesvol meldingen
        public static string VerwijderSuccesvolKost = "De kost is succesvol verwijderd";
        public static string VerwijderSuccesvolBaat = "De baat is succesvol verwijderd";
        #endregion
    }
}
