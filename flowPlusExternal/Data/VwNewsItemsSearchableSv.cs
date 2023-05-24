using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class VwNewsItemsSearchableSv
    {
        public int Id { get; set; }
        public string SearchableText { get; set; }
        public string HyperlinkText { get; set; }
        public string HyperlinkUrl { get; set; }
    }
}
