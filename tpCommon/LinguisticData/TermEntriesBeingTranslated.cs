using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class TermEntriesBeingTranslated
    {
        public int Id { get; set; }
        public int AttachedToJobItemId { get; set; }
        public int TermEntryId { get; set; }
        public byte TranslationStatus { get; set; }
        public string TermTextDraftTranslation { get; set; }
        public DateTime? TermTextDraftTranslationLastModifiedDateTime { get; set; }
        public string TermTextDraftTranslationLastModifiedByExtranetUserName { get; set; }
        public short? TermTextDraftTranslationLastModifiedByEmployeeId { get; set; }
        public string TermTextPostProofreadTranslation { get; set; }
        public DateTime? TermTextPostProofreadTranslationLastModifiedDateTime { get; set; }
        public string TermTextPostProofreadTranslationLastModifiedByExtranetUserName { get; set; }
        public short? TermTextPostProofreadTranslationLastModifiedByEmployeeId { get; set; }
        public DateTime? FinalDeliveryToLinguisticDatabaseDateTime { get; set; }
        public short? FinalDeliveryToLinguisticDatabaseByEmployeeId { get; set; }
    }
}
