using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class VodropdownItem
    {
        public int Id { get; set; }
        public int VodropdownListId { get; set; }
        public string Value { get; set; }
        public int? Idvalue { get; set; }
        public bool? Exclude { get; set; }
    }
}
