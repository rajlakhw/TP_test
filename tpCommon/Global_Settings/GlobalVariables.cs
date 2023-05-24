using System;

namespace Global_Settings
{
    public class GlobalVariables
    {
        public GlobalVariables()
        {

        }

        public const string GlobalVars = "GlobalVars";


        //public AppModes CurrentAppMode;

        public string ConnectionString { get; set; }
        public string CurrentAppModeString { get; set; }
        public Int16 iplusEmployeeID { get; set; } // the internal database employee ID marked as "i plus System"
        public Int16 TBAEmployeeID { get; set; } // the internal database employee ID marked as "TBA System"
        public string ExtranetNetworkBaseDirectoryPath { get; set; }
        public string LondonJobDriveBaseDirectoryPathForApp { get; set; } // where this application will write to job folders on London Job drive
        public string LondonJobDriveBaseDirectoryPathForUser { get; set; } // where the user will see job folders from his/her PC on London Job drive
        public string SofiaJobDriveBaseDirectoryPathForApp { get; set; } // where this application will write to job folders on Sofia Job drive
        public string SofiaJobDriveBaseDirectoryPathForUser { get; set; } // where the user will see job folders from his/her PC on Sofia Job drive
        public string ParisJobDriveBaseDirectoryPathForApp { get; set; } // where this application will write to job folders on Paris Job drive
        public string ParisJobDriveBaseDirectoryPathForUser { get; set; } // where the user will see job folders from his/her PC on Paris Job drive
        public string ExternalJobDriveBaseDirectoryPathForApp { get; set; } // where this application will check job folders if they are not on the internal jobs drive
        public string InternalJobDriveBaseDirectoryPathForApp { get; set; } // where this application will write to job folders
        public string InternalJobDriveBaseDirectoryPathForUser { get; set; } // where the user will see job folders from his/her PC
        public string InternalMirrorredJobDriveBaseDirectoryPathForApp { get; set; } // where the user will see job folders from his/her PC
        public string InternalQuoteDriveBaseDirectoryPathForApp { get; set; } // where this application will write to quote folders
        public string InternalQuoteDriveBaseDirectoryPathForUser { get; set; } // where the user will see quote folders from his/her PC
        public string JobFolderTemplatesPath { get; set; } // contains template folder structures for copying when new jobs are set up
        public string OpenXMLTemplatesPath { get; set; } // contains template files for manipulation via the Open XML Document SDK code
        public string EmailSignatureTemplatesPath;  // contains template files for generating employees' e-mail signatures for Outlook
        public string TeamWorksDownloadDirectorypathForApp { get; set; } // at present, only 1 SDL TeamWorks client (FLSmidth), so this just stores path to where TTP files are saved
                                                                         // Public Shared TeamWorksDefaultPMEmployeeID As Short ' at present, just have 1 nominated employee who is the PM for all new TeamWorks jobs
        public short TeamWorksJobOrderChannelID { get; set; } // the hard-coded ID from the database representing "TeamWorks" as an order channel when setting up new jobs
                                                              // Public Shared AutomationBatchNetworkBaseDirectoryPath As String ' folder under which batch automation task files are saved/processed
                                                              // Public Shared AutomationBatchForDTPNetworkBaseDirectoryPath As String ' folder under which batch automation task files specifically for DTP are saved/processed
                                                              // Public Shared AutomationBatchForCMSNetworkBaseDirectoryPath As String ' folder under which batch automation task files specifically for client-facing CMS functionality are saved/processed
                                                              // Public Shared AutomationBatchForTradosNetworkBaseDirectoryPath As String ' folder under which batch automation task files specifically for Trados automation functionality are saved/processed
        public string PublicWebQuoteRequestFolder { get; set; } // path where info and uploaded files for quote requests from the public website are saved
        public string DefaultLocalBlankTrados2007MemoryPath { get; set; } // a local path to a .tmw memory file for analysing files against a blank memory
        public string TPWSVersionNumberAndDescription { get; set; } // as reported by the Web Service e.g. "TPWS PRODUCTION version 2.1"
        public string WebConferencingPassword { get; set; } // The current password we use to access our external account for web conferencing (e.g. WebEx). This is to help update the display of the current information in the intranet but does not play any functional role.
        public string KnowledgeBasePassword { get; set; } // The current password we use to access our external knowledge base ("share plus") account. This is to help update the display of the current information in the intranet but does not play any functional role.
        public string EnquiriesTeamEmailAddressesString { get; set; } // for e.g. notifying the enquiries/quoting team 
        public int TPInternalMarketingRequestsContactID { get; set; } // contact to assign internal marketing jobs to 
        public string DTDsNetworkFolderPath { get; set; } // where the DTD for TMX files (and any other future formats) live on the network
        public string InternalWebServiceGUID { get; set; } // for connecting to our Web Service from internal applications
        public string PhoneReportsHistoricalFolderPath { get; set; } // for where we move imported files to
        public string DesignPlusExternalFolderPath { get; set; } // design plus folder path
        public string DesignPlusScriptsLocation { get; set; } // design plus scripts location
        public string iPlusGuideLocation { get; set; } // iPlus guide location on the network drive
        public string iPlusGuideLocationForLinguists { get; set; } // iPlus linguist guide location on the network drive
        public string iPlusGuideLocationForJT { get; set; } // iPlus for Jackpot translation guide location on the network drive
        public string TranslationFlowJsonApiUrl { get; set; } // Translation flow URL that returns JSON text for a translation request

        public DateTime DateTimeToLookForExtBaseAtNewLocation = new DateTime(2015, 7, 15, 18, 30, 0);
        public string OldExtranetNetworkBaseDirectoryPathDEV1 { get; set; } // June, 2017: we moved ExtranetBase from TP-SV-LN1-DEV-1 to TP-SV-LN1-NAS-1
        public string OldExtranetNetworkBaseDirectoryPathNAS1 { get; set; } // March, 2019: we moved ExtranetBase from TP-SV-LN1-NAS-1 to TP-SV-LN1-NAS-5
        public string OldDesignPlusExternalFolderPath { get; set; }
        public string NewDesignPlusExternalFolderPath { get; set; }
        public string DesignPlusTempFolderPath { get; set; }
        public DateTime DesignPlusTrackChangesDate { get; set; }
        public DateTime DesignPlusTMIntegrationStartDate { get; set; }
        public string HighFiveImagesDirectoryPath { get; set; }
        public string iplusDownTimeXMLFilePath { get; set; }
        public string designPlusManualAlignmentTemplatePath { get; set; }
        public DateTime DateTimeForLowMarginChangedTo48 { get; set; }// dev req ID 931 by David Richards
        public DateTime DateTimeForLowMarginChangedTo50 { get; set; }
        public DateTime DateTimeForLowMarginChangedTo52 { get; set; }

        public decimal TotalSalesLastFinancialYear { get; set; }
        public string HRDoctorsCertificateFolderLocation { get; set; }
        public string LinguistSearchExportTempLocation { get; set; }
        public string SicknessReportExportTempLocation { get; set; }
        public string ProcessAutomationAccountsGeneralPath { get; set; }
        public string LondonWiFiPassword { get; set; }
        public string SofiaWifiPassword { get; set; }
        public string MarketingImportsDirectory { get; set; }
        public string DFSTempFilesDirectory { get; set; }
        public string TranslatePlusLogoPath { get; set; }
        public string ReferenceTemplatePath { get; set; }
        public string UnbatchedOrdersListReportLocation { get; set; }
        public string GDPRDocTemplatePath { get; set; }
        //public static DateTime ExtranetBaseArchiveCutOffDate = DateTime.DateAdd(DateInterval.Year, -2, DateTime.Now);
        public string InternalNotificationRecipientAddress { get; set; }
        public int FLSmidthEnglishRussianPreferredSupplierID { get; set; }
        public string InternalDirectorsRecipientAddress { get; set; }
        public string InternalMarketingRecipientAddress { get; set; }
        public int DefaultInvoiceChaserEmployeeID { get; set; }   // Atanas
        public string SageAutoExportsFolderLocation { get; set; }
        public string SageTemplatesFolderLocation { get; set; }
        public string AccountsIntranetExpensesFolderLocation { get; set; }
        public string SageXMLWatchedFolderLocation { get; set; }
        public string AccountsNotificationRecipient { get; set; }
        public string InternalHRRecipientAddress { get; set; }
        public int NetEntDBID { get; set; }
        public int NetEntPolandDBID { get; set; }
        public int BostikDBID { get; set; }
        public int SKOVDBID { get; set; }
        public int FLSmidthDBID { get; set; }
        public int CEGODBID { get; set; }
        public int OneTimesTwoDBID { get; set; }
        public int AtlasCopcoNackaApplicationsDBID { get; set; }
        public int AtlasCopcoNantesApplicationsDBID { get; set; }
        public int VeroModaOrgID { get; set; }
        public int LOCOGWebServiceContactID { get; set; }
        public string SQLCommandTimeoutForLinguisticDatabases { get; set; } // often some hefty queries for Linguistic Database
        public string NetworkSystemTemplatesFolderPath { get; set; }
        public int WackerNeusonFTPContactID { get; set; }
        public int KramerFTPContactID { get; set; }
        public int WeidemannFTPContactID { get; set; }
        public int BestsellerPIMFTPContactID { get; set; }
        public int NestleWorldServerContactID { get; set; }
        public int KambiWebServiceContactID { get; set; }
        public int FLSmidthReportLoqOrgID { get; set; }
        public int GiantIconumOrgID { get; set; }
        public int HaldorTopsoeOrgID { get; set; }
        public string StartDateOfSupplierInvoicesExportToSage { get; set; }
        public string StartDateOfClientInvoicesExportToSage { get; set; }
        public int EccoWebServiceContactID { get; set; }
        public string InDesignServerHostName { get; set; }
        public string InDesignServerFontFolderPath { get; set; }
        public string InDesignServerPortNumber { get; set; }
        public string ServerSideTempFolderPath { get; set; }
        public int RentalcarsWorldServerContactID { get; set; }
        public string RentalcarsWorldServerDownloadsFolderPath { get; set; }
        public string flowPlusUserProfileImageDirectory { get; set; }
    }
}
