using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class MiscResource
    {
        public int Id { get; set; }
        public string ResourceName { get; set; }
        public string LangIanacode { get; set; }
        public string StringContent { get; set; }
        public string Description { get; set; }
    }
}
