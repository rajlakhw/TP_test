using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using Data;
using System.Collections;
using Microsoft.AspNetCore.Http;


namespace ViewModels.flowPlusExternal
{
    public class ProjectCreationModel
    {
        public IEnumerable<DropdownOptionViewModel> Languages { get; set; }
        public IReadOnlyCollection<TimeZoneInfo> AllTimeZones { get; set; }
        public ExtranetUsersTemp JobRequestedByContact { get; set; }
        public IEnumerable<DropdownOptionViewModel> AllAvailableCurrencies { get; set; }
        public Org ParentOrg { get; set; }
        public Data.OrgGroup ParentOrgGroup { get; set; }
        public Data.Contact ContactObject { get; set; }
        public string QuoteOrJobRequest { get; set; }
        public int LangageService { get; set; }
        public string ProjectName { get; set; }
        public string SourceLangIANACode { get; set; }
        public IEnumerable<string> TargetLangIANACode { get; set; }
        public bool CreateDifferentItemsForLangs { get; set; }

        public bool FixedDeadlineRequestedByClient { get; set; }
        public DateTime JobDeadline { get; set; }
        public int JobDeadlineHours { get; set; }
        public int JobDeadlineMinutes { get; set; }
        public string JobDeadlineTimeZone { get; set; }
        public List<FileModel> SourceFiles { get; set; }
        public List<FileModel> ReferenceFiles { get; set; }

        public string PONumber { get; set; }
        public bool ReviewRequired { get; set; }
        public string ReviewPlusOrDesignPlus { get; set; }
        public bool NotifyReviewerOfDelivery { get; set; }
        public bool DTPRequired { get; set; }
        public bool TranslateOnlineSelected { get; set; }

        public bool? TargetProofReadingRequired { get; set; }
        public string CustomerSpecificData1 { get; set; }
        public string CustomerSpecificData2 { get; set; }

        public string Notes { get; set; }

        public short InvoiceCurrencyID { get; set; }

        public byte Priority { get; set; }

        public bool ProofreadingRequired { get; set; }

        public bool IsExtraConfidential { get; set; }

        public bool IsPrintingPackagingProject { get; set; }

        public int LanguageServiceSelected { get; set; }

        public byte MTEngineSelected { get; set; }
        public bool IsPostEditingSelected { get; set; }

        public byte PostEditingOption { get; set; }

        public DateTime PEDeadline { get; set; }
        public int PEDeadlineHours { get; set; }
        public int PEDeadlineMinutes { get; set; }
        public string PEDeadlineTimeZone { get; set; }
        public int PEPreTranslateFrom { get; set; }
        public int PESaveTranslationTo { get; set; }
        public int InterpretingHours { get; set; }
        public int InterpretingMinutes { get; set; }
        public string InterpretingOrg { get; set; }
        public string InterpretingAddressLine1 { get; set; }
        public string InterpretingAddressLine2 { get; set; }
        public string InterpretingAddressLine3 { get; set; }
        public string InterpretingAddressLine4 { get; set; }
        public string InterpretingCountyOrState { get; set; }
        public string InterpretingPostOrZip { get; set; }
        public int InterpretingCountryId { get; set; }

    }

    public class FileModel
    {
        public IFormFile file { get; set; }
    }
}
