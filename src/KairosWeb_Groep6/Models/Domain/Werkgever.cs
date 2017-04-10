using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Werkgever
    {
        #region Properties
       
        public int WerkgeverId { get; set; }
     
        public string Naam { get; set; } = "";

        public string Straat { get; set; } = "";

        public int? Nummer { get; set; }

        public string Bus { get; set; } = "";

        public int Postcode { get; set; }

        public string Gemeente { get; set; } = "";

        public double AantalWerkuren { get; set; }

        public double PatronaleBijdrage { get; set; } = 35D;
      
        public List<ContactPersoon> ContactPersonen { get; set; } = new List<ContactPersoon>();
             
        public List<Departement> Departementen { get; set; }
        #endregion

        #region Constructors
        public Werkgever()

        {
            
        }

        public Werkgever(string naam, string straat, int nummer, string bus, int postcode, string gemeente, int aantalWerkuren)
            : this(naam, straat, nummer, bus, postcode, gemeente, aantalWerkuren, 35)
        {
            
        }

        public Werkgever(string naam, string straat, int nummer, string bus, int postcode, string gemeente, int aantalWerkuren, double patronaleBijdrage)
        {
            Naam = naam;
            Straat = straat;
            Nummer = nummer;
            Bus = bus;
            Postcode = postcode;
            Gemeente = gemeente;
            AantalWerkuren = aantalWerkuren;
            PatronaleBijdrage = patronaleBijdrage;          
        }
        #endregion

        #region Methods

        public bool Contains(string zoekterm)
        {
            if (Naam.IndexOf(zoekterm, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            if (Gemeente.IndexOf(zoekterm, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
