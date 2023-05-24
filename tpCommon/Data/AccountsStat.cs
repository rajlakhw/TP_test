using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class AccountsStat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public bool? IncludeVat { get; set; }
    }
}
