using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Country
    {
        public short Id { get; set; }
        public string Isocode { get; set; }
        public string DiallingPrefix { get; set; }
        public byte GeoPoliticalGroup { get; set; }
        public string SageCountryCode { get; set; }
        public bool IsIbancountry { get; set; }
        public byte? Ibanlength { get; set; }
        public bool? Eea { get; set; }
    }
}
