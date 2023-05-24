using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class NewsItem
    {
        public int Id { get; set; }
        public string LangIanacode { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool Live { get; set; }
        public string Headline { get; set; }
        public string BodyCopyStart { get; set; }
        public string BodyCopyContinuation { get; set; }
        public string PageName { get; set; }
        public string SeokeyWords { get; set; }
        public string Seodescription { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
