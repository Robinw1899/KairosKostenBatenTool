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

            try {
                if (text.Contains(","))
                {
                    text=text.Replace(",", ".");
                }
                decimalVal = Convert.ToDecimal(text);
                if(decimalVal < 0)
                    throw new ArgumentException("De ingegeven waarde mag niet kleiner zijn dan 0");
                return decimalVal;
            }catch(FormatException e)
            {
                throw new FormatException("De ingegeven waarde mag geen letters of tekens bevatten");
            }              
            catch (ArgumentNullException e)
            {
                throw e;
            }
        }

        public string ConvertToString(decimal getal)
        {
            return getal.ToString("F", new CultureInfo("nl-BE"));
        }

    }
}
