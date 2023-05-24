using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class LinguisticDatabase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CustomTextField1Name { get; set; }
        public string CustomTextField2Name { get; set; }
        public string CustomTextField3Name { get; set; }
        public string CustomTextField4Name { get; set; }
        public string CustomTextField5Name { get; set; }
        public string CustomTextField6Name { get; set; }
        public string CustomListField1Name { get; set; }
        public string CustomListField2Name { get; set; }
        public string CustomListField3Name { get; set; }
        public string CustomListField4Name { get; set; }
        public string CustomListField5MultiselectName { get; set; }
        public string CustomImageField1Name { get; set; }
        public string CustomBitField1Name { get; set; }
        public string CustomBitField1TrueName { get; set; }
        public string CustomBitField1FalseName { get; set; }
        public string CustomBitField1Nullname { get; set; }
        public string ClientTermKeyIdname { get; set; }
        public bool AllowSearchByClientTermKeyId { get; set; }
        public bool UsesCmspublicationRelease { get; set; }
        public string DefaultSourceLangIanacode { get; set; }
        public bool ShowAdvancedSearchByDefault { get; set; }
        public string EmailAddressesToNotifyOnAddEditOrDeleteTermEntries { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
