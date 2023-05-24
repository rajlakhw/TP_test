using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class BetssonTranslationFlowFile
    {
        public int Id { get; set; }
        public int TptextId { get; set; }
        public int TffileId { get; set; }
        public string TffileName { get; set; }
        public string TffileType { get; set; }
        public string TfazureBlobName { get; set; }
    }
}
