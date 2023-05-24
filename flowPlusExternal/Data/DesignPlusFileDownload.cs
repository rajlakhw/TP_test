using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusFileDownload
    {
        public int Id { get; set; }
        public int DesignPlusFileId { get; set; }
        public string FileTypeDownloaded { get; set; }
        public bool TranslatedOrReviewedDownload { get; set; }
        public int? DesignPlusReviewJobId { get; set; }
        public int DownloadedByContactId { get; set; }
        public DateTime DownloadedDateTime { get; set; }
    }
}
