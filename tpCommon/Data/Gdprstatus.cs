using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Gdprstatus
    {
        public int Id { get; set; }
        public string Gdprvalue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeledDateTime { get; set; }
    }
}
