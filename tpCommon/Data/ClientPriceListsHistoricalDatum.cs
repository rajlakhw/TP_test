using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ClientPriceListsHistoricalDatum
    {
        public int Id { get; set; }
        public int ClientPriceListId { get; set; }
        public decimal MemoryRateForExactMatches { get; set; }
        public decimal MemoryRateForRepetitions { get; set; }
        public decimal MemoryRateForFuzzyMatches { get; set; }
        public decimal MemoryRateForFuzzyBand2Matches { get; set; }
        public decimal MemoryRateForFuzzyBand3Matches { get; set; }
        public decimal MemoryRateForFuzzyBand4Matches { get; set; }
        public decimal MemoryRateForPerfectMatches { get; set; }
        public decimal? MemoryRateForClientSpecificMatches { get; set; }
        public decimal DefaultTranslationRate { get; set; }
        public DateTime LoggedDateTime { get; set; }
        public short LoggedByEmployeeId { get; set; }
    }
}
