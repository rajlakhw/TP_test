using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class NetEntTemp
    {
        public int? Id { get; set; }
        public int? LinguisticDatabaseTermId { get; set; }
        public string LanguageIanacode { get; set; }
        public string TermText { get; set; }
        public long? ActualNumOfCharsInTermText { get; set; }
        public byte? Status { get; set; }
        public string CustomTextField1Data { get; set; }
        public string CustomTextField2Data { get; set; }
        public string CustomTextField3Data { get; set; }
        public string CustomTextField4Data { get; set; }
        public string CustomTextField5Data { get; set; }
        public string CustomTextField6Data { get; set; }
        public short? CustomListField1Data { get; set; }
        public short? CustomListField2Data { get; set; }
        public short? CustomListField3Data { get; set; }
        public short? CustomListField4Data { get; set; }
        public byte[] CustomImageField1Data { get; set; }
        public bool? CustomBitField1Data { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public short? CreatedByEmployeeId { get; set; }
        public string CreatedByExtranetUserName { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public string LastModifiedByExtranetUserName { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public string DeletedByExtranetUserName { get; set; }
        public string CurrentlyBeingEditedByExtranetUserName { get; set; }
        public DateTime? CurrentlyBeingEditedAsOfDateTime { get; set; }
    }
}
