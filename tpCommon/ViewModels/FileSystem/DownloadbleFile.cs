using System;

namespace ViewModels.FileSystem
{
    public class DownloadbleFile
    {
        public string FileName { get; set; }
        public DateTime LastModifiedGMTDateTime { get; set; }
        public string AbsoluteSystemFilePath { get; set; }
        public string TranslatePlusDescription { get; set; }
        public long FileSizeBytes { get; set; }
    }
}
