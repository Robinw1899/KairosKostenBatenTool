using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class AnalyseExtensionsTest
    {
        private readonly List<Analyse> _analyses = new List<Analyse>();

        #region InArchief
        [Fact]
        public void TestInArchief_LegeLijst_ReturnedNiks()
        {
            IEnumerable<Analyse> analyses = _analyses.InArchief();
            Assert.Equal(0, analyses.Count());
        }

        [Fact]
        public void TestInArchief_ReturnedAlleAnalysesInArchief()
        {
            int aantalInArchief = 8;
            VulAnalyses(aantalInArchief, 15);
            IEnumerable<Analyse> analyses = _analyses.InArchief();
            Assert.Equal(aantalInArchief, analyses.Count());
        }
        #endregion

        #region NietInArchief        
        [Fact]
        public void TestNietInArchief_LegeLijst_ReturnedNiks()
        {
            IEnumerable<Analyse> analyses = _analyses.NietInArchief();
            Assert.Equal(0, analyses.Count());
        }

        [Fact]
        public void TestNietInArchief_ReturnedAlleAnalysesNietInArchief()
        {
            int aantalNietInArchief = 14;
            int totaal = 20;
            VulAnalyses(totaal - aantalNietInArchief, totaal);
            IEnumerable<Analyse> analyses = _analyses.NietInArchief();
            Assert.Equal(aantalNietInArchief, analyses.Count());
        }
        #endregion

        #region Hulpmethoden
        private void VulAnalyses(int inarchief, int totaal)
        {
            for (int i = 1; i <= totaal; i++)
            {
                if (i <= inarchief)
                {
                    _analyses.Add(new Analyse { AnalyseId = i, InArchief = true });
                }
                else
                {
                    _analyses.Add(new Analyse{AnalyseId = i, InArchief = false});
                }
            }
        }
        #endregion
    }
}
