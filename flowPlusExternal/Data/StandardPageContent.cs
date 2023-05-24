using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class StandardPageContent
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int PageRelationshipId { get; set; }
        public string LangIanacode { get; set; }
        public string FileName { get; set; }
        public string RedirectToHardCodedFileName { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string SeokeyWords { get; set; }
        public string Seodescription { get; set; }
        public string MainHeading { get; set; }
        public string BodyContent { get; set; }
        public string SideContent { get; set; }
        public byte SideContentType { get; set; }
        public bool? IncludeInFooterOnAllPages { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
