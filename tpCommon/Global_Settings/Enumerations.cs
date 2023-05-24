namespace Global_Settings
{
    public class Enumerations
    {
        public enum Brands
        {
            TP = 1,
            JT = 2,
            Prodigious = 3
        }

        public enum JobFilesToBeSavedInRegion
        {
            NoPreference = 0,
            UK = 1,
            EEA = 2
        }

        public enum Departments
        {
            SalesAndMarketing = 1,
            //Operations = 2,
            FinanceAndAccounts = 3,
            Administration = 4,
            IT = 5,
            HRAndTraining = 6,
            CompanyDirectors = 7,
            None = 8,
            GeneralLinguisticServices = 9,
            TranscreationAndProduction = 10
        }

        public enum DataObjectTypes
        {
            Contact = 1,
            Org = 2,
            OrgGroup = 3,
            LinguisticSupplier = 4,
            Employee = 5,
            Conference = 6,
            GeneralReminder = 7,
            JobOrder = 8,
            ClientInvoice = 9,
            JobItem = 10,
            LinguisticDatabaseTermKey = 11,
            LinguisticDatabaseTermEntry = 12,
            LinguisticDatabaseCMSPublication = 13,
            LinguisticDatabaseCMSPublicationFamily = 14
        }

        public enum EmployeeOwnerships
        {
            SalesNewBusinessLead = 1,
            OperationsLead = 2,
            AccountsLead = 3,
            SalesAccountManagerLead = 4,
            EnquiriesLead = 5,
            ClientIntroLead = 6
        }

        public enum Teams
        {
            ScandinavianSales = 1,
            DACHSales = 2,
            FieldSales = 3,
            Enquiries = 4,
            Marketing = 5,
            Amber = 6,
            Flame = 7,
            Coral = 8,
            Ruby = 9,
            LocalisationEngineering = 10,
            DTP = 11,
            VendorManagement = 12,
            Accounts = 13,
            Administration = 14,
            ITSupportAndAdministration = 15,
            HRAndTraining = 16,
            Directors = 17,
            None = 18,
            OperationsManagement = 19,
            Crimson = 20,
            QualityAssurance = 21,
            TranslationAndQA = 22,
            Scarlet = 23,
            SalesManagement = 24,
            FinanceManagement = 25,
            DenmarkSales = 26,
            SwedenSales = 27,
            Akai = 28,
            Cerise = 29,
            FMCG = 30,
            PublicSector = 31,
            Google = 32,
            BeneluxSales = 33,
            Zora = 34,
            Rose = 35,
            ITSoftwareDevelopment = 36,
            Iris = 37,
            OfficeAdminOrTrainingAndRecruitment = 38,
            Maroon = 39,
            TechnicalConsultancy = 40,
            TechnicalOperations = 41,
            TPOperationsManagement = 42
        }

        public enum JobDriveBaseDirectoryPathForApp
        {
            London = 0,
            Sofia = 1
        }

        public enum SupplierStatus
        {
            ApprovedByUsOrByTheEndClient = -1,
            Blocked = 0,
            ApprovedByUsOnly = 1,
            ApprovedByTheEndClient = 2,
            //3: until we have a full "priority of multiple suppliers" (if that's ever needed), 
            //this just temporarily deactivates a normally-approved supplier, 
            //e.g. if just for 1 week while they're on holiday we want a different approved 
            //supplier to receive jobs
            TemporarilyUnavailable = 3
        }

        public enum WorkingPattern
        {
            AnyTime = 0,
            WorkingDaysOnly = 1,
            WeekendsAndHolidayModeDaysOnly = 2,
            // 3: send stuff out on a Friday afternoon if it would mean they will 
            // be working on it over the weekend
            WeekendsAndHolidayModeDaysOnlyAndFridayAfternoons = 3
        }

        public enum TranslationMemoryRequiredValues
        {
            No = 0,
            Yes = 1,
            NotKnownForHistoricalReasons = 2,
            NotApplicable = 3,
            YesAndTagEditorRequired = 4,
            YesAndTrados2009or2011Required = 5,
            YesAndAcrossRequired = 6
        }

        public enum LinguisticSupplierStatus
        {
            NotYetSentToSupplier = 1,
            SentToSupplierAwaitingAcceptance = 2,
            WorkIsInProgress = 3,
            CompleteAndSignedOffBySupplier = 4,
            RejectThisItem = 5
        }

        public enum ReviewStatus
        {
            NoReviewRequired = 0,
            NotYetSentToReviewer = 1,
            SentToReviewerAwaitingAcceptance = 2,
            ReviewInProgress = 3,
            CompleteAndSignedOffByReviewer = 4
        }

        public enum ServiceCategory
        {
            NoSpecificCategory = 0,
            TranslationAndOtherWrittenServices = 1,
            DesktopPublishing = 2,
            FaceToFaceInterpreting = 3,
            TelephoneInterpreting = 4,
            AudioServices = 5,
            ClientCommissionOrReferralPayments = 6,
            ClientReview = 7,
            Certification = 8,
            OtherServices = 9
        }

        public enum TimelineUnits : byte
        {
            Days = 1,
            Weeks = 2
        }

        public enum WordCountDisplay
        {
            DoNotShow = 0,
            ShowTotalsOnly = 1,
            ShowMemoryBreakdownStandard = 2,
            ShowMemoryBreakdownIncludingPerfectMatch = 3
        }

        //public enum AppModes
        //{
        //    DEV,
        //    QAandCustomerWSTesting,
        //    PRODUCTION
        //}

        //public enum RambollContact : short
        //{
        //    Exist = 0,
        //    Deleted = 1,
        //    DontExist = 2,
        //    Locked = 4,
        //    NotRamboll = 5,
        //    NotEnabledForExtranetAccess = 6
        //}
        public enum MemoryApplications : int
        {
            Trados2007 = 0,
            Trados2009 = 1,
            NoneOrUnknown = 2,
            Trados2011 = 3,
            Trados2014 = 4
        }
    }
}
