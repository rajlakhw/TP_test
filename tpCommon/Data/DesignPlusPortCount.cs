using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusPortCount
    {
        public short Port { get; set; }
        public int? DocsOpen { get; set; }
        public bool IsDeliveryPort { get; set; }
    }
}
