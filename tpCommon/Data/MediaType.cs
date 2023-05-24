﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class MediaType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
