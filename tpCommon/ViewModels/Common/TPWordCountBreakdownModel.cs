using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common
{
    public class TPWordCountBreakdownModel
    {
        public int pNewWords { get; set; }
        public int pFuzzyBand1Words { get; set; }
        public int pFuzzyBand2Words { get; set; }
        public int pFuzzyBand3Words { get; set; }
        public int pFuzzyBand4Words { get; set; }
        public int pLinguisticNewWords { get; set; }
        public int pLinguisticFuzzyBand1Words { get; set; }
        public int pLinguisticFuzzyBand2Words { get; set; }
        public int pLinguisticFuzzyBand3Words { get; set; }
        public int pLinguisticFuzzyBand4Words { get; set; }
       // public int pLinguisticRepetitionsWords { get; set; }
        public int pRepetitionsWords { get; set; }
        public int pExactMatchWords { get; set; }
        public int pMatchPlusOrPerfectMatchWords { get; set; }
        public Global_Settings.Enumerations.MemoryApplications pMemoryFormat { get; set; }
        public string pFileName { get; set; }

    }
}
