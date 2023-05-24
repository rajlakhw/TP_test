using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class BestsellerDetailsTemp
    {
        public int Id { get; set; }
        public string DateTimeGenerated { get; set; }
        public int JobOrderId { get; set; }
        public int JobItemId { get; set; }
        public string JobName { get; set; }
        public int Brand { get; set; }
        public string StyleId { get; set; }
        public string ItemText { get; set; }
        public string ItemAttributes { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }
}
