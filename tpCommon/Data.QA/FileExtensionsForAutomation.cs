﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class FileExtensionsForAutomation
    {
        public int Id { get; set; }
        public string FileExtension { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
