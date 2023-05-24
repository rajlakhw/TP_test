using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ExtranetAvailableAccessLevel
    {
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public int AccessLevelId { get; set; }
    }
}
