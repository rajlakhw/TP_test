using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class CustomFiltersForOrderStatusPage
    {
        public int Id { get; set; }
        public string FilterName { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
        public int? JobOrderNumberToShow { get; set; }
        public int? EnquiryOrderNumber { get; set; }
        public int? QuoteIdnumber { get; set; }
        public string ProjectNameToShow { get; set; }
        public string PurchaseOrderNumberToShow { get; set; }
        public string RowsToShowPerPage { get; set; }
        public string StatusToShow { get; set; }
        public string SourceLanguagesToShow { get; set; }
        public string TargetLanguagesToShow { get; set; }
        public string OrgsToShow { get; set; }
        public string ContactsToShow { get; set; }
        public string CustomField1FilterList { get; set; }
        public string CustomField2FilterList { get; set; }
        public string CustomField3FilterList { get; set; }
        public int DataObjectId { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int? CreatedByContactId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? LastModifiedByContactId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByContactId { get; set; }
    }
}
