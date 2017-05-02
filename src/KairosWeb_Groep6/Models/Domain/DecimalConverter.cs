using System;
using System.Globalization;
using System.Linq;

namespace KairosWeb_Groep6.Models.Domain
{
    public class DecimalConverter
    {
        
        public DecimalConverter()
        {
        }

        public decimal ConvertToDecimal(string text)
        {
            decimal decimalVal;
            if (text.Any(x => Char.IsLetter(x)))
                throw new ArgumentException("Er mogen geen letters of tekens in de waarde worden meegegeven");
            try
            {
                decimalVal = Convert.ToDecimal(text);
                if(decimalVal < 0)
                    throw new ArgumentException("De ingegeven waarde mag niet kleiner zijn dan 0");
                return decimalVal;
            }           
            catch (ArgumentNullException e)
            {
                throw e;
            }
        }

        public string ConvertToString(decimal getal)
        {
            return getal.ToString("C", new CultureInfo("nl-BE"));
        }

    }
}
