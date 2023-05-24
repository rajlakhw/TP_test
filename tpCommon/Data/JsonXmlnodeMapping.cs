using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class JsonXmlnodeMapping
    {
        public int Id { get; set; }
        public string ClientNodeName { get; set; }
        public string TpxmlnodeName { get; set; }
        public string XmlnodeString { get; set; }
        public string XmlnodeClosingString { get; set; }
        public string JsonfileName { get; set; }
        public DateTime? ExportedDateTime { get; set; }
        public DateTime? LastImportedDateTime { get; set; }
    }
}
