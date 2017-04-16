using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;
using OfficeOpenXml.Style;

namespace KairosWeb_Groep6.Models.Domain.Excel
{
    public class ExcelWriterResultaat
    {
        private readonly Dictionary<Soort, string> tabelBedragen = new Dictionary<Soort, string>
            {
                //Baten
                {Soort.LoonkostSubsidies, "E8" },
                {Soort.Subsidie, "E9" },
                {Soort.MedewerkersZelfdeNiveau, "E10" },
                {Soort.MedewerkersHogerNiveau, "E11" },
                {Soort.UitzendkrachtBesparing, "E12" },
                {Soort.ExtraOmzet, "E13" },
                {Soort.ExtraProductiviteit, "E14" },
                {Soort.OverurenBesparing, "E15" },
                {Soort.ExterneInkoop, "E16" },
                {Soort.LogistiekeBesparing, "E17" },
                {Soort.ExtraBesparing, "E18" },

                //Kosten
                {Soort.Loonkost, "F8" },
                {Soort.EnclaveKost, "F9" },
                {Soort.VoorbereidingsKost, "F10" },
                {Soort.InfrastructuurKost, "F11" },
                {Soort.GereedschapsKost, "F12" },
                {Soort.OpleidingsKost, "F13" },
                { Soort.BegeleidingsKost, "F14" },
                {Soort.ExtraKost, "F15" }
            };

        public string MaakExcel(Analyse analyse)
        {
            FileInfo template = new FileInfo("Models/Domain/Excel/template.xlsx");
            EncodingProvider pro = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(pro);

            using (ExcelPackage excel = new ExcelPackage(template, true))
            {
                ExcelWorksheet ws = excel.Workbook.Worksheets[1];

                VulBatenIn(analyse, ws);
                VulKostenIn(analyse, ws);

                Byte[] bin = excel.GetAsByteArray();

                string file = "resultaat.xlsx";
                File.WriteAllBytes(file, bin);
                return file;
            }
        }

        public void VulBatenIn(Analyse analyse, ExcelWorksheet ws)
        {
            foreach (var pair in analyse.GeefTotalenBaten())
            {
                Soort soort = pair.Key;
                decimal waarde = pair.Value;

                if (tabelBedragen.ContainsKey(soort))
                {
                    ws.Cells[tabelBedragen[soort]].Value = waarde;
                }
            }
        }
        public void VulKostenIn(Analyse analyse, ExcelWorksheet ws)
        {
            foreach (var pair in analyse.GeefTotalenKosten())
            {
                Soort soort = pair.Key;
                decimal waarde = pair.Value;

                if (tabelBedragen.ContainsKey(soort))
                {
                    ws.Cells[tabelBedragen[soort]].Value = waarde;
                }
            }
        }
    }
}
