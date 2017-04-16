using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using KairosWeb_Groep6.Models.Domain;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Data.SqlClient;
using System.Reflection;
using KairosWeb_Groep6.Data.Repositories;

namespace KairosWeb_Groep6.Excel
{
    public class AnalyseResultaat
    {

        //------------------
        /// <summary>
        /// This sample creates a new workbook from a template file containing a chart and populates it with Exchangrates from 
        /// the Adventureworks database and set the three series on the chart.
        /// </summary>
        /// <param name="connectionString">Connectionstring to the Adventureworks db</param>
        /// <param name="template">the template</param>
        /// <param name="outputdir">output dir</param>
        /// <returns></returns>
        public static string RunSample4(string connectionString, FileInfo template, DirectoryInfo outputdir)
        {
            Dictionary<Soort, string> tabelBedragen = new Dictionary<Soort, string>
            {
                //Baten
                {Soort.Subsidie, "E8" },
                {Soort.Subsidie, "E9" },                                    //moet nog aangepast worden
                {Soort.MedewerkersZelfdeNiveau, "E10" },
                {Soort.MedewerkersHogerNiveau, "E11" },
                {Soort.UitzendkrachtBesparing, "E12" },
                {Soort.ExtraOmzet, "E13" },
                {Soort.ExtraProductiviteit, "E14" },
                {Soort.OverurenBesparing, "E15" },
                {Soort.ExterneInkoop, "E16" },
                {Soort.EnclaveKost, "E17" },                       //logistieke besparing moet nog veranderd worden
                {Soort.ExtraBesparing, "E18" },

                //Kosten
                {Soort.Loonkost, "F8" },
                {Soort.EnclaveKost, "F9" },
                {Soort.VoorbereidingsKost, "F10" },
                {Soort.Loonkost, "F11" },                           //moet nog aangepast worden
                {Soort.GereedschapsKost, "F12" },
                {Soort.OpleidingsKost, "F13" },
                { Soort.BegeleidingsKost, "F14" },
                {Soort.ExtraKost, "F15" }
            };

            using (ExcelPackage p = new ExcelPackage(template, true))
            {
                //Set up the headers
                ExcelWorksheet ws = p.Workbook.Worksheets[1];


                Analyse analyse = new Analyse();
                //waarden die in de tabel moeten komen
                ws.Cells[tabelBedragen[Soort.Loonkost]].Value = analyse.Loonkosten.Select(e => e.Id == 1);

                //Bedrag tag
                ws.Cells["E7"].Value = "Bedrag";
                ws.Cells["E7:E8"].Merge = true;
                ws.Cells["E7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                using (ExcelRange row = ws.Cells["D8:D19"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                    row.Style.Font.Color.SetColor(Color.White);
                    row.Style.Font.Bold = false;
                }

                //baten
                ws.Cells["H8"].Value = "Baten";
                ws.Cells["C8:C18"].Merge = true;
                //ws.Cells["H8"].Style.Fill.BackgroundColor.SetColor = ""; achtergrondkleur

                //Tagsbaten
                ws.Cells["D8"].Value = " Totale loonkostsubsidies (VOP, IBO en doelgroepvermindering) ";
                ws.Cells["D9"].Value = " Tegemoetkoming in de kosten voor aanpassingen werkomgeving/aangepast gereedschap ";
                ws.Cells["D10"].Value = " Besparing reguliere medew. op hetzelfde niveau ";
                ws.Cells["D11"].Value = " Besparing reguliere medew. op hoger niveau ";
                ws.Cells["D12"].Value = " Besparing (extra) uitzendkrachten ";
                ws.Cells["D13"].Value = " Inperking omzetverlies ";
                ws.Cells["D14"].Value = " Productiviteitswinst ";
                ws.Cells["D15"].Value = " Besparing op overuren ";
                ws.Cells["D16"].Value = " Besparing op outsourcing ";
                ws.Cells["D17"].Value = " Logistieke besparing ";
                ws.Cells["D18"].Value = " Andere besparingen ";

                using (ExcelRange row = ws.Cells["D8:D19"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 255, 128));
                    row.Style.Font.Color.SetColor(Color.White);
                    row.Style.Font.Bold = false;
                }
                //opmaak tag subtotaal baten
                ws.Cells["D20"].Value = " Subtotaal baten ";
                using (ExcelRange row = ws.Cells["D20"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(77, 255, 77));
                    row.Style.Font.Color.SetColor(Color.Black);
                    row.Style.Font.Bold = true;
                }
                //format voor de velden met een bedrag (hier baten & kosten)
                using (ExcelRange row = ws.Cells["E8:E18:F8:F15"])
                {
                    row.Style.Numberformat.Format = "###.###.##0,00";
                }
                //Subtotalen
                using (ExcelRange row = ws.Cells["E20:F20"])
                {
                    row.Style.Numberformat.Format = "###.###.##0,00";
                }
                //nettoResultaat
                using (ExcelRange row = ws.Cells["E21"])
                {
                    row.Style.Numberformat.Format = "###.###.##0,00";
                }
                //Kosten
                ws.Cells["H8"].Value = "Kosten";
                ws.Cells["H8:H20"].Merge = true;
                //opmaak tagskosten
                using (ExcelRange row = ws.Cells["G8:G19"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 173, 153));
                    row.Style.Font.Color.SetColor(Color.Black);
                    row.Style.Font.Bold = false;
                }
                //TagsKosten
                ws.Cells["G8"].Value = " loonkosten medewerkers met grote afstand tot arbeidsmarkt ";
                ws.Cells["G9"].Value = " Kost overname werk door maatwerkbedrijf via enclave of onderaanneming ";
                ws.Cells["G10"].Value = " voorbereiding start medewerker met grote afstand tot de arbeidsmarkt ";
                ws.Cells["G11"].Value = " extra kosten werkkleding e.a. personeelskosten ";
                ws.Cells["G12"].Value = " extra kosten voor aanpassingen werkomgeving/aangepast gereedschap ";
                ws.Cells["G13"].Value = " extra kosten opleiding ";
                ws.Cells["G14"].Value = " extra kosten administratie en begeleiding ";
                ws.Cells["G15"].Value = " Andere kosten ";

                //opmaak tagskosten
                using (ExcelRange row = ws.Cells["G8:G19"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 173, 153));
                    row.Style.Font.Color.SetColor(Color.Black);
                    row.Style.Font.Bold = false;
                }

                ws.Cells["G20"].Value = " Subtotaal kosten ";
                //opmaak tag subtotaal kosten
                using (ExcelRange row = ws.Cells["G20"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 46, 0));
                    row.Style.Font.Color.SetColor(Color.Black);
                    row.Style.Font.Bold = true;
                }

                ws.Cells["E21:E22:F21:F22"].Merge = true;

                //opmaak Nettoresultaat
                using (ExcelRange row = ws.Cells["E21:E22:F21:F22"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                    row.Style.Font.Color.SetColor(Color.Black);
                    row.Style.Font.Bold = true;
                    //border
                    row.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    row.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    row.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    row.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    //row.Style.Border.BorderAround. = ; nog kijken of ik deze kan gebruiken ipv allemaal apart!
                }    //nodig om een analyse op te halen met een bepaald id, zo kunnen we aan de totalen van de baten en kosten
                AnalyseRepository analyseRepo = new AnalyseRepository();
               
                foreach(var key in analyseRepo.GetById(analyse.AnalyseId).GeefTotalenBaten())
                {
                    double waarde = 0;
                    Soort soort = key.Key;
                    waarde = key.Value;//waarde van de key (= bedrag) wordt in de variabele gestopt
                    if (tabelBedragen.ContainsKey(soort))
                    {
                        ws.Cells[tabelBedragen[soort]].Value = waarde;   
                    }
                }
                foreach (var key in analyseRepo.GetById(analyse.AnalyseId).GeefTotalenKosten())
                {
                    double waarde = 0;
                    Soort soort = key.Key;
                    waarde = key.Value;//waarde van de key (= bedrag) wordt in de variabele gestopt
                    if (tabelBedragen.ContainsKey(soort))
                    {
                        ws.Cells[tabelBedragen[soort]].Value = waarde;
                    }
                }
                //Get the documet as a byte array from the stream and save it to disk.  (This is usefull in a webapplication) ... 
                Byte[] bin = p.GetAsByteArray();
                
                string file = outputdir + "\\Analsye.xlsx";
                File.WriteAllBytes(file, bin);
                return file;
                
            }
        }
    }
}
