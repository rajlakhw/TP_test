using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class BestsellerParagraphsToExclude
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string LangIanacode { get; set; }
        public string Heading { get; set; }
        public string Title { get; set; }
        public string Paragraph { get; set; }
    }
}
