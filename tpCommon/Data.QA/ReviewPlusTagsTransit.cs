﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ReviewPlusTagsTransit
    {
        public int Id { get; set; }
        public int JobItemId { get; set; }
        public string FileName { get; set; }
        public int Segment { get; set; }
        public string TagIdentifier { get; set; }
        public string AlternativeTagIdentifier { get; set; }
        public string TagText { get; set; }
    }
}
