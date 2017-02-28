using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Tests.Data
{
    public class DummyApplicationDbContext : DbContext
    {
        
        public IEnumerable<Jobcoach> Jobcoaches { get; set; }
        public IEnumerable<Organisatie> Organisaties { get; set; }
        public IEnumerable<Werkgever> Werkgevers { get; set; }
        public IEnumerable<Functie> Functies { get; set; }
        public Jobcoach CoachDimmy { get; set; }
        public Jobcoach CoachThomas { get; set; }
        public Jobcoach CoachRobin { get; set; }
        public Werkgever WerkgeverUgent { get; set; }
        public Werkgever WerkgeverErasmus { get; set; }
       
        /*public Doelgroep Doelgroep1 { get; set; }
        public Doelgroep Doelgroep2 { get; set; }*/
        public Functie FunctieArbeider { get; set; }
        public Functie FunctieBediende { get; set; }

        public Organisatie OrganisatieHoGent { get; set; }
        public Organisatie OrganisatieColruyt { get; set; }

        public DummyApplicationDbContext()
        {
            OrganisatieHoGent = new Organisatie("HoGent", "Arbeidstraat", 10, 9300, "Aalst");
            OrganisatieColruyt = new Organisatie("Colruyt", "Weggevoerdenstraat", 55, 9404, "Ninove");
            Organisaties = new[] { OrganisatieHoGent, OrganisatieColruyt };


            CoachThomas = new Jobcoach("Aelbrecht", "Thomas", "thomas.aelbrecht@gmail.com", OrganisatieHoGent);
            CoachThomas.JobcoachId = 1;
            CoachRobin = new Jobcoach("Coppens", "Robin", "robbin.coppens@gmail.com", OrganisatieHoGent);
            CoachRobin.JobcoachId = 2;
            CoachDimmy = new Jobcoach("Maenhout", "Dimmy", "dimmy.maenhout@test.be", OrganisatieColruyt);
            CoachDimmy.JobcoachId = 3;
            Jobcoaches = new[] { CoachThomas, CoachRobin, CoachDimmy};

            WerkgeverUgent = new Werkgever("UGent", "Capucienenlaan", 20, 9300, "Aalst", 20);   
            WerkgeverErasmus = new Werkgever("Erasmus","Erasmusstraat", 25, 9450, "Denderhoutem", 30, 10);//patronale bijdrage nog eens verifieren
            Werkgevers = new Werkgever[] {WerkgeverUgent, WerkgeverErasmus};

            FunctieArbeider = new Functie("Arbeider", 40, 2100.00, 500.00, Doelgroep.LaaggeschooldTot25);
            FunctieBediende = new Functie("Bediende", 23, 2500.00, 1000.00, Doelgroep.MiddengeschooldTot25);
            //Functies = new Functie[FunctieArbeider, FunctieBediende]; hier zit nog een fout in!

        }
    }
}
