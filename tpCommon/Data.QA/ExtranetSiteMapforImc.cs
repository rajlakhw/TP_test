using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ExtranetSiteMapforImc
    {
        public int Id { get; set; }
        public string PageLink { get; set; }
        public int? ParentId { get; set; }
        public bool? ShowOnlyForClients { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionEn { get; set; }
        public string TitleDe { get; set; }
        public string DescriptionDe { get; set; }
        public string TitleSv { get; set; }
        public string DescriptionSv { get; set; }
        public string TitleDa { get; set; }
        public string DescriptionDa { get; set; }
    }
}
