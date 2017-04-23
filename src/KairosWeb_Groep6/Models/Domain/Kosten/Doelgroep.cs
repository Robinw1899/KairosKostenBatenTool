using KairosWeb_Groep6.Models.Domain.Extensions;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class Doelgroep
    {
        #region Properties
        public DoelgroepSoort Soort { get; set; }

        // het minimum brutoloon dat hoort bij deze doelgroep, dit kan veranderen!
        public decimal MinBrutoloon { get; set; }
        #endregion

        #region Constructors
        public Doelgroep()
        {
            
        }

        public Doelgroep(DoelgroepSoort soort, decimal minBrutoloon)
        {
            Soort = soort;
            MinBrutoloon = minBrutoloon;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return DoelgroepSoortExtensions.GeefOmschrijving(Soort);
        }
        #endregion
    }
}
