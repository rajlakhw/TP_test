using System;
using Microsoft.Extensions.Configuration;

namespace Global_Settings
{
    public class GlobalSettings
    {
        private readonly IConfiguration configuration;

        public GlobalSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void SetAppMode(GlobalVariables.AppModes AppMode)
        {
            GlobalVariables.CurrentAppMode = AppMode;

            TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations = new Dictionary<TPProcessAutomationNonClientSpecific.AutomationServers, string>();
            TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations = new Dictionary<TPProcessAutomationNonClientSpecific.AutomationServers, string>();
            TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions = new Dictionary<TPProcessAutomationNonClientSpecific.AutomationServers, string>();
            TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions = new Dictionary<TPProcessAutomationNonClientSpecific.AutomationServers, string>();

            switch (AppMode)
            {
                case GlobalVariables.AppModes.DEV:
                    {
                        TPCommonDataUtils.CoreConnectionStringName = "DEV"; // as of Nov 2011, runs off a dedicated DEV core database centrally
                        TPCommonDataUtils.LinguisticDatabasesConnectionStringName = "LinguisticDatabasesDEV"; // likewise, now a central DEV database for linguistic database content
                        TPCommonDataUtils.PublicWebsiteConnectionStringName = "TPPublicWebDevNEW2"; // likewise, now a DEV database on the server
                                                                                                    // OldExtranetNetworkBaseDirectoryPathDEV1 = "\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseDEV"
                                                                                                    // OldExtranetNetworkBaseDirectoryPathNAS1 = "\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseDEV"
                        GlobalVariables.ExtranetNetworkBaseDirectoryPath = @"\\10.196.48.130\ExtranetBase\DEV";
                        TPCommonEmailUtils.InternalNotificationRecipientAddress = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com,martin.stoimenov@translateplus.com";

                        GlobalVariables.SofiaJobDriveBaseDirectoryPathForApp = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVJDrive";
                        GlobalVariables.LondonJobDriveBaseDirectoryPathForApp = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVJDrive";
                        GlobalVariables.SofiaJobDriveBaseDirectoryPathForUser = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVJDrive";
                        GlobalVariables.LondonJobDriveBaseDirectoryPathForUser = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVJDrive";

                        // \\fredcpfspdm0003\SharedInfo\Marketing\Guides\i plus - linguists\i plus user guide - linguists.pdf

                        // InternalJobDriveBaseDirectoryPathForApp = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVJDrive"
                        // InternalJobDriveBaseDirectoryPathForUser = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVJDrive"
                        GlobalVariables.InternalQuoteDriveBaseDirectoryPathForApp = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVQDrive";
                        GlobalVariables.InternalQuoteDriveBaseDirectoryPathForUser = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVQDrive";
                        GlobalVariables.JobFolderTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\JobFolders";
                        GlobalVariables.TranslatePlusLogoPath = @"\\10.196.48.130\ExtranetBase\Logos\";
                        GlobalVariables.OpenXMLTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\OpenXML";
                        GlobalVariables.EmailSignatureTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\Signatures";
                        // AutomationBatchNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationDEV"
                        // AutomationBatchForDTPNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationDTPDEV"
                        // AutomationBatchForCMSNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationCMSDEV"
                        // AutomationBatchForTradosNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationTradosDEV"
                        GlobalVariables.TeamWorksDownloadDirectorypathForApp = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\TeamWorksDownloadFolder";
                        // TeamWorksDefaultPMEmployeeID = 2
                        GlobalVariables.TeamWorksJobOrderChannelID = 10;
                        GlobalVariables.iplusEmployeeID = 9;
                        GlobalVariables.TBAEmployeeID = 216;
                        TPProcessAutomationClientSpecific.FLSmidthEnglishRussianPreferredSupplierID = 10870;
                        TPCommonEmailUtils.InternalDirectorsRecipientAddress = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPCommonEmailUtils.InternalMarketingRecipientAddress = "Vee.Kiganda@translateplus.com";
                        // PublicWebQuoteRequestFolder = "C:\Users\RobertT\Desktop\PublicWebQuoteTest"
                        GlobalVariables.PublicWebQuoteRequestFolder = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVPublicWebQuotes";
                        TPAccountingLogic.DefaultInvoiceChaserEmployeeID = 6;
                        GlobalVariables.DefaultLocalBlankTrados2007MemoryPath = @"C:\Users\RobertT\Documents\Dummy TM\EmptyLocalDummy.tmw";
                        TPAccountingLogic.SageTemplatesFolderLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\SageImportExport";
                        TPAccountingLogic.SageAutoExportsFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVSageTPAutoExports";
                        TPAccountingLogic.AccountsIntranetExpensesFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVIntranetExpenses";
                        TPAccountingLogic.SageXMLWatchedFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\TPXMLToOnlineBanking";
                        TPAccountingLogic.AccountsNotificationRecipient = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPCommonEmailUtils.InternalHRRecipientAddress = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPLinguisticDatabasesLogic.NetEntDBID = 1004;
                        TPLinguisticDatabasesLogic.BostikDBID = 1054;
                        TPLinguisticDatabasesLogic.SKOVDBID = 0;
                        TPLinguisticDatabasesLogic.FLSmidthDBID = 0;
                        TPLinguisticDatabasesLogic.CEGODBID = 0;
                        TPLinguisticDatabasesLogic.OneTimesTwoDBID = 0;
                        TPLinguisticDatabasesLogic.AtlasCopcoNackaApplicationsDBID = 0;
                        TPLinguisticDatabasesLogic.AtlasCopcoNantesApplicationsDBID = 0;
                        // TPProcessAutomationClientSpecific.NetEntFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\NetEntertainment\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.EdictFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Edict\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.CEGOFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\CEGO\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.AtlasCopcoFTPBaseFolderPath = ""
                        // TPProcessAutomationClientSpecific.BostikFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Bostik\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.EvolutionFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Evolution\_TEST_AutoPublish\"
                        // TPLinguisticDatabasesLogic.NetEntAutoImportFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\NetEntertainment\_TEST_AutoImport\"
                        TPProcessAutomationClientSpecific.VeroModaOrgID = 0; // to be updated
                        TPProcessAutomationClientSpecific.LOCOGWebServiceContactID = 0; // to be updated
                        TPWSVersionNumberAndDescription = "TPWS DEV version 2.14";
                        TPCommonDataUtils.SQLCommandTimeoutForLinguisticDatabases = 3600; // often some hefty queries for Linguistic Databases
                        GlobalVariables.EnquiriesTeamEmailAddressesString = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPFileSystem.NetworkSystemTemplatesFolderPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV";
                        // TPProcessAutomationClientSpecific.WackerNeusonFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\WackerNeusonTypo3\01ToTrans"
                        TPProcessAutomationClientSpecific.WackerNeusonFTPContactID = 82076;
                        // TPProcessAutomationClientSpecific.KramerFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\KramerTypo3TEST\01ToTrans"
                        TPProcessAutomationClientSpecific.KramerFTPContactID = 82075;
                        // TPProcessAutomationClientSpecific.WeidemannFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\KramerTypo3TEST\01ToTrans"
                        TPProcessAutomationClientSpecific.WeidemannFTPContactID = 82075;
                        // TPProcessAutomationClientSpecific.BestsellerPIMFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\BestsellerPIM\TEST\01ToTrans"
                        TPProcessAutomationClientSpecific.BestsellerPIMFTPContactID = 82098;
                        // TPProcessAutomationClientSpecific.FLSmidthPlunetDownloadsFolderPath = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\PlunetDownloadFolder"
                        TPInternalMarketingRequestsContactID = 82067;
                        TPProcessAutomationClientSpecific.NestleWorldServerContactID = 82102;
                        // TPProcessAutomationClientSpecific.NestleWorldServerDownloadsFolderPath = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\NestleWorldServerDownloadFolder"
                        TPProcessAutomationClientSpecific.KambiWebServiceContactID = 82070;
                        TPProcessAutomationClientSpecific.FLSmidthReportLoqOrgID = 0;
                        TPProcessAutomationClientSpecific.GiantIconumOrgID = 0;
                        TPProcessAutomationClientSpecific.HaldorTopsoeOrgID = 0;
                        GlobalVariables.DTDsNetworkFolderPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\Software\DTDs";
                        GlobalVariables.InternalWebServiceGUID = "115022B0-4FF6-4463-AD29-C2F4E64CF136";
                        GlobalVariables.PhoneReportsHistoricalFolderPath = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVZDrive\UC560Historical";
                        TPAccountingLogic.SetUpHistoricalSageFigures();
                        TPAccountingLogic.StartDateOfSupplierInvoicesExportToSage = (DateTime)"01/01/2008 00:00:00";
                        TPAccountingLogic.StartDateOfClientInvoicesExportToSage = (DateTime)"01/01/2008 00:00:00";
                        TPProcessAutomationClientSpecific.EccoWebServiceContactID = 0; // Note from Robert - I haven't needed to test this against a Dev account yet
                        GlobalVariables.DesignPlusExternalFolderPath = @"\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseDEV\design plus\Contacts";
                        GlobalVariables.OldDesignPlusExternalFolderPath = @"\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseDEV\design plus\Contacts";
                        GlobalVariables.NewDesignPlusExternalFolderPath = @"\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseDEV\design plus\Contacts";
                        GlobalVariables.DesignPlusTempFolderPath = @"\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseDEV\design plus\d+temp folder\";
                        GlobalVariables.DesignPlusScriptsLocation = @"\\fredcpfspdm0005\ExtranetBase\design plus\Adobe InDesign CC Server TP scripts\TPDesignPlusScriptsDEV";
                        TPInDesignAutomationLogic.InDesignServerHostName = "fredcpappdm0001";
                        TPInDesignAutomationLogic.InDesignServerFontFolderPath = @"\\tp-sv-ln1-app-1.translateplus.local\c$\Program Files\Adobe\Adobe InDesign CC Server 2018\Fonts\";
                        TPInDesignAutomationLogic.InDesignServerPortNumber = 12345;
                        TPInDesignAutomationLogic.ServerSideTempFolderPath = @"\\TP-SV-LN1-NAS-5.translateplus.local\TPInDesignTempFiles\Dev";
                        GlobalVariables.iPlusGuideLocation = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide v3.9.pdf";
                        GlobalVariables.iPlusGuideLocationForLinguists = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide - 09 May 18.pdf";
                        GlobalVariables.iPlusGuideLocationForJT = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide v3.9- JT.pdf";
                        GlobalVariables.ProcessAutomationAccountsGeneralPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\ProcessAutomationAccountsGeneral";
                        GlobalVariables.TranslationFlowJsonApiUrl = "https://app.translationflow.com/api/v1/translationrequest/";

                        GlobalVariables.DesignPlusTrackChangesDate = new DateTime(2015, 7, 27, 9, 0, 0);
                        GlobalVariables.DesignPlusTMIntegrationStartDate = new DateTime(2015, 7, 23, 0, 0, 0);
                        GlobalVariables.HighFiveImagesDirectoryPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\High five pictures\";

                        // CurrentBrand = Brands.TP
                        GlobalVariables.iplusDownTimeXMLFilePath = @"\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseDEV\System config\iplusUpdateMessage.xml";
                        GlobalVariables.designPlusManualAlignmentTemplatePath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\design plus manual alignment template.xltx";
                        GlobalVariables.HRDoctorsCertificateFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\HRDoctorsCertificate\";
                        GlobalVariables.LinguistSearchExportTempLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\Linguist search result export.xlt";
                        GlobalVariables.SicknessReportExportTempLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\Sickness report template.xltx";
                        GlobalVariables.MarketingImportsDirectory = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\marketing imports";
                        GlobalVariables.ReferenceTemplatePath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\ReferenceTemplate.dot";
                        GlobalVariables.UnbatchedOrdersListReportLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\Unbatched orders list report.xlt";
                        break;
                    }

                case GlobalVariables.AppModes.QAandCustomerWSTesting // can be used for internal QA but also (esp. for Web Service) allows external customers to submit jobs to a non-production database
         :
                    {
                        TPCommonDataUtils.CoreConnectionStringName = "QAandCustomerWSTesting";
                        TPCommonDataUtils.LinguisticDatabasesConnectionStringName = "TPLinguisticDatabasesQAandCustomerWSTesting";
                        TPCommonDataUtils.PublicWebsiteConnectionStringName = "TPPublicWebQA";
                        // OldExtranetNetworkBaseDirectoryPathDEV1 = "\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseQA"
                        // OldExtranetNetworkBaseDirectoryPathNAS1 = "\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBaseQA"
                        GlobalVariables.ExtranetNetworkBaseDirectoryPath = @"\\10.196.48.130\ExtranetBase\QA";

                        TPCommonEmailUtils.InternalNotificationRecipientAddress = "KavitaJ@translateplus.com,ArianD@translateplus.com,martin.stoimenov@translateplus.com";

                        // SofiaJobDriveBaseDirectoryPathForApp = "\\TP-SV-LN1-NAS-2.translateplus.local\JobsQA"
                        // LondonJobDriveBaseDirectoryPathForApp = "\\TP-SV-LN1-NAS-2.translateplus.local\JobsQA"
                        // SofiaJobDriveBaseDirectoryPathForUser = "\\TP-SV-LN1-NAS-2.translateplus.local\JobsQA"
                        // LondonJobDriveBaseDirectoryPathForUser = "\\TP-SV-LN1-NAS-2.translateplus.local\JobsQA"
                        GlobalVariables.SofiaJobDriveBaseDirectoryPathForApp = @"\\gblonpfspdm0005\Jobs\QA";
                        GlobalVariables.LondonJobDriveBaseDirectoryPathForApp = @"\\gblonpfspdm0005\Jobs\QA";
                        GlobalVariables.SofiaJobDriveBaseDirectoryPathForUser = @"\\gblonpfspdm0005\Jobs\QA";
                        GlobalVariables.LondonJobDriveBaseDirectoryPathForUser = @"\\gblonpfspdm0005\Jobs\QA";

                        // InternalJobDriveBaseDirectoryPathForApp = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\QAandClientTestingJDrive"
                        // InternalJobDriveBaseDirectoryPathForUser = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\QAandClientTestingJDrive"
                        GlobalVariables.InternalQuoteDriveBaseDirectoryPathForApp = @"\\gblonpfspdm0005\Quotes\QA";
                        GlobalVariables.InternalQuoteDriveBaseDirectoryPathForUser = @"\\gblonpfspdm0005\Quotes\QA";
                        GlobalVariables.JobFolderTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\JobFolders";
                        GlobalVariables.TranslatePlusLogoPath = @"\\10.196.48.130\ExtranetBase\Logos\";
                        GlobalVariables.OpenXMLTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\OpenXML";
                        GlobalVariables.EmailSignatureTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\Signatures";
                        // AutomationBatchNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationQA"
                        // AutomationBatchForDTPNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationDTPQA"
                        // AutomationBatchForCMSNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationCMSQA"
                        // AutomationBatchForTradosNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationTradosQA"
                        TeamWorksDownloadDirectorypathForApp = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\TeamWorksDownloadFolder";
                        // TeamWorksDefaultPMEmployeeID = 6
                        TeamWorksJobOrderChannelID = 10;
                        iplusEmployeeID = 9;
                        TBAEmployeeID = 213;
                        TPProcessAutomationClientSpecific.FLSmidthEnglishRussianPreferredSupplierID = 10870;
                        TPCommonEmailUtils.InternalDirectorsRecipientAddress = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPCommonEmailUtils.InternalMarketingRecipientAddress = "Vee.Kiganda@translateplus.com";
                        PublicWebQuoteRequestFolder = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\QAPublicWebQuotes";
                        TPAccountingLogic.DefaultInvoiceChaserEmployeeID = 6;
                        DefaultLocalBlankTrados2007MemoryPath = @"C:\Users\RobertT\Documents\Dummy TM\EmptyLocalDummy.tmw";
                        TPAccountingLogic.SageAutoExportsFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVSageTPAutoExports";
                        TPAccountingLogic.SageTemplatesFolderLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesDEV\SageImportExport";
                        TPAccountingLogic.AccountsIntranetExpensesFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVIntranetExpenses";
                        TPAccountingLogic.SageXMLWatchedFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\TPXMLToOnlineBanking";
                        TPAccountingLogic.AccountsNotificationRecipient = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPCommonEmailUtils.InternalHRRecipientAddress = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPLinguisticDatabasesLogic.NetEntDBID = 1012;
                        TPLinguisticDatabasesLogic.BostikDBID = 1040;
                        TPLinguisticDatabasesLogic.SKOVDBID = 0;
                        TPLinguisticDatabasesLogic.FLSmidthDBID = 0;
                        TPLinguisticDatabasesLogic.CEGODBID = 0;
                        TPLinguisticDatabasesLogic.OneTimesTwoDBID = 0;
                        TPLinguisticDatabasesLogic.AtlasCopcoNackaApplicationsDBID = 1180;
                        TPLinguisticDatabasesLogic.AtlasCopcoNantesApplicationsDBID = 0;
                        // TPProcessAutomationClientSpecific.NetEntFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\NetEntertainment\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.EdictFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Edict\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.CEGOFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\CEGO\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.AtlasCopcoFTPBaseFolderPath = ""
                        // TPProcessAutomationClientSpecific.BostikFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Bostik\_TEST_AutoPublish\"
                        // TPProcessAutomationClientSpecific.EvolutionFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Evolution\_TEST_AutoPublish\"
                        // TPLinguisticDatabasesLogic.NetEntAutoImportFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\NetEntertainment\_TEST_AutoImport\"
                        TPProcessAutomationClientSpecific.VeroModaOrgID = 87684;
                        TPProcessAutomationClientSpecific.LOCOGWebServiceContactID = 57826;
                        TPWSVersionNumberAndDescription = "TPWS external QA/customer TEST version 2.14";
                        TPCommonDataUtils.SQLCommandTimeoutForLinguisticDatabases = 3600; // often some hefty queries for Linguistic Databases 
                        EnquiriesTeamEmailAddressesString = "KavitaJ@translateplus.com,ArianD@translateplus.com,Kaloyan.Nikolov@translateplus.com";
                        TPFileSystem.NetworkSystemTemplatesFolderPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA";
                        // TPProcessAutomationClientSpecific.WackerNeusonFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\WackerNeusonTypo3\01ToTrans"
                        // TPProcessAutomationClientSpecific.WackerNeusonFTPFolderPath = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\DEVZDrive\WackerNeusonTypo3\01ToTrans"
                        TPProcessAutomationClientSpecific.WackerNeusonFTPContactID = 82099;
                        // TPProcessAutomationClientSpecific.KramerFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\KramerTypo3TEST\01ToTrans"
                        TPProcessAutomationClientSpecific.KramerFTPContactID = 82119;
                        // TPProcessAutomationClientSpecific.WeidemannFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\KramerTypo3TEST\01ToTrans"
                        TPProcessAutomationClientSpecific.WeidemannFTPContactID = 82119;
                        // TPProcessAutomationClientSpecific.BestsellerPIMFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\BestsellerPIM\TEST\01ToTrans"
                        TPProcessAutomationClientSpecific.BestsellerPIMFTPContactID = 82098;
                        // TPProcessAutomationClientSpecific.FLSmidthPlunetDownloadsFolderPath = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\PlunetDownloadFolder"
                        TPInternalMarketingRequestsContactID = 82101;
                        TPProcessAutomationClientSpecific.NestleWorldServerContactID = 82102;
                        // TPProcessAutomationClientSpecific.NestleWorldServerDownloadsFolderPath = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\NestleWorldServerDownloadFolder"
                        TPProcessAutomationClientSpecific.KambiWebServiceContactID = 82095;
                        TPProcessAutomationClientSpecific.FLSmidthReportLoqOrgID = 89497;
                        TPProcessAutomationClientSpecific.GiantIconumOrgID = 89888;
                        TPProcessAutomationClientSpecific.HaldorTopsoeOrgID = 0;
                        DTDsNetworkFolderPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\Software\DTDs";
                        InternalWebServiceGUID = "115022B0-4FF6-4463-AD29-C2F4E64CF136";
                        PhoneReportsHistoricalFolderPath = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\QAZDrive\UC560Historical";
                        TPAccountingLogic.SetUpHistoricalSageFigures();
                        TPAccountingLogic.StartDateOfSupplierInvoicesExportToSage = (DateTime)"01/01/2008 00:00:00";
                        TPAccountingLogic.StartDateOfClientInvoicesExportToSage = (DateTime)"01/01/2008 00:00:00";
                        TPProcessAutomationClientSpecific.EccoWebServiceContactID = 82126;
                        DesignPlusExternalFolderPath = @"\\TP-SV-LN1-NAS-5.translateplus.local\ExtranetBase\QA\design plus\Contacts";
                        OldDesignPlusExternalFolderPath = @"\\TP-SV-LN1-NAS-5.translateplus.local\ExtranetBase\QA\design plus\Contacts";
                        NewDesignPlusExternalFolderPath = @"\\TP-SV-LN1-NAS-5.translateplus.local\ExtranetBase\QA\design plus\Contacts";
                        DesignPlusTempFolderPath = @"\\TP-SV-LN1-NAS-5.translateplus.local\ExtranetBase\QA\design plus\d+temp folder\";
                        DesignPlusScriptsLocation = @"\\fredcpfspdm0005\ExtranetBase\design plus\Adobe InDesign CC Server TP scripts\TPDesignPlusScriptsQA";
                        TPInDesignAutomationLogic.InDesignServerHostName = "fredcpappdm0001";
                        TPInDesignAutomationLogic.InDesignServerFontFolderPath = @"\\tp-sv-ln1-app-1.translateplus.local\c$\Program Files\Adobe\Adobe InDesign CC Server 2018\Fonts\";
                        TPInDesignAutomationLogic.InDesignServerPortNumber = 12345;
                        TPInDesignAutomationLogic.ServerSideTempFolderPath = @"\\TP-SV-LN1-NAS-5.translateplus.local\TPInDesignTempFiles\QA";
                        iPlusGuideLocation = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide v3.9.pdf";
                        iPlusGuideLocationForLinguists = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide - 09 May 18.pdf";
                        iPlusGuideLocationForJT = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide v3.9- JT.pdf";
                        TranslationFlowJsonApiUrl = "https://app.translationflow.com/api/v1/translationrequest/";
                        TPProcessAutomationClientSpecific.RentalcarsWorldServerContactID = 82531;
                        TPProcessAutomationClientSpecific.RentalcarsWorldServerDownloadsFolderPath = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\QAandClientTestingJDrive\89898 - Rentalcars.com\WorldServerDownloadFolder";

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, "TP-SS-QA-1");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, @"\\FREDCPAPPDM0001\translateplusSystemFiles\ProcessAutomationTP-SS-QA-1");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, "QA");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, "<li>QA testing</li>");

                        // TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.GeneralServer, "TOWER033")
                        // TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.GeneralServer, "\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\ProcessAutomationQA")
                        // TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.GeneralServer, "general processing - QA")
                        // TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.GeneralServer, String.Format("<li>new Sage XML payment files in: <a href=""file://{0}"">{0}</a></li>" &
                        // "<li>new FLSmidth Plunet downloads (XML + ZIP) in <a href=""file://{1}"">{1}</a></li>" &
                        // "<li>new Wacker Neuson i plus FTP integration XML files for batching in <a href=""file://{2}"">{2}</a></li>" &
                        // "<li>new Kramer i plus FTP integration XML files for batching in <a href=""file://{3}"">{3}</a></li>" &
                        // "<li>new Weidemann i plus FTP integration XML files for batching in  <a href=""file://{4}"">{4}</a></li>",
                        // TPFileSystem.MapTPNetworkPath(TPAccountingLogic.SageXMLWatchedFolderLocation),
                        // TPFileSystem.MapTPNetworkPath(TPProcessAutomationClientSpecific.FLSmidthPlunetDownloadsFolderPath),
                        // TPFileSystem.MapTPNetworkPath(TPProcessAutomationClientSpecific.WackerNeusonFTPFolderPath),
                        // TPFileSystem.MapTPNetworkPath(TPProcessAutomationClientSpecific.KramerFTPFolderPath),
                        // TPFileSystem.MapTPNetworkPath(TPProcessAutomationClientSpecific.WeidemannFTPFolderPath)))

                        DesignPlusTrackChangesDate = new DateTime(2015, 7, 27, 9, 0, 0);
                        DesignPlusTMIntegrationStartDate = new DateTime(2015, 7, 23, 0, 0, 0);
                        HighFiveImagesDirectoryPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\High five pictures\";

                        // CurrentBrand = Brands.TP
                        iplusDownTimeXMLFilePath = @"\\10.196.48.130\ExtranetBase\System config\iplusUpdateMessage.xml";
                        designPlusManualAlignmentTemplatePath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\design plus manual alignment template.xltx";
                        HRDoctorsCertificateFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\HRDoctorsCertificate\";
                        LinguistSearchExportTempLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\Linguist search result export.xlt";
                        SicknessReportExportTempLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\Sickness report template.xltx";
                        MarketingImportsDirectory = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\marketing imports";
                        ReferenceTemplatePath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\ReferenceTemplate.dot";
                        UnbatchedOrdersListReportLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplatesQA\Unbatched orders list report.xlt";
                        break;
                    }

                case GlobalVariables.AppModes.PRODUCTION // current live production settings 
         :
                    {
                        TPCommonDataUtils.CoreConnectionStringName = "PRODUCTION";
                        TPCommonDataUtils.LinguisticDatabasesConnectionStringName = "LinguisticDatabasesPRODUCTION";
                        // TPCommonDataUtils.PublicWebsiteConnectionStringName = "TPPublicWebPRODUCTION"
                        // TPCommonDataUtils.CallDataRecordsConnectionStringName = "PRODUCTIONCallDataRecords"
                        // TPCommonDataUtils.CallDataRecordsArchivedConnectionStringName = "PRODUCTIONCallDataRecordsArchived"
                        // OldExtranetNetworkBaseDirectoryPathDEV1 = "\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBase"
                        // OldExtranetNetworkBaseDirectoryPathNAS1 = "\\TP-SV-LN1-nas-5.translateplus.local\ExtranetBase"
                        ExtranetNetworkBaseDirectoryPath = @"\\10.196.48.130\ExtranetBase";
                        TPCommonEmailUtils.InternalNotificationRecipientAddress = "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com";

                        SofiaJobDriveBaseDirectoryPathForApp = @"\\BGSOFPFSPDM0001\SFJobs";
                        // LondonJobDriveBaseDirectoryPathForApp = "\\TP-SV-LN1-NAS-5.translateplus.local\Jobs"
                        SofiaJobDriveBaseDirectoryPathForUser = "K:"; // this does not need the backslash
                                                                      // LondonJobDriveBaseDirectoryPathForUser = "J:" ' this does not need the backslash

                        // 'ExternalJobDriveBaseDirectoryPathForApp = "\\tp-sv-sf1-nas-5.translateplus.local\Jobs"
                        // 'InternalJobDriveBaseDirectoryPathForApp = "\\TP-SV-LN1-NAS-5.translateplus.local\Jobs"
                        // 'InternalMirrorredJobDriveBaseDirectoryPathForApp = "\\TPJobs\Jobs" ' this does not need the backslash
                        // 'InternalJobDriveBaseDirectoryPathForUser = "J:" ' this does not need the backslash
                        // InternalQuoteDriveBaseDirectoryPathForApp = "\\TP-SV-LN1-NAS-2.translateplus.local\Quotes"
                        // InternalQuoteDriveBaseDirectoryPathForUser = "Q:\" ' this needs the backslash

                        // SofiaJobDriveBaseDirectoryPathForApp = "\\TP-SV-LN1-NAS-2.translateplus.local\JobsQA"
                        LondonJobDriveBaseDirectoryPathForApp = @"\\GBLONPFSPDM0005\Jobs";
                        // SofiaJobDriveBaseDirectoryPathForUser = "\\TP-SV-LN1-NAS-2.translateplus.local\JobsQA"
                        LondonJobDriveBaseDirectoryPathForUser = "J:";

                        // InternalJobDriveBaseDirectoryPathForApp = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\QAandClientTestingJDrive"
                        // InternalJobDriveBaseDirectoryPathForUser = "\\fredcpfspdm0003\SharedInfo\Development\AltNetworkPaths\QAandClientTestingJDrive"
                        InternalQuoteDriveBaseDirectoryPathForApp = @"\\GBLONPFSPDM0005\Quotes";
                        InternalQuoteDriveBaseDirectoryPathForUser = "Q:";

                        JobFolderTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\JobFolders";
                        TranslatePlusLogoPath = @"\\10.196.48.130\ExtranetBase\Logos\";
                        OpenXMLTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\OpenXML";
                        EmailSignatureTemplatesPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\Signatures";
                        // AutomationBatchNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomation"
                        // AutomationBatchForDTPNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationDTP"
                        // AutomationBatchForCMSNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationCMS"
                        // AutomationBatchForTradosNetworkBaseDirectoryPath = "\\TP-SV-LN1-nas-5.translateplus.local\Software\translateplus\ProcessAutomationTrados"
                        TeamWorksDownloadDirectorypathForApp = @"\\tp-sv-sf1-nas-5.translateplus.local\SFJobs\66929 - FLSmidth\Temp TeamWorks downloads";
                        // TeamWorksDefaultPMEmployeeID = 12
                        TeamWorksJobOrderChannelID = 11;
                        iplusEmployeeID = 9;
                        TBAEmployeeID = 435;
                        TPProcessAutomationClientSpecific.FLSmidthEnglishRussianPreferredSupplierID = 10870;
                        TPCommonEmailUtils.InternalDirectorsRecipientAddress = "adrian.metcalf@translateplus.com,umer.nizam@translateplus.com";
                        TPCommonEmailUtils.InternalMarketingRecipientAddress = "Vee.Kiganda@translateplus.com";
                        PublicWebQuoteRequestFolder = @"\\TP-SV-LN1-NAS-2.translateplus.local\PublicWebQuotes";
                        TPAccountingLogic.DefaultInvoiceChaserEmployeeID = 780;   // Atanas
                                                                                  // DefaultLocalBlankTrados2007MemoryPath = "C:\Program Files\translate plus\TPSystemService\EmptyT2007Mem\TPAutomationEmptyMem.tmw"
                        TPAccountingLogic.SageAutoExportsFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Finance and Accounts\Sage\TPAutoExports";
                        TPAccountingLogic.SageTemplatesFolderLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\SageImportExport";
                        TPAccountingLogic.AccountsIntranetExpensesFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Finance and Accounts\ALL\Accounts Department\Accounting Team\Staff expenses\Intranet expenses";
                        TPAccountingLogic.SageXMLWatchedFolderLocation = @"\\fredcpfspdm0003\SharedInfo\Finance and Accounts\Sage\TPXMLToOnlineBanking";
                        TPAccountingLogic.AccountsNotificationRecipient = "Umer.Nizam@translateplus.com";
                        TPCommonEmailUtils.InternalHRRecipientAddress = "NicoleM@translateplus.com";
                        TPLinguisticDatabasesLogic.NetEntDBID = 1012;
                        TPLinguisticDatabasesLogic.NetEntPolandDBID = 1216;
                        TPLinguisticDatabasesLogic.BostikDBID = 1054;
                        TPLinguisticDatabasesLogic.SKOVDBID = 1135;
                        TPLinguisticDatabasesLogic.FLSmidthDBID = 1133;
                        TPLinguisticDatabasesLogic.CEGODBID = 1173;
                        TPLinguisticDatabasesLogic.OneTimesTwoDBID = 1159;
                        TPLinguisticDatabasesLogic.AtlasCopcoNackaApplicationsDBID = 1180;
                        TPLinguisticDatabasesLogic.AtlasCopcoNantesApplicationsDBID = 1209;
                        // TPProcessAutomationClientSpecific.NetEntFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\NetEntertainment\AutoPublish_LIVE\"
                        // TPProcessAutomationClientSpecific.EdictFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Edict\AutoPublish\"
                        // TPProcessAutomationClientSpecific.CEGOFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\CEGO\AutoPublish\"
                        // TPProcessAutomationClientSpecific.AtlasCopcoFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\AtlasCopco\AutoPublish"
                        // TPProcessAutomationClientSpecific.BostikFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Bostik\AutoPublish\"
                        // TPProcessAutomationClientSpecific.EvolutionFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\Evolution\AutoPublish\"
                        // TPLinguisticDatabasesLogic.NetEntAutoImportFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\NetEntertainment\AutoImport_LIVE\"
                        // TPProcessAutomationClientSpecific.SKOVFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\SKOV\AutoPublish\"
                        // TPProcessAutomationClientSpecific.FLSFTPBaseFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\FLSmidth\AutoPublish"
                        TPProcessAutomationClientSpecific.VeroModaOrgID = 87684;
                        TPProcessAutomationClientSpecific.LOCOGWebServiceContactID = 57826;
                        TPWSVersionNumberAndDescription = "TPWS PRODUCTION version 2.14";
                        TPCommonDataUtils.SQLCommandTimeoutForLinguisticDatabases = 3600; // often some hefty queries for Linguistic Databases
                        EnquiriesTeamEmailAddressesString = "Enquiries@translateplus.com";
                        TPFileSystem.NetworkSystemTemplatesFolderPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates";
                        // TPProcessAutomationClientSpecific.WackerNeusonFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\WackerNeusonTypo3\01ToTrans"
                        TPProcessAutomationClientSpecific.WackerNeusonFTPContactID = 95857;
                        // TPProcessAutomationClientSpecific.KramerFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\KramerTypo3\01ToTrans"
                        TPProcessAutomationClientSpecific.KramerFTPContactID = 106373;
                        // TPProcessAutomationClientSpecific.WeidemannFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\WeidemannTypo3\01ToTrans"
                        TPProcessAutomationClientSpecific.WeidemannFTPContactID = 123159;
                        // TPProcessAutomationClientSpecific.BestsellerPIMFTPFolderPath = "\\TP-SV-LN1-NAS-2.translateplus.local\FTP Folders\BestsellerPIM\LIVE\01ToTrans"
                        TPProcessAutomationClientSpecific.BestsellerPIMFTPContactID = 95856;
                        // TPProcessAutomationClientSpecific.FLSmidthPlunetDownloadsFolderPath = "\\tp-sv-sf1-nas-5.translateplus.local\SFJobs\66929 - FLSmidth\Temp Plunet downloads"
                        TPInternalMarketingRequestsContactID = 96558;
                        TPProcessAutomationClientSpecific.NestleWorldServerContactID = 98344;
                        // TPProcessAutomationClientSpecific.NestleWorldServerDownloadsFolderPath = "\\tp-sv-sf1-nas-5.translateplus.local\SFJobs\85594 - Nestlé Deutschland AG\WorldServerDownloadFolder"
                        TPProcessAutomationClientSpecific.KambiWebServiceContactID = 95940;
                        TPProcessAutomationClientSpecific.FLSmidthReportLoqOrgID = 116345;
                        TPProcessAutomationClientSpecific.GiantIconumOrgID = 94254;
                        TPProcessAutomationClientSpecific.HaldorTopsoeOrgID = 83830;
                        DTDsNetworkFolderPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\Software\DTDs";
                        InternalWebServiceGUID = "115022B0-4FF6-4463-AD29-C2F4E64CF136";
                        PhoneReportsHistoricalFolderPath = @"\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\Cisco UC560\UC560\HistoricalCDRFiles";
                        TPAccountingLogic.SetUpHistoricalSageFigures();
                        TPAccountingLogic.StartDateOfSupplierInvoicesExportToSage = (DateTime)"01/01/2014 00:00:00";
                        TPAccountingLogic.StartDateOfClientInvoicesExportToSage = (DateTime)"01/09/2010 00:00:00";
                        TPProcessAutomationClientSpecific.EccoWebServiceContactID = 127653;
                        DesignPlusExternalFolderPath = @"\\TP-SV-LN1-DEV-1.translateplus.local\ExtranetBase\design plus\Contacts";
                        // OldDesignPlusExternalFolderPath = "\\TP-SV-LN1-APP-1.translateplus.local\ExtranetBase\design plus\Contacts"
                        NewDesignPlusExternalFolderPath = @"\\10.196.48.130\ExtranetBase\design plus\Contacts";
                        DesignPlusTempFolderPath = @"\\10.196.48.130\ExtranetBase\design plus\d+temp folder\";
                        DesignPlusScriptsLocation = @"\\fredcpfspdm0005\ExtranetBase\design plus\Adobe InDesign CC Server TP scripts\TPDesignPlusScriptsLIVE";
                        TPInDesignAutomationLogic.InDesignServerHostName = "fredcpappdm0001";
                        TPInDesignAutomationLogic.InDesignServerFontFolderPath = @"\\FREDCPAPPDM0001\Adobe\Adobe InDesign Server 2021\Fonts\";
                        TPInDesignAutomationLogic.InDesignServerPortNumber = 12345;
                        TPInDesignAutomationLogic.ServerSideTempFolderPath = @"\\10.196.48.130\ExtranetBase\TPInDesignTempFiles\Production";
                        iPlusGuideLocation = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide v3.9.pdf";
                        iPlusGuideLocationForLinguists = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide - 09 May 18.pdf";
                        iPlusGuideLocationForJT = @"\\10.196.48.130\ExtranetBase\UserGuides\i plus user guide v3.9- JT.pdf";
                        ProcessAutomationAccountsGeneralPath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\ProcessAutomationAccountsGeneral";
                        TranslationFlowJsonApiUrl = "https://app.translationflow.com/api/v1/translationrequest/";
                        MarketingImportsDirectory = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\marketing imports";
                        DFSTempFilesDirectory = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\DFS Temp files";
                        TPProcessAutomationClientSpecific.RentalcarsWorldServerContactID = 141418;
                        TPProcessAutomationClientSpecific.RentalcarsWorldServerDownloadsFolderPath = @"\\BGSOFPFSPDM0001\SFJobs\117510 - rentalcars.com\WorldServerDownloadFolder";

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, "TP-SS-QA-1");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, @"\\FREDCPAPPDM0001\translateplusSystemFiles\ProcessAutomationTP-SS-QA-1");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, "QA");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.QAServer, "<li>QA testing</li>");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD1, "TP-SS-PROD-1");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD1, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-1");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD1, "TP-SS-PROD-1");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD1, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD2, "TP-SS-PROD-2");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD2, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-2");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD2, "TP-SS-PROD-2");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD2, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD3, "TP-SS-PROD-3");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD3, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-3");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD3, "TP-SS-PROD-3");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD3, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD4, "TP-SS-PROD-4");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD4, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-4");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD4, "TP-SS-PROD-4");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD4, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD5, "TP-SS-PROD-5");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD5, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-5");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD5, "TP-SS-PROD-5");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD5, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD6, "TP-SS-PROD-6");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD6, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-6");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD6, "TP-SS-PROD-6");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD6, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD7, "TP-SS-PROD-7");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD7, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-7");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD7, "TP-SS-PROD-7");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD7, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD8, "TP-SS-PROD-8");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD8, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-8");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD8, "TP-SS-PROD-8");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD8, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD9, "TP-SS-PROD-9");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD9, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-9");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD9, "TP-SS-PROD-9");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD9, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD10, "TP-SS-PROD-10");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD10, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-10");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD10, "TP-SS-PROD-10");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD10, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD11, "TP-SS-PROD-11");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD11, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-11");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD11, "TP-SS-PROD-11");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD11, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD12, "TP-SS-PROD-12");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD12, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-12");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD12, "TP-SS-PROD-12");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD12, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD13, "TP-SS-PROD-13");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD13, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-13");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD13, "TP-SS-PROD-13");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD13, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD14, "TP-SS-PROD-14");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD14, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-14");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD14, "TP-SS-PROD-14");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD14, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD15, "TP-SS-PROD-15");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD15, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-15");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD15, "TP-SS-PROD-15");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD15, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD16, "TP-SS-PROD-16");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD16, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-16");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD16, "TP-SS-PROD-16");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD16, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD17, "TP-SS-PROD-17");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD17, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-17");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD17, "TP-SS-PROD-17");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD17, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD18, "TP-SS-PROD-18");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD18, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-18");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD18, "TP-SS-PROD-18");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD18, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD19, "TP-SS-PROD-19");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD19, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-19");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD19, "TP-SS-PROD-19");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD19, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD20, "TP-SS-PROD-20");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD20, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-20");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD20, "TP-SS-PROD-20");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD20, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD21, "TP-SS-PROD-21");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD21, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-21");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD21, "TP-SS-PROD-21");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD21, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD22, "TP-SS-PROD-22");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD22, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-22");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD22, "TP-SS-PROD-22");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD22, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD23, "TP-SS-PROD-23");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD23, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-23");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD23, "TP-SS-PROD-23");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD23, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD24, "TP-SS-PROD-24");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD24, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-24");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD24, "TP-SS-PROD-24");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD24, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD25, "TP-SS-PROD-25");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD25, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-25");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD25, "TP-SS-PROD-25");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD25, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD26, "TP-SS-PROD-26");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD26, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-26");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD26, "TP-SS-PROD-26");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD26, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD27, "TP-SS-PROD-27");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD27, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-27");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD27, "TP-SS-PROD-27");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD27, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD28, "TP-SS-PROD-28");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD28, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-28");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD28, "TP-SS-PROD-28");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD28, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD29, "TP-SS-PROD-29");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD29, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-29");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD29, "TP-SS-PROD-29");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD29, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD30, "TP-SS-PROD-30");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD30, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-30");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD30, "TP-SS-PROD-30");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD30, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD31, "TP-SS-PROD-31");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD31, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-31");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD31, "TP-SS-PROD-31");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD31, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD32, "TP-SS-PROD-32");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD32, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-32");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD32, "TP-SS-PROD-32");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD32, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD33, "TP-SS-PROD-33");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD33, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-33");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD33, "TP-SS-PROD-33");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD33, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD34, "TP-SS-PROD-34");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD34, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-34");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD34, "TP-SS-PROD-34");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD34, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD35, "TP-SS-PROD-35");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD35, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-35");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD35, "TP-SS-PROD-35");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD35, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD36, "TP-SS-PROD-36");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD36, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-36");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD36, "TP-SS-PROD-36");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD36, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD37, "TP-SS-PROD-37");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD37, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-37");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD37, "TP-SS-PROD-37");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD37, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD38, "TP-SS-PROD-38");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD38, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-38");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD38, "TP-SS-PROD-38");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD38, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD39, "TP-SS-PROD-39");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD39, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-39");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD39, "TP-SS-PROD-39");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD39, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD40, "TP-SS-PROD-40");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD40, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-40");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD40, "TP-SS-PROD-40");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD40, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD41, "TP-SS-PROD-41");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD41, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-41");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD41, "TP-SS-PROD-41");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD41, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD42, "TP-SS-PROD-42");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD42, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-42");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD42, "TP-SS-PROD-42");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD42, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD43, "TP-SS-PROD-43");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD43, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-43");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD43, "TP-SS-PROD-43");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD43, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD44, "TP-SS-PROD-44");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD44, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-44");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD44, "TP-SS-PROD-44");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD44, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD45, "TP-SS-PROD-45");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD45, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-45");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD45, "TP-SS-PROD-45");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD45, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD46, "TP-SS-PROD-46");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD46, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-46");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD46, "TP-SS-PROD-46");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD46, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD47, "TP-SS-PROD-47");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD47, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-47");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD47, "TP-SS-PROD-47");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD47, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD48, "TP-SS-PROD-48");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD48, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-48");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD48, "TP-SS-PROD-48");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD48, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD49, "TP-SS-PROD-49");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD49, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-49");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD49, "TP-SS-PROD-49");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD49, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD50, "TP-SS-PROD-50");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD50, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-50");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD50, "TP-SS-PROD-50");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD50, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD51, "TP-SS-PROD-51");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD51, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-51");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD51, "TP-SS-PROD-51");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD51, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD52, "TP-SS-PROD-52");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD52, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-52");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD52, "TP-SS-PROD-52");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD52, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD53, "TP-SS-PROD-53");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD53, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-53");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD53, "TP-SS-PROD-53");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD53, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD54, "TP-SS-PROD-54");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD54, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-54");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD54, "TP-SS-PROD-54");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD54, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD55, "TP-SS-PROD-55");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD55, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-55");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD55, "TP-SS-PROD-55");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD55, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD56, "TP-SS-PROD-56");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD56, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-56");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD56, "TP-SS-PROD-56");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD56, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD57, "TP-SS-PROD-57");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD57, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-57");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD57, "TP-SS-PROD-57");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD57, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD58, "TP-SS-PROD-58");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD58, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-58");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD58, "TP-SS-PROD-58");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD58, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD59, "TP-SS-PROD-59");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD59, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-59");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD59, "TP-SS-PROD-59");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD59, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD60, "TP-SS-PROD-60");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD60, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-60");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD60, "TP-SS-PROD-60");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPROD60, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODOutlook, "TP-SS-PROD-Outlook");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODOutlook, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-Outlook");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODOutlook, "TP-SS-PROD-Outlook");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODOutlook, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODSequence, "TP-SS-PROD-Sequence");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODSequence, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-Sequence");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODSequence, "TP-SS-PROD-Sequence");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODSequence, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODArchive, "TP-SS-PROD-Archive");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODArchive, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-Archive");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODArchive, "TP-SS-PROD-Archive");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODArchive, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS1, "TP-SS-PROD-CMS1");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS1, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-CMS1");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS1, "TP-SS-PROD-CMS1");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS1, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS2, "TP-SS-PROD-CMS2");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS2, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-CMS2");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS2, "TP-SS-PROD-CMS2");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS2, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS3, "TP-SS-PROD-CMS3");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS3, @"\\FREDCPAPPDM0004\translateplusSystemFiles\ProcessAutomationTP-SS-PROD-CMS3");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS3, "TP-SS-PROD-CMS3");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSPRODCMS3, "");

                        TPProcessAutomationNonClientSpecific.AutomationServerComputerLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSDEV02, "WKWFR7890752");
                        TPProcessAutomationNonClientSpecific.AutomationServerNetworkFolderLocations.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSDEV02, @"\\FREDCPAPPDM0001\translateplusSystemFiles\ProcessAutomationTP-SS-DEV-02");
                        TPProcessAutomationNonClientSpecific.AutomationServerFunctionDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSDEV02, "TP-SS-DEV-02");
                        TPProcessAutomationNonClientSpecific.AutomationServerEmailSummaryDescriptions.Add(TPProcessAutomationNonClientSpecific.AutomationServers.TPSSDEV02, "");

                        DesignPlusTrackChangesDate = new DateTime(2015, 7, 27, 9, 0, 0);
                        DesignPlusTMIntegrationStartDate = new DateTime(2015, 7, 23, 0, 0, 0);
                        HighFiveImagesDirectoryPath = @"\\fredcpfspdm0003\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\High five pictures\";

                        // CurrentBrand = Brands.TP
                        iplusDownTimeXMLFilePath = @"S:\Sites\Translateplus\System config\iplusUpdateMessage.xml";
                        designPlusManualAlignmentTemplatePath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\design plus manual alignment template.xltx";
                        HRDoctorsCertificateFolderLocation = @"\\fredcpfspdm0003\SharedInfo\HR\ALL\7_Sickness\Doctor's certificates\";
                        LinguistSearchExportTempLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\Linguist search result export.xlt";
                        SicknessReportExportTempLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\Sickness report template.xltx";
                        ReferenceTemplatePath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\ReferenceTemplate.dot";
                        UnbatchedOrdersListReportLocation = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\Unbatched orders list report.xlt";
                        GDPRDocTemplatePath = @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\TP System Files\translateplus\SystemTemplates\GDPR template.docx";
                        break;
                    }
            }


            // go on to load other core system settings from the database (as of Aug 2011, to
            // enable us to set these via the database without needing to re-code/re-publish
            // applications)
            try
            {
                LoadSystemConfig();
            }
            catch
            {
            }
        }

    }
}
