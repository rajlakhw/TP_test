using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ApprovedOrBlockedLinguisticSupplier
    {
        public int Id { get; set; }
        public int LinguisticSupplierId { get; set; }
        public byte AppliesToDataObjectTypeId { get; set; }
        public int AppliesToDataObjectId { get; set; }
        public byte Status { get; set; }
        public byte LanguageServiceId { get; set; }
        public string SourceLangIanacode { get; set; }
        public string TargetLangIanacode { get; set; }
        public short? DefaultSoftwareApplicationId { get; set; }
        public byte WorkingPatternId { get; set; }
        public short? WorkingTimeStartHours { get; set; }
        public short? WorkingTimeStartMinutes { get; set; }
        public short? WorkingTimeEndHours { get; set; }
        public short? WorkingTimeEndMinutes { get; set; }
        public string Notes { get; set; }
    }
}
