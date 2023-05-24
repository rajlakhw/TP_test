using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class SharedDoc
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ClientFileName { get; set; }
        public int DataObjectId { get; set; }
        public byte DataObjectTypeId { get; set; }
    }
}
