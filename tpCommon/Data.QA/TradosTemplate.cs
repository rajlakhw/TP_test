using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class TradosTemplate
    {
        public int Id { get; set; }
        public string TradosTemplateName { get; set; }
        public string TradosTemplateFilePath { get; set; }
        public int OrgId { get; set; }
        public bool IsDefaultTempleForEmptyTms { get; set; }
        public bool ToShowOnIplus { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DatetedByEmployeeId { get; set; }
    }
}
