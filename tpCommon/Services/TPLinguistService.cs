using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.LinguisticSupplier;

namespace Services
{
    public class TPLinguistService : ITPLinguistService
    {
        private readonly IRepository<LinguisticSupplier> supplierRepository;
        private readonly IRepository<LinguisticSupplierInvoiceJobItem> LSIrepository;
        private readonly IRepository<LinguisticSupplierInvoice> invoiceRepository;
        private readonly IRepository<LinguisticSupplierRate> LsRateRepository;
        private readonly IRepository<LocalCountryInfo> lcInfoRepo;
        private readonly IRepository<LinguisticSupplierStatus> lsStatusRepo;
        private readonly IRepository<LinguisticSupplierType> lsTypeRepo;
        private readonly IRepository<ExtranetUser> eUserRepo;
        private readonly IRepository<PaymentMethod> paymentRepo;
        private readonly IRepository<LocalCurrencyInfo> currencyRepo;
        private readonly IRepository<LinguisticSupplierSoftwareApplication> linguisticSupplierSoftwareApplicationRepo;

        public TPLinguistService(IRepository<LinguisticSupplier> supplierRepository,
            IRepository<LinguisticSupplierInvoiceJobItem> lSIrepository,
            IRepository<LinguisticSupplierInvoice> invoiceRepository,
            IRepository<LinguisticSupplierRate> lsRateRepository,
            IRepository<LocalCountryInfo> lcInfoRepo,
            IRepository<LinguisticSupplierStatus> lsStatusRepo,
            IRepository<LinguisticSupplierType> lsTypeRepo,
            IRepository<ExtranetUser> eUserRepo,
            IRepository<PaymentMethod> paymentRepo,
            IRepository<LocalCurrencyInfo> currencyRepo,
            IRepository<LinguisticSupplierSoftwareApplication> linguisticSupplierSoftwareApplicationRepo)
        {
            this.supplierRepository = supplierRepository;
            LSIrepository = lSIrepository;
            this.lcInfoRepo = lcInfoRepo;
            this.invoiceRepository = invoiceRepository;
            LsRateRepository = lsRateRepository;
            this.lsStatusRepo = lsStatusRepo;
            this.lsTypeRepo = lsTypeRepo;
            this.eUserRepo = eUserRepo;
            this.paymentRepo = paymentRepo;
            this.currencyRepo = currencyRepo;
            this.linguisticSupplierSoftwareApplicationRepo = linguisticSupplierSoftwareApplicationRepo;
        }

        public string getCountryName(int id)
        {
            string country = lcInfoRepo.All().Where(x => x.CountryId == id && x.LanguageIanacode == "en").Select(x => x.CountryName).SingleOrDefault();
            return country;
        }

        public string getStatus(int id)
        {
            string status = lsStatusRepo.All().Where(x => x.Id == id).Select(x => x.Name).SingleOrDefault();
            return status;
        }

        public string getType(int id)
        {
            string type = lsTypeRepo.All().Where(x => x.Id == id).Select(x => x.Name).SingleOrDefault();
            return type;
        }
        public string getCurrencyName(int id)
        {
            string type = currencyRepo.All().Where(x => x.CurrencyId == id && x.LanguageIanacode == "en").Select(x => x.CurrencyName).SingleOrDefault();
            return type;
        }

        public string getPaymentType(int id)
        {
            string type = paymentRepo.All().Where(x => x.Id == id).Select(x => x.Name).SingleOrDefault();
            return type;
        }
        public string getExtranetName(int id)
        {
            string eName = eUserRepo.All().Where(x => x.DataObjectId == id && x.DataObjectTypeId == 4).Select(x => x.UserName).SingleOrDefault();
            return eName;
        }



        public string getLastLogin(int id)
        {
            string eLogIn = eUserRepo.All().Where(x => x.DataObjectId == id && x.DataObjectTypeId == 4).Select(x => x.LastLoginDateTime.GetValueOrDefault().ToString("dd/M/yyyy HH:mm:ss")).SingleOrDefault();
            return eLogIn;
        }
        public async Task<LinguistViewModel> GetById(int id)
        {
            var linguist = await supplierRepository.All().Where(x => x.Id == id && x.DeletedDate == null)
                .Select(x => new LinguistViewModel()
                {
                    Id = x.Id,
                    SupplierTypeId = x.SupplierTypeId,
                    AgencyOrTeamName = x.AgencyOrTeamName,
                    MainContactSurname = x.MainContactSurname,
                    MainContactFirstName = x.MainContactFirstName,
                    MainContactGender = x.MainContactGender,
                    MainContactDob = x.MainContactDob,
                    MainContactNationalityCountryId = x.MainContactNationalityCountryId,
                    AgencyCompanyRegistrationNumber = x.AgencyCompanyRegistrationNumber,
                    AgencyNumberOfLinguists = x.AgencyNumberOfLinguists,
                    AgencyNumberOfDtporMultimediaOps = x.AgencyNumberOfDtporMultimediaOps,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    Address3 = x.Address3,
                    Address4 = x.Address4,
                    CountyOrState = x.CountyOrState,
                    PostcodeOrZip = x.PostcodeOrZip,
                    CountryId = x.CountryId,
                    MainLandlineCountryId = x.MainLandlineCountryId,
                    MainLandlineNumber = x.MainLandlineNumber,
                    SecondaryLandlineCountryId = x.SecondaryLandlineCountryId,
                    SecondaryLandlineNumber = x.SecondaryLandlineNumber,
                    SecondaryLandlineIsWorkNumber = x.SecondaryLandlineIsWorkNumber,
                    MobileCountryId = x.MobileCountryId,
                    MobileNumber = x.MobileNumber,
                    FaxCountryId = x.FaxCountryId,
                    FaxNumber = x.FaxNumber,
                    EmailAddress = x.EmailAddress,
                    SecondaryEmailAddress = x.SecondaryEmailAddress,
                    SecondaryEmailIsWorkAddress = x.SecondaryEmailIsWorkAddress,
                    SkypeId = x.SkypeId,
                    WebAddress = x.WebAddress,
                    Notes = x.Notes,
                    SupplierStatusId = x.SupplierStatusId,
                    SupplierSourceId = x.SupplierSourceId,
                    ApplicationFormSentToSupplierDate = x.ApplicationFormSentToSupplierDate,
                    ApplicationFormReceivedFromSupplierDate = x.ApplicationFormReceivedFromSupplierDate,
                    HasAccessToCar = x.HasAccessToCar,
                    WouldSignWitnessStatement = x.WouldSignWitnessStatement,
                    PrimaryOperatingSystemId = x.PrimaryOperatingSystemId,
                    PrimaryOperatingSystemVersionId = x.PrimaryOperatingSystemVersionId,
                    SubjectAreaSpecialismsAsDescribedBySupplier = x.SubjectAreaSpecialismsAsDescribedBySupplier,
                    MotherTongueLanguageIanacode = x.MotherTongueLanguageIanacode,
                    IfBilingualOtherMotherTongueIanacode = x.IfBilingualOtherMotherTongueIanacode,
                    CurrencyId = x.CurrencyId,
                    AgreedPaymentMethodId = x.AgreedPaymentMethodId,
                    Vatnumber = x.Vatnumber,
                    RatesNotes = x.RatesNotes,
                    MemoryRateForExactMatches = x.MemoryRateForExactMatches,
                    MemoryRateForRepetitions = x.MemoryRateForRepetitions,
                    MemoryRateFor95To99Percent = x.MemoryRateFor95To99Percent,
                    MemoryRateFor85To94Percent = x.MemoryRateFor85To94Percent,
                    MemoryRateFor75To84Percent = x.MemoryRateFor75To84Percent,
                    MemoryRateFor50To74Percent = x.MemoryRateFor50To74Percent,
                    MemoryRateForPerfectMatches = x.MemoryRateForPerfectMatches,
                    Referee1Name = x.Referee1Name,
                    Referee1FullAddress = x.Referee1FullAddress,
                    Referee1Phone = x.Referee1Phone,
                    Referee1EmailAddress = x.Referee1EmailAddress,
                    Referee2Name = x.Referee2Name,
                    Referee2FullAddress = x.Referee2FullAddress,
                    Referee2Phone = x.Referee2Phone,
                    Referee2EmailAddress = x.Referee2EmailAddress,
                    IsAclientInternalLinguist = x.IsAclientInternalLinguist,
                    CreatedDate = x.CreatedDate,
                    CreatedByEmployeeId = x.CreatedByEmployeeId,
                    LastModifiedDate = x.LastModifiedDate,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    DeletedDate = x.DeletedDate,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    NdaunlockedDateTime = x.NdaunlockedDateTime,
                    NdaunlockedByEmployeeId = x.NdaunlockedByEmployeeId,
                    NdauploadedDateTime = x.NdauploadedDateTime,
                    NdauploadedByEmployeeId = x.NdauploadedByEmployeeId,
                    LastExportedToSageDateTime = x.LastExportedToSageDateTime,
                    CurrencyOfFirstInvoiceExportedToSage = x.CurrencyOfFirstInvoiceExportedToSage,
                    HasEncryptedComputer = x.HasEncryptedComputer,
                    SupplierResponsivenessRating = x.SupplierResponsivenessRating,
                    SupplierFollowingTheInstructionsRating = x.SupplierFollowingTheInstructionsRating,
                    SupplierAttitudeRating = x.SupplierAttitudeRating,
                    SupplierQualityOfWorkRating = x.SupplierQualityOfWorkRating,
                    SupplierOnTimeDeliveryRating = x.SupplierOnTimeDeliveryRating,
                    OverallRating = x.OverallRating,
                    QualityRatingAvgForFirst2Jobs = x.QualityRatingAvgForFirst2Jobs,
                    IsQualityRatingPendingForFirst2Jobs = x.IsQualityRatingPendingForFirst2Jobs,
                    IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded = x.IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded,
                    Ftpenabled = x.Ftpenabled,
                    GdpracceptedDateTime = x.GdpracceptedDateTime,
                    Gdprstatus = x.Gdprstatus,
                    GdpraccetedViaIplus = x.GdpraccetedViaIplus,
                    GdprrejectedDateTime = x.GdprrejectedDateTime,
                    NeedApprovalToBeAddedToDb = x.NeedApprovalToBeAddedToDb,
                    ApprovedToAbeAddedtoDbdatetime = x.ApprovedToAbeAddedtoDbdatetime,
                    ApprovedToAbeAddedtoDbbyEmpId = x.ApprovedToAbeAddedtoDbbyEmpId,
                    NonEeaclauseAcceptedDateTime = x.NonEeaclauseAcceptedDateTime,
                    NonEeaclauseDeclinedDateTime = x.NonEeaclauseDeclinedDateTime,
                    SapmasterDataReferenceNumber = x.SapmasterDataReferenceNumber,
                    ContractUnlockedDateTime = x.ContractUnlockedDateTime,
                    ContractUnlockedByEmployeeId = x.ContractUnlockedByEmployeeId,
                    ContractUploadedDateTime = x.ContractUploadedDateTime,
                    ContractUploadedByEmployeeId = x.ContractUploadedByEmployeeId,
                    MasterFieldID = x.MasterFieldID,
                    SubFieldID = x.SubFieldID,
                    MediaTypeID = x.MediaTypeID,
                    VendorTypeID = x.VendorTypeID,
                    Referee1Received = x.Referee1Received,
                    Referee2Received = x.Referee2Received,
                    RateNotes = x.RatesNotes,
                    ProfessionalBodyID = x.ProfessionalBodyID
                }).FirstOrDefaultAsync();

            return linguist;
        }

        public async Task<InvoiceViewModel> GetSupplierInvoiceFromJobItemId(int jobItemId)
        {
            var invoiceId = await LSIrepository.All().Where(x => x.JobItemId == jobItemId).FirstOrDefaultAsync();

            if (invoiceId == null)
                return null;

            var invoice = await invoiceRepository.All().Where(x => x.Id == invoiceId.InvoiceId)
                .Select(x => new InvoiceViewModel()
                {
                    Id = x.Id,
                    LinguisticSupplierName = x.LinguisticSupplierName,
                    LinguisticSupplierId = x.LinguisticSupplierId,
                    InvoiceDate = x.InvoiceDate,
                    DueDate = x.DueDate,
                    PaidDate = x.PaidDate,
                    PaidMethod = x.PaidMethod,
                    Vatrate = x.Vatrate,
                    InvoiceLangIanacode = x.InvoiceLangIanacode,
                    InvoiceCurrencyId = x.InvoiceCurrencyId,
                    SubTotalValue = x.SubTotalValue,
                    DiscountValue = x.DiscountValue,
                    Vatvalue = x.Vatvalue,
                    Vatnumber = x.Vatnumber,
                    TotalValue = x.TotalValue,
                    Amount = x.Amount,
                    LinguisticSupplierInvoiceNumber = x.LinguisticSupplierInvoiceNumber,
                    NameToShowOnInvoice = x.NameToShowOnInvoice,
                    Address1ToShowOnInvoice = x.Address1ToShowOnInvoice,
                    Address2ToShowOnInvoice = x.Address2ToShowOnInvoice,
                    Address3ToShowOnInvoice = x.Address3ToShowOnInvoice,
                    Address4ToShowOnInvoice = x.Address4ToShowOnInvoice,
                    CountryToShowOnInvoice = x.CountryToShowOnInvoice,
                    EarlyPaymentOption = x.EarlyPaymentOption,
                    AgreedPaymentMethod = x.AgreedPaymentMethod,
                    BankBranchName = x.BankBranchName,
                    BankBranchAddress = x.BankBranchAddress,
                    BankBranchPostCode = x.BankBranchPostCode,
                    BankBranchCountry = x.BankBranchCountry,
                    BankBranchCity = x.BankBranchCity,
                    BankAccountName = x.BankAccountName,
                    BankAccountSortCode = x.BankAccountSortCode,
                    BankAccountRtnnumber = x.BankAccountRtnnumber,
                    BankAccountNumber = x.BankAccountNumber,
                    BankAccountSwiftorBic = x.BankAccountSwiftorBic,
                    BankAccountIban = x.BankAccountIban,
                    IntermediaryBankDetails = x.IntermediaryBankDetails,
                    AdditionalInformationForBank = x.AdditionalInformationForBank,
                    PayPalId = x.PayPalId,
                    SkrillId = x.SkrillId,
                    OptionalText1 = x.OptionalText1,
                    OptionalText2 = x.OptionalText2,
                    CreatedDateTime = x.CreatedDateTime,
                    CreatedByEmployee = x.CreatedByEmployee,
                    LastModifiedDateTime = x.LastModifiedDateTime,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    DeletedDateTime = x.DeletedDateTime,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    SupplierInvoiceHoldDateTime = x.SupplierInvoiceHoldDateTime,
                    SupplierInvoiceHoldByEmployee = x.SupplierInvoiceHoldByEmployee,
                    SupplierInvoiceOffHoldDateTime = x.SupplierInvoiceOffHoldDateTime,
                    SupplierInvoiceOffHoldByEmployee = x.SupplierInvoiceOffHoldByEmployee,
                    SupplierInvoiceOnHold = x.SupplierInvoiceOnHold,
                    ExportedToSageDateTime = x.ExportedToSageDateTime
                }).FirstOrDefaultAsync();

            return invoice;
        }

        public async Task<IEnumerable<LinguistViewModel>> SearchByNameOrId(string searchTerm, string sourceLangIanaCode, string targetLangIanaCode)
        {
            searchTerm = searchTerm.Trim();

            int id;
            var isId = int.TryParse(searchTerm, out id);

            string surname = searchTerm;
            if (searchTerm.Contains(" "))
                surname = searchTerm.Split(" ")[1];

            var res = await LsRateRepository.All()
                .Where(x => x.DeletedDate == null && x.SourceLangIanacode == sourceLangIanaCode && x.TargetLangIanacode == targetLangIanaCode)
                .Join(supplierRepository.All()
                .Where(x => x.Id == id ||
                (x.MainContactFirstName.Contains(searchTerm) ||
                x.MainContactSurname.Contains(surname) ||
                x.AgencyOrTeamName.Contains(searchTerm)) &&
                x.DeletedDate == null),
                rate => rate.SupplierId, supplier => supplier.Id, (rate, supplier) => new { rate, supplier })
                .Select(x => new LinguistViewModel()
                {
                    Id = x.supplier.Id,
                    SupplierTypeId = x.supplier.SupplierTypeId,
                    AgencyOrTeamName = x.supplier.AgencyOrTeamName,
                    MainContactSurname = x.supplier.MainContactSurname,
                    MainContactFirstName = x.supplier.MainContactFirstName,
                    MainContactGender = x.supplier.MainContactGender,
                    MainContactDob = x.supplier.MainContactDob,
                    MainContactNationalityCountryId = x.supplier.MainContactNationalityCountryId,
                    AgencyCompanyRegistrationNumber = x.supplier.AgencyCompanyRegistrationNumber,
                    AgencyNumberOfLinguists = x.supplier.AgencyNumberOfLinguists,
                    AgencyNumberOfDtporMultimediaOps = x.supplier.AgencyNumberOfDtporMultimediaOps,
                    Address1 = x.supplier.Address1,
                    Address2 = x.supplier.Address2,
                    Address3 = x.supplier.Address3,
                    Address4 = x.supplier.Address4,
                    CountyOrState = x.supplier.CountyOrState,
                    PostcodeOrZip = x.supplier.PostcodeOrZip,
                    CountryId = x.supplier.CountryId,
                    MainLandlineCountryId = x.supplier.MainLandlineCountryId,
                    MainLandlineNumber = x.supplier.MainLandlineNumber,
                    SecondaryLandlineCountryId = x.supplier.SecondaryLandlineCountryId,
                    SecondaryLandlineNumber = x.supplier.SecondaryLandlineNumber,
                    SecondaryLandlineIsWorkNumber = x.supplier.SecondaryLandlineIsWorkNumber,
                    MobileCountryId = x.supplier.MobileCountryId,
                    MobileNumber = x.supplier.MobileNumber,
                    FaxCountryId = x.supplier.FaxCountryId,
                    FaxNumber = x.supplier.FaxNumber,
                    EmailAddress = x.supplier.EmailAddress,
                    SecondaryEmailAddress = x.supplier.SecondaryEmailAddress,
                    SecondaryEmailIsWorkAddress = x.supplier.SecondaryEmailIsWorkAddress,
                    SkypeId = x.supplier.SkypeId,
                    WebAddress = x.supplier.WebAddress,
                    Notes = x.supplier.Notes,
                    SupplierStatusId = x.supplier.SupplierStatusId,
                    SupplierSourceId = x.supplier.SupplierSourceId,
                    ApplicationFormSentToSupplierDate = x.supplier.ApplicationFormSentToSupplierDate,
                    ApplicationFormReceivedFromSupplierDate = x.supplier.ApplicationFormReceivedFromSupplierDate,
                    HasAccessToCar = x.supplier.HasAccessToCar,
                    WouldSignWitnessStatement = x.supplier.WouldSignWitnessStatement,
                    PrimaryOperatingSystemId = x.supplier.PrimaryOperatingSystemId,
                    PrimaryOperatingSystemVersionId = x.supplier.PrimaryOperatingSystemVersionId,
                    SubjectAreaSpecialismsAsDescribedBySupplier = x.supplier.SubjectAreaSpecialismsAsDescribedBySupplier,
                    MotherTongueLanguageIanacode = x.supplier.MotherTongueLanguageIanacode,
                    IfBilingualOtherMotherTongueIanacode = x.supplier.IfBilingualOtherMotherTongueIanacode,
                    CurrencyId = x.supplier.CurrencyId,
                    AgreedPaymentMethodId = x.supplier.AgreedPaymentMethodId,
                    Vatnumber = x.supplier.Vatnumber,
                    RatesNotes = x.supplier.RatesNotes,
                    MemoryRateForExactMatches = x.supplier.MemoryRateForExactMatches,
                    MemoryRateForRepetitions = x.supplier.MemoryRateForRepetitions,
                    MemoryRateFor95To99Percent = x.supplier.MemoryRateFor95To99Percent,
                    MemoryRateFor85To94Percent = x.supplier.MemoryRateFor85To94Percent,
                    MemoryRateFor75To84Percent = x.supplier.MemoryRateFor75To84Percent,
                    MemoryRateFor50To74Percent = x.supplier.MemoryRateFor50To74Percent,
                    MemoryRateForPerfectMatches = x.supplier.MemoryRateForPerfectMatches,
                    Referee1Name = x.supplier.Referee1Name,
                    Referee1FullAddress = x.supplier.Referee1FullAddress,
                    Referee1Phone = x.supplier.Referee1Phone,
                    Referee1EmailAddress = x.supplier.Referee1EmailAddress,
                    Referee2Name = x.supplier.Referee2Name,
                    Referee2FullAddress = x.supplier.Referee2FullAddress,
                    Referee2Phone = x.supplier.Referee2Phone,
                    Referee2EmailAddress = x.supplier.Referee2EmailAddress,
                    IsAclientInternalLinguist = x.supplier.IsAclientInternalLinguist,
                    CreatedDate = x.supplier.CreatedDate,
                    CreatedByEmployeeId = x.supplier.CreatedByEmployeeId,
                    LastModifiedDate = x.supplier.LastModifiedDate,
                    LastModifiedByEmployeeId = x.supplier.LastModifiedByEmployeeId,
                    DeletedDate = x.supplier.DeletedDate,
                    DeletedByEmployeeId = x.supplier.DeletedByEmployeeId,
                    NdaunlockedDateTime = x.supplier.NdaunlockedDateTime,
                    NdaunlockedByEmployeeId = x.supplier.NdaunlockedByEmployeeId,
                    NdauploadedDateTime = x.supplier.NdauploadedDateTime,
                    NdauploadedByEmployeeId = x.supplier.NdauploadedByEmployeeId,
                    LastExportedToSageDateTime = x.supplier.LastExportedToSageDateTime,
                    CurrencyOfFirstInvoiceExportedToSage = x.supplier.CurrencyOfFirstInvoiceExportedToSage,
                    HasEncryptedComputer = x.supplier.HasEncryptedComputer,
                    SupplierResponsivenessRating = x.supplier.SupplierResponsivenessRating,
                    SupplierFollowingTheInstructionsRating = x.supplier.SupplierFollowingTheInstructionsRating,
                    SupplierAttitudeRating = x.supplier.SupplierAttitudeRating,
                    SupplierQualityOfWorkRating = x.supplier.SupplierQualityOfWorkRating,
                    SupplierOnTimeDeliveryRating = x.supplier.SupplierOnTimeDeliveryRating,
                    OverallRating = x.supplier.OverallRating,
                    QualityRatingAvgForFirst2Jobs = x.supplier.QualityRatingAvgForFirst2Jobs,
                    IsQualityRatingPendingForFirst2Jobs = x.supplier.IsQualityRatingPendingForFirst2Jobs,
                    IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded = x.supplier.IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded,
                    Ftpenabled = x.supplier.Ftpenabled,
                    GdpracceptedDateTime = x.supplier.GdpracceptedDateTime,
                    Gdprstatus = x.supplier.Gdprstatus,
                    GdpraccetedViaIplus = x.supplier.GdpraccetedViaIplus,
                    GdprrejectedDateTime = x.supplier.GdprrejectedDateTime,
                    NeedApprovalToBeAddedToDb = x.supplier.NeedApprovalToBeAddedToDb,
                    ApprovedToAbeAddedtoDbdatetime = x.supplier.ApprovedToAbeAddedtoDbdatetime,
                    ApprovedToAbeAddedtoDbbyEmpId = x.supplier.ApprovedToAbeAddedtoDbbyEmpId,
                    NonEeaclauseAcceptedDateTime = x.supplier.NonEeaclauseAcceptedDateTime,
                    NonEeaclauseDeclinedDateTime = x.supplier.NonEeaclauseDeclinedDateTime,
                    SapmasterDataReferenceNumber = x.supplier.SapmasterDataReferenceNumber,
                    ContractUnlockedDateTime = x.supplier.ContractUnlockedDateTime,
                    ContractUnlockedByEmployeeId = x.supplier.ContractUnlockedByEmployeeId,
                    ContractUploadedDateTime = x.supplier.ContractUploadedDateTime,
                    ContractUploadedByEmployeeId = x.supplier.ContractUploadedByEmployeeId
                })
                .Distinct()
                .OrderBy(x => x.MainContactFirstName)
                .ThenBy(x => x.MainContactSurname)
                .ThenBy(x => x.AgencyOrTeamName)
                .ToListAsync();

            return res;
        }

        public async Task<IEnumerable<LinguistViewModel>> SearchByNameOrIdOnly(string searchTerm)
        {
            searchTerm = searchTerm.Trim();

            int id;
            var isId = int.TryParse(searchTerm, out id);

            string surname = searchTerm;
            if (searchTerm.Contains(" "))
                surname = searchTerm.Split(" ")[1];

            //var res = await LsRateRepository.All()
            //    .Where(x => x.DeletedDate == null)
            //    .Join(supplierRepository.All()

            var res = await supplierRepository.All()
             .Where(x => x.Id == id ||
             (x.MainContactFirstName.Contains(searchTerm) ||
             x.MainContactSurname.Contains(surname) ||
             x.AgencyOrTeamName.Contains(searchTerm)) &&
             x.DeletedDate == null)//,
                                   //rate => rate.SupplierId, supplier => supplier.Id, (rate, supplier) => new { rate, supplier }).DefaultIfEmpty()
             .Select(x => new LinguistViewModel()
             {
                 Id = x.Id,
                 SupplierTypeId = x.SupplierTypeId,
                 AgencyOrTeamName = x.AgencyOrTeamName,
                 MainContactSurname = x.MainContactSurname,
                 MainContactFirstName = x.MainContactFirstName,
                 MainContactGender = x.MainContactGender,
                 MainContactDob = x.MainContactDob,
                 MainContactNationalityCountryId = x.MainContactNationalityCountryId,
                 AgencyCompanyRegistrationNumber = x.AgencyCompanyRegistrationNumber,
                 AgencyNumberOfLinguists = x.AgencyNumberOfLinguists,
                 AgencyNumberOfDtporMultimediaOps = x.AgencyNumberOfDtporMultimediaOps,
                 Address1 = x.Address1,
                 Address2 = x.Address2,
                 Address3 = x.Address3,
                 Address4 = x.Address4,
                 CountyOrState = x.CountyOrState,
                 PostcodeOrZip = x.PostcodeOrZip,
                 CountryId = x.CountryId,
                 MainLandlineCountryId = x.MainLandlineCountryId,
                 MainLandlineNumber = x.MainLandlineNumber,
                 SecondaryLandlineCountryId = x.SecondaryLandlineCountryId,
                 SecondaryLandlineNumber = x.SecondaryLandlineNumber,
                 SecondaryLandlineIsWorkNumber = x.SecondaryLandlineIsWorkNumber,
                 MobileCountryId = x.MobileCountryId,
                 MobileNumber = x.MobileNumber,
                 FaxCountryId = x.FaxCountryId,
                 FaxNumber = x.FaxNumber,
                 EmailAddress = x.EmailAddress,
                 SecondaryEmailAddress = x.SecondaryEmailAddress,
                 SecondaryEmailIsWorkAddress = x.SecondaryEmailIsWorkAddress,
                 SkypeId = x.SkypeId,
                 WebAddress = x.WebAddress,
                 Notes = x.Notes,
                 SupplierStatusId = x.SupplierStatusId,
                 SupplierSourceId = x.SupplierSourceId,
                 ApplicationFormSentToSupplierDate = x.ApplicationFormSentToSupplierDate,
                 ApplicationFormReceivedFromSupplierDate = x.ApplicationFormReceivedFromSupplierDate,
                 HasAccessToCar = x.HasAccessToCar,
                 WouldSignWitnessStatement = x.WouldSignWitnessStatement,
                 PrimaryOperatingSystemId = x.PrimaryOperatingSystemId,
                 PrimaryOperatingSystemVersionId = x.PrimaryOperatingSystemVersionId,
                 SubjectAreaSpecialismsAsDescribedBySupplier = x.SubjectAreaSpecialismsAsDescribedBySupplier,
                 MotherTongueLanguageIanacode = x.MotherTongueLanguageIanacode,
                 IfBilingualOtherMotherTongueIanacode = x.IfBilingualOtherMotherTongueIanacode,
                 CurrencyId = x.CurrencyId,
                 AgreedPaymentMethodId = x.AgreedPaymentMethodId,
                 Vatnumber = x.Vatnumber,
                 RatesNotes = x.RatesNotes,
                 MemoryRateForExactMatches = x.MemoryRateForExactMatches,
                 MemoryRateForRepetitions = x.MemoryRateForRepetitions,
                 MemoryRateFor95To99Percent = x.MemoryRateFor95To99Percent,
                 MemoryRateFor85To94Percent = x.MemoryRateFor85To94Percent,
                 MemoryRateFor75To84Percent = x.MemoryRateFor75To84Percent,
                 MemoryRateFor50To74Percent = x.MemoryRateFor50To74Percent,
                 MemoryRateForPerfectMatches = x.MemoryRateForPerfectMatches,
                 Referee1Name = x.Referee1Name,
                 Referee1FullAddress = x.Referee1FullAddress,
                 Referee1Phone = x.Referee1Phone,
                 Referee1EmailAddress = x.Referee1EmailAddress,
                 Referee2Name = x.Referee2Name,
                 Referee2FullAddress = x.Referee2FullAddress,
                 Referee2Phone = x.Referee2Phone,
                 Referee2EmailAddress = x.Referee2EmailAddress,
                 IsAclientInternalLinguist = x.IsAclientInternalLinguist,
                 CreatedDate = x.CreatedDate,
                 CreatedByEmployeeId = x.CreatedByEmployeeId,
                 LastModifiedDate = x.LastModifiedDate,
                 LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                 DeletedDate = x.DeletedDate,
                 DeletedByEmployeeId = x.DeletedByEmployeeId,
                 NdaunlockedDateTime = x.NdaunlockedDateTime,
                 NdaunlockedByEmployeeId = x.NdaunlockedByEmployeeId,
                 NdauploadedDateTime = x.NdauploadedDateTime,
                 NdauploadedByEmployeeId = x.NdauploadedByEmployeeId,
                 LastExportedToSageDateTime = x.LastExportedToSageDateTime,
                 CurrencyOfFirstInvoiceExportedToSage = x.CurrencyOfFirstInvoiceExportedToSage,
                 HasEncryptedComputer = x.HasEncryptedComputer,
                 SupplierResponsivenessRating = x.SupplierResponsivenessRating,
                 SupplierFollowingTheInstructionsRating = x.SupplierFollowingTheInstructionsRating,
                 SupplierAttitudeRating = x.SupplierAttitudeRating,
                 SupplierQualityOfWorkRating = x.SupplierQualityOfWorkRating,
                 SupplierOnTimeDeliveryRating = x.SupplierOnTimeDeliveryRating,
                 OverallRating = x.OverallRating,
                 QualityRatingAvgForFirst2Jobs = x.QualityRatingAvgForFirst2Jobs,
                 IsQualityRatingPendingForFirst2Jobs = x.IsQualityRatingPendingForFirst2Jobs,
                 IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded = x.IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded,
                 Ftpenabled = x.Ftpenabled,
                 GdpracceptedDateTime = x.GdpracceptedDateTime,
                 Gdprstatus = x.Gdprstatus,
                 GdpraccetedViaIplus = x.GdpraccetedViaIplus,
                 GdprrejectedDateTime = x.GdprrejectedDateTime,
                 NeedApprovalToBeAddedToDb = x.NeedApprovalToBeAddedToDb,
                 ApprovedToAbeAddedtoDbdatetime = x.ApprovedToAbeAddedtoDbdatetime,
                 ApprovedToAbeAddedtoDbbyEmpId = x.ApprovedToAbeAddedtoDbbyEmpId,
                 NonEeaclauseAcceptedDateTime = x.NonEeaclauseAcceptedDateTime,
                 NonEeaclauseDeclinedDateTime = x.NonEeaclauseDeclinedDateTime,
                 SapmasterDataReferenceNumber = x.SapmasterDataReferenceNumber,
                 ContractUnlockedDateTime = x.ContractUnlockedDateTime,
                 ContractUnlockedByEmployeeId = x.ContractUnlockedByEmployeeId,
                 ContractUploadedDateTime = x.ContractUploadedDateTime,
                 ContractUploadedByEmployeeId = x.ContractUploadedByEmployeeId
             })
             .Distinct()
             .OrderBy(x => x.MainContactFirstName)
             .ThenBy(x => x.MainContactSurname)
             .ThenBy(x => x.AgencyOrTeamName)
             .ToListAsync();

            return res;
        }

        public async Task<LinguistViewModel> Update(LinguistViewModel supplier, short EmployeCurrentlyLoggedInID)
        {
            var dbLinguist = await supplierRepository.All().Where(x => x.Id == supplier.Id).FirstOrDefaultAsync();

            if (dbLinguist == null)
                return supplier;

            if (supplier.SectionToUpdate == "KeyInfo")
            {
                dbLinguist.MainContactFirstName = supplier.MainContactFirstName;
                dbLinguist.MainContactSurname = supplier.MainContactSurname;
                dbLinguist.CountryId = supplier.CountryId;
                dbLinguist.SupplierStatusId = supplier.SupplierStatusId;
                dbLinguist.SupplierTypeId = supplier.SupplierTypeId;
                dbLinguist.LastModifiedByEmployeeId = EmployeCurrentlyLoggedInID;
                dbLinguist.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
                dbLinguist.VendorTypeID = supplier.VendorTypeID;
                dbLinguist.AgencyOrTeamName = supplier.AgencyOrTeamName;
            }
            else if (supplier.SectionToUpdate == "NotesForm")
            {
                dbLinguist.Notes = supplier.Notes;
                dbLinguist.RatesNotes = supplier.RatesNotes;
                dbLinguist.LastModifiedByEmployeeId = EmployeCurrentlyLoggedInID;
                dbLinguist.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            }
            else if (supplier.SectionToUpdate == "PersonalForm")
            {
                dbLinguist.AgencyCompanyRegistrationNumber = supplier.AgencyCompanyRegistrationNumber;
                dbLinguist.AgencyNumberOfLinguists = supplier.AgencyNumberOfLinguists;
                dbLinguist.AgencyNumberOfDtporMultimediaOps = supplier.AgencyNumberOfDtporMultimediaOps;
                dbLinguist.MainContactGender = supplier.MainContactGender;
                dbLinguist.Address1 = supplier.Address1;
                dbLinguist.CountyOrState = supplier.CountyOrState;
                dbLinguist.MainContactNationalityCountryId = supplier.MainContactNationalityCountryId;
                dbLinguist.EmailAddress = supplier.EmailAddress;
                dbLinguist.SecondaryEmailAddress = supplier.SecondaryEmailAddress;
                dbLinguist.SkypeId = supplier.SkypeId;
                dbLinguist.MainLandlineNumber = supplier.MainLandlineNumber;
                dbLinguist.SecondaryLandlineNumber = supplier.SecondaryLandlineNumber;
                dbLinguist.MobileNumber = supplier.MobileNumber;
                if (supplier.WouldSignWitnessStatementString == "true")
                {
                    dbLinguist.WouldSignWitnessStatement = true;
                }
                else
                {
                    dbLinguist.WouldSignWitnessStatement = false;
                }
                dbLinguist.LastModifiedByEmployeeId = EmployeCurrentlyLoggedInID;
                dbLinguist.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
                dbLinguist.MainLandlineCountryId = supplier.MainLandlineCountryId;
                dbLinguist.SecondaryLandlineCountryId = supplier.SecondaryLandlineCountryId;
                dbLinguist.MobileCountryId = supplier.MobileCountryId;
            }
            else if (supplier.SectionToUpdate == "RefereesForm")
            {
                dbLinguist.Referee1Name = supplier.Referee1Name;
                dbLinguist.Referee1Phone = supplier.Referee1Phone;
                dbLinguist.Referee1EmailAddress = supplier.Referee1EmailAddress;
                dbLinguist.Referee2Name = supplier.Referee2Name;
                dbLinguist.Referee2Phone = supplier.Referee2Phone;
                dbLinguist.Referee2EmailAddress = supplier.Referee2EmailAddress;
                dbLinguist.LastModifiedByEmployeeId = EmployeCurrentlyLoggedInID;
                dbLinguist.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
                if (supplier.Referee1ReceivedString == "true")
                {
                    dbLinguist.Referee1Received = true;
                }
                else
                {
                    dbLinguist.Referee1Received = false;
                }
                if (supplier.Referee2ReceivedString == "true")
                {
                    dbLinguist.Referee2Received = true;
                }
                else
                {
                    dbLinguist.Referee2Received = false;
                }
            }
            else if (supplier.SectionToUpdate == "NDA")
            {
                dbLinguist.NdauploadedByEmployeeId = EmployeCurrentlyLoggedInID;
                dbLinguist.NdauploadedDateTime = GeneralUtils.GetCurrentUKTime();
            }
            else if (supplier.SectionToUpdate == "Contract")
            {
                dbLinguist.ContractUploadedByEmployeeId = EmployeCurrentlyLoggedInID;
                dbLinguist.ContractUploadedDateTime = GeneralUtils.GetCurrentUKTime();
            }
            await supplierRepository.SaveChangesAsync();

            return supplier;
        }

        public async Task<List<ViewModels.LinguisticSupplier.LinguistRateResults>> GetLinguistRates(string lingId)
        {

            var res = new List<ViewModels.LinguisticSupplier.LinguistRateResults>();
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"select LinguisticSupplierRates.id, LanguageServices.Name, LS.Name,
LT.Name,LanguageSubjectAreas.Name, LocalCurrencyInfo.CurrencyName,
cast(LinguisticSupplierRates.StandardRate as nvarchar), LanguageRateUnits.Name, cast(LinguisticSupplierRates.MinimumCharge as nvarchar),
case when LinguisticSupplierRates.AppliesToDataObjectTypeID = 1 then  cast(LinguisticSupplierRates.AppliesToDataObjectID as nvarchar) + ' - ' + CA.Name 
when LinguisticSupplierRates.AppliesToDataObjectTypeID = 2 then cast(LinguisticSupplierRates.AppliesToDataObjectID as nvarchar) + ' - ' + OA.OrgName 
when LinguisticSupplierRates.AppliesToDataObjectTypeID = 3 then cast(LinguisticSupplierRates.AppliesToDataObjectID as nvarchar) + ' - ' + OGA.Name 
else 'any translate plus client ' end, isnull(LinguisticSupplierRates.RatesNotes,''), isnull(cast(LinguisticSupplierRates.LastModifiedDate as nvarchar),''), isnull(AppliesToDataObjectID,0), isnull(AppliesToDataObjectTypeID,0)
,  isnull(MemoryRateForPerfectMatches,0.9), isnull(MemoryRateForExactMatches,0.9),isnull(MemoryRateForRepetitions,0.9)
,isnull(MemoryRateFor95To99Percent,0.9),isnull(MemoryRateFor85To94Percent,0.9),isnull(MemoryRateFor75To84Percent,0.9),isnull(MemoryRateFor50To74Percent,0.9),isnull(MemoryRateForClientSpecificPercent,0.9)
from LinguisticSupplierRates 
left outer join LanguageServices on LinguisticSupplierRates.LanguageServiceID = LanguageServices.ID
left outer join LocalLanguageInfo LS on LS.LanguageIANAcodeBeingDescribed = LinguisticSupplierRates.SourceLangIANAcode and LS.LanguageIANAcode = 'en'
left outer join LocalLanguageInfo LT on LT.LanguageIANAcodeBeingDescribed = LinguisticSupplierRates.TargetLangIANAcode and LT.LanguageIANAcode = 'en'
left outer join LocalCurrencyInfo on LocalCurrencyInfo.CurrencyID = LinguisticSupplierRates.CurrencyID
left outer join LanguageSubjectAreas on LanguageSubjectAreas.ID = LinguisticSupplierRates.SubjectAreaID
left outer join LanguageRateUnits on LanguageRateUnits.ID = LinguisticSupplierRates.RateUnitID
left outer join Contacts CA on CA.ID = LinguisticSupplierRates.AppliesToDataObjectID and LinguisticSupplierRates.AppliesToDataObjectTypeID = 1
left outer join Orgs OA on OA.ID = LinguisticSupplierRates.AppliesToDataObjectID and LinguisticSupplierRates.AppliesToDataObjectTypeID = 2
left outer join OrgGroups OGA on OGA.ID = LinguisticSupplierRates.AppliesToDataObjectID and LinguisticSupplierRates.AppliesToDataObjectTypeID = 3
where LinguisticSupplierRates.DeletedDate is null and SupplierID  = " + lingId + @"");

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.LinguisticSupplier.LinguistRateResults()
                    {
                        linguistRateID = await result.GetFieldValueAsync<int>(0),
                        serviceName = await result.GetFieldValueAsync<string>(1),
                        lingSourceLang = await result.GetFieldValueAsync<string>(2),
                        lingTargetLang = await result.GetFieldValueAsync<string>(3),
                        lingSubjectArea = await result.GetFieldValueAsync<string>(4),
                        lingCurrency = await result.GetFieldValueAsync<string>(5),
                        standardRate = await result.GetFieldValueAsync<string>(6),
                        linguistRateRateUnit = await result.GetFieldValueAsync<string>(7),
                        minimumCharge = await result.GetFieldValueAsync<string>(8),
                        appliesTo = await result.GetFieldValueAsync<string>(9),
                        rateNotes = await result.GetFieldValueAsync<string>(10),
                        lastModified = await result.GetFieldValueAsync<string>(11),
                        appliesToId = await result.GetFieldValueAsync<int>(12),
                        appliesToTypeID = await result.GetFieldValueAsync<byte>(13),
                        MemoryRateForPerfectMatches = await result.GetFieldValueAsync<decimal>(14),
                        MemoryRateForExactMatches = await result.GetFieldValueAsync<decimal>(15),
                        MemoryRateForRepetitions = await result.GetFieldValueAsync<decimal>(16),
                        MemoryRateFor95To99Percent = await result.GetFieldValueAsync<decimal>(17),
                        MemoryRateFor85To94Percent = await result.GetFieldValueAsync<decimal>(18),
                        MemoryRateFor75To84Percent = await result.GetFieldValueAsync<decimal>(19),
                        MemoryRateFor50To74Percent = await result.GetFieldValueAsync<decimal>(20),
                        MemoryRateForClientSpecificPercent = await result.GetFieldValueAsync<decimal>(21),
                    });
                }
            }
            return res;
        }

        public async Task<List<ViewModels.LinguisticSupplier.LinguistTop5Words>> GetLinguistTop5Words(string lingId)
        {

            var res = new List<ViewModels.LinguisticSupplier.LinguistTop5Words>();
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"		declare @AllWordsCount int

		set @AllWordsCount = (select SUM(WordCountNew +  WordCountFuzzyBand1 + ISNULL(WordCountFuzzyBand2, 0) + ISNULL(WordCountFuzzyBand3, 0) + ISNULL(WordCountFuzzyBand4, 0) + WordCountExact + WordCountRepetitions + WordCountPerfectMatches) from Jobitems
		inner join joborders on jobitems.joborderid = joborders.id
inner join contacts on joborders.contactid = contacts.id
inner join orgs on orgs.id = contacts.OrgId
where SupplierIsClientReviewer = 0 and LinguisticSupplierOrClientReviewerID = " + lingId + @" and DeletedDateTime is null)

select top 5 orgs.OrgName,  isnull(SUM(WordCountNew +  WordCountFuzzyBand1 + ISNULL(WordCountFuzzyBand2, 0) + ISNULL(WordCountFuzzyBand3, 0) + ISNULL(WordCountFuzzyBand4, 0) + WordCountExact + WordCountRepetitions + WordCountPerfectMatches),0) AS TotalWordsByOrg , isnull(@AllWordsCount,0) as TotalWords, orgs.ID from jobitems 
inner join joborders on jobitems.joborderid = joborders.id
inner join contacts on joborders.contactid = contacts.id
inner join orgs on orgs.id = contacts.OrgId
where SupplierIsClientReviewer = 0 and LinguisticSupplierOrClientReviewerID = " + lingId + @" and DeletedDateTime is null
group by orgs.OrgName, orgs.id
order by TotalWordsByOrg desc");

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.LinguisticSupplier.LinguistTop5Words()
                    {
                        numberOfOrgWords = await result.GetFieldValueAsync<int>(1),
                        orgName = await result.GetFieldValueAsync<string>(0),
                        TotalNumberOfWords = await result.GetFieldValueAsync<int>(2),
                        orgID = await result.GetFieldValueAsync<int>(3)

                    });
                }
            }
            return res;
        }
        public async Task<List<ViewModels.LinguisticSupplier.LinguistTop5Items>> GetLinguistTop5Items(string lingId)
        {

            var res = new List<ViewModels.LinguisticSupplier.LinguistTop5Items>();
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"		declare @allItems int
	set @allitems = (select count(jobitems.id) from jobitems inner join joborders on jobitems.joborderid = joborders.id
inner join contacts on joborders.contactid = contacts.id
inner join orgs on orgs.id = contacts.OrgId
where SupplierIsClientReviewer = 0 and LinguisticSupplierOrClientReviewerID = " + lingId + @" and DeletedDateTime is null )
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	select top 5 orgs.OrgName, isnull(count(jobitems.id) ,0) as jobitemscount, @allitems as 'allJobItems', orgs.id from jobitems 
inner join joborders on jobitems.joborderid = joborders.id
inner join contacts on joborders.contactid = contacts.id
inner join orgs on orgs.id = contacts.OrgId
where SupplierIsClientReviewer = 0 and LinguisticSupplierOrClientReviewerID = " + lingId + @" and DeletedDateTime is null
group by orgs.OrgName, orgs.id
order by jobitemscount desc ");

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.LinguisticSupplier.LinguistTop5Items()
                    {
                        numberOfOrgItems = await result.GetFieldValueAsync<int>(1),
                        orgName = await result.GetFieldValueAsync<string>(0),
                        TotalNumberOfItems = await result.GetFieldValueAsync<int>(2),
                        orgID = await result.GetFieldValueAsync<int>(3)

                    });
                }
            }
            return res;
        }
        public async Task<List<ViewModels.LinguisticSupplier.LinguistItems>> GetLinguistItems(string lingId)
        {

            var res = new List<ViewModels.LinguisticSupplier.LinguistItems>();
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                

                command.CommandText = string.Format(@"select JobItems.id, orgs.orgname, Contacts.Name, JobOrders.JobName, LanguageServices.Name, isnull(LS.Name,''), isnull(lt.Name,''),
isnull(jobitems.PaymentToSupplier,0), isnull(cast(jobitems.MarginAfterDiscountSurcharges as decimal(20,2)), 0),
orgs.id, contacts.id, joborders.id,
case 
    when jobitems.SupplierSentWorkDateTime is null 
    then 
	   case 
	       when JobItems.LanguageServiceID = 46 
		     then
			 case
			 when JobItems.WeCompletedItemDateTime is null 
		     then ''
             else 'Fully completed ' + FORMAT (JobItems.WeCompletedItemDateTime, 'dddd d MMM yyyy hh:mm')
		     end
		else
		'Not yet sent to supplier'
		end
	else		
         case 
		   when JobItems.SupplierCompletedItemDateTime is null 
           then
		      case
			     when jobitems.SupplierAcceptedWorkDateTime is null 
				 then 'Not yet accepted by supplier'
				 else
                      case 
					  when dbo.funcGetCurrentUKTime() < jobitems.SupplierCompletionDeadline 
					  then 'In progress with supplier' 
                      else
					       case
						   when dbo.funcGetCurrentUKTime() < jobitems.OurCompletionDeadline 
						   then 'Supplier is overdue, but we are still within the client deadline' 
                           else 'Supplier is overdue, and our client deadline is overdue too' 
						   end	
					  end
               end
		else	
             case 
			   when jobitems.WeCompletedItemDateTime is null 
			   then
                  case 
				  when dbo.funcGetCurrentUKTime() < jobitems.OurCompletionDeadline 
				  then CONCAT('Supplier completed ' + FORMAT (JobItems.SupplierCompletedItemDateTime, 'dddd d MMM yyyy hh:mm') , '; we have not completed for client yet')
                  else CONCAT('Supplier completed ' + FORMAT (JobItems.SupplierCompletedItemDateTime, 'dddd d MMM yyyy hh:mm') , ' but our client deadline is overdue')     
				end
				else
                'Fully completed ' + FORMAT (JobItems.WeCompletedItemDateTime, 'dddd d MMM yyyy hh:mm')
				end
		end

end
,
case 
    when jobitems.SupplierSentWorkDateTime is null 
    then 
	   case 
	       when JobItems.LanguageServiceID = 46 
		     then
			 case
			 when JobItems.WeCompletedItemDateTime is null 
		     then ''
             else 'green'
		     end
		else
		''
		end
	else		
         case 
		   when JobItems.SupplierCompletedItemDateTime is null 
           then
		      case
			     when jobitems.SupplierAcceptedWorkDateTime is null 
				 then ''
				 else
                      case 
					  when dbo.funcGetCurrentUKTime() < jobitems.SupplierCompletionDeadline 
					  then '' 
                      else
					       case
						   when dbo.funcGetCurrentUKTime() < jobitems.OurCompletionDeadline 
						   then '' 
                           else 'red' 
						   end	
					  end
               end
		else	
             case 
			   when jobitems.WeCompletedItemDateTime is null 
			   then
                  case 
				  when dbo.funcGetCurrentUKTime() < jobitems.OurCompletionDeadline 
				  then ''
                  else 'red'   
				end
				else
                'green'
				end
		end

end
from JobItems
left outer join JobOrders on JobItems.JobOrderID = JobOrders.ID
left outer join Contacts on joborders.ContactID = Contacts.ID
left outer join Orgs on orgs.ID = Contacts.OrgID
left outer join LocalLanguageInfo LS on LS.LanguageIANAcodeBeingDescribed = JobItems.SourceLanguageIANAcode and LS.LanguageIANAcode = 'en'
left outer join LocalLanguageInfo LT on LT.LanguageIANAcodeBeingDescribed = JobItems.TargetLanguageIANAcode and LT.LanguageIANAcode = 'en'
left outer join LanguageServices on LanguageServices.id  = jobitems.LanguageServiceID
where SupplierIsClientReviewer = 0 and JobItems.DeletedDateTime is null
and LinguisticSupplierOrClientReviewerID = " + lingId + @"");

                context.Database.OpenConnection();
               
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.LinguisticSupplier.LinguistItems()
                    {
                        itemID = await result.GetFieldValueAsync<int>(0),
                        orgName = await result.GetFieldValueAsync<string>(1),
                        contactName = await result.GetFieldValueAsync<string>(2),
                        orderName = await result.GetFieldValueAsync<string>(3),
                        itemService = await result.GetFieldValueAsync<string>(4),
                        itemSource = await result.GetFieldValueAsync<string>(5),
                        itemTarget = await result.GetFieldValueAsync<string>(6),
                        itemPaymentToSupplier = await result.GetFieldValueAsync<decimal>(7),
                        itemMargin = await result.GetFieldValueAsync<decimal>(8),
                        orgID = await result.GetFieldValueAsync<int>(9),
                        contactID = await result.GetFieldValueAsync<int>(10),
                        orderID = await result.GetFieldValueAsync<int>(11),
                        itemStatus = await result.GetFieldValueAsync<string>(12),
                        itemStatusColor = await result.GetFieldValueAsync<string>(13)
                    });
                }
            }
            return res;
        }
        public async Task<List<ViewModels.LinguisticSupplier.LinguistApprovedOrgs>> GetApprovedOrgs(string lingId)
        {

            var res = new List<ViewModels.LinguisticSupplier.LinguistApprovedOrgs>();
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"SELECT distinct AppliesToDataObjectID , orgs.OrgName 				
 FROM ApprovedOrBlockedLinguisticSuppliers
 inner join orgs on orgs.id = AppliesToDataObjectID
 WHERE	AppliesToDataObjectTypeID = 2
and ApprovedOrBlockedLinguisticSuppliers.Status in (1,2) and LinguisticSupplierID = " + lingId + @"");

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.LinguisticSupplier.LinguistApprovedOrgs()
                    {
                        orgID = await result.GetFieldValueAsync<int>(0),
                        orgName = await result.GetFieldValueAsync<string>(1),

                    });
                }
            }
            return res;
        }

        public async Task<List<ViewModels.LinguisticSupplier.LinguistBlockedOrgs>> GetBlockedOrgs(string lingId)
        {

            var res = new List<ViewModels.LinguisticSupplier.LinguistBlockedOrgs>();
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"SELECT distinct AppliesToDataObjectID , orgs.OrgName 				
 FROM ApprovedOrBlockedLinguisticSuppliers
 inner join orgs on orgs.id = AppliesToDataObjectID
 WHERE	AppliesToDataObjectTypeID = 2
and ApprovedOrBlockedLinguisticSuppliers.Status = 0 and LinguisticSupplierID = " + lingId + @"");

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.LinguisticSupplier.LinguistBlockedOrgs()
                    {
                        orgID = await result.GetFieldValueAsync<int>(0),
                        orgName = await result.GetFieldValueAsync<string>(1),

                    });
                }
            }
            return res;
        }
        public async Task<int> GeoPoliticalGroup(short countryId)
        {
            var res = new List<ViewModels.LinguisticSupplier.LinguistViewModel>();
            int GeoPoliticalGroup = 0;
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"SELECT GeoPoliticalGroup 				
 FROM Countries where 
ID = " + countryId + @"");

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    GeoPoliticalGroup = Convert.ToInt32(result[0]);


                }


            }

            return GeoPoliticalGroup;


        }
        public async Task<LinguisticSupplier> UpdateLinguistExpertise(LinguisticSupplier model)
        {
            var linguistDetails = await supplierRepository.All().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (linguistDetails != null)
            {

                linguistDetails.WebAddress = model.WebAddress;
                linguistDetails.SubjectAreaSpecialismsAsDescribedBySupplier = model.SubjectAreaSpecialismsAsDescribedBySupplier;
                linguistDetails.MotherTongueLanguageIanacode = model.MotherTongueLanguageIanacode;
                linguistDetails.IfBilingualOtherMotherTongueIanacode = model.IfBilingualOtherMotherTongueIanacode;
                linguistDetails.HasEncryptedComputer = model.HasEncryptedComputer;
                linguistDetails.PrimaryOperatingSystemId = model.PrimaryOperatingSystemId;
                linguistDetails.PrimaryOperatingSystemVersionId = model.PrimaryOperatingSystemVersionId;
                linguistDetails.LastModifiedByEmployeeId = model.LastModifiedByEmployeeId;
                linguistDetails.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
                linguistDetails.MasterFieldID = model.MasterFieldID;
                linguistDetails.SubFieldID = model.SubFieldID;
                linguistDetails.MediaTypeID = model.MediaTypeID;
                linguistDetails.ProfessionalBodyID = model.ProfessionalBodyID;
                supplierRepository.Update(linguistDetails);
                await supplierRepository.SaveChangesAsync();
            }

            return linguistDetails;
        }
        public async Task<LinguisticSupplierSoftwareApplication> AddApplications(LinguisticSupplierSoftwareApplication model)
        {


            var applicationsDetails = new LinguisticSupplierSoftwareApplication()
            {
                LinguisticSupplierId = model.LinguisticSupplierId,
                SoftwareApplicationId = model.SoftwareApplicationId

            };

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"insert into LinguisticSupplierSoftwareApplications (LinguisticSupplierID,SoftwareApplicationID) Values (" + model.LinguisticSupplierId + "," + model.SoftwareApplicationId + ")" + @"");

                context.Database.OpenConnection();
                command.ExecuteNonQuery();


            }

            return applicationsDetails;
        }
        public async Task<LinguisticSupplier> UpdateInvoicing(LinguisticSupplier model)
        {


            var Details = await supplierRepository.All().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (Details != null)
            {

                Details.AgreedPaymentMethodId = model.AgreedPaymentMethodId;
                Details.CurrencyId = model.CurrencyId;
                Details.Vatnumber = model.Vatnumber;
                Details.LastModifiedByEmployeeId = model.LastModifiedByEmployeeId;
                Details.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
                supplierRepository.Update(Details);
                await supplierRepository.SaveChangesAsync();
            }

            return Details;

        }
        public async Task<LinguisticSupplierSoftwareApplication> DeleteApplications(LinguisticSupplierSoftwareApplication model)
        {


            var Details = await linguisticSupplierSoftwareApplicationRepo.All().Where(x => x.LinguisticSupplierId == model.LinguisticSupplierId && x.SoftwareApplicationId == model.SoftwareApplicationId).FirstOrDefaultAsync();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"delete from LinguisticSupplierSoftwareApplications WHERE LinguisticSupplierID = " + model.LinguisticSupplierId + " and SoftwareApplicationID = " + model.SoftwareApplicationId + @"");

                context.Database.OpenConnection();
                command.ExecuteNonQuery();


            }

            return Details;
        }
        public async Task<LinguisticSupplier> UpdateLinguistStudioBreakdown(LinguisticSupplier model)
        {


            var Details = await supplierRepository.All().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (Details != null)
            {

                Details.MemoryRateForPerfectMatches = model.MemoryRateForPerfectMatches;
                Details.MemoryRateForExactMatches = model.MemoryRateForExactMatches;
                Details.MemoryRateForRepetitions = model.MemoryRateForRepetitions;
                Details.MemoryRateFor95To99Percent = model.MemoryRateFor95To99Percent;
                Details.MemoryRateFor85To94Percent = model.MemoryRateFor85To94Percent;
                Details.MemoryRateFor75To84Percent = model.MemoryRateFor75To84Percent;
                Details.MemoryRateFor50To74Percent = model.MemoryRateFor50To74Percent;
                Details.LastModifiedByEmployeeId = model.LastModifiedByEmployeeId;
                Details.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
                supplierRepository.Update(Details);
                await supplierRepository.SaveChangesAsync();
            }

            return Details;

        }
        public async Task<LinguisticSupplierRate> UpdateLinguistClientSpecifcBreakdown(LinguisticSupplierRate model)
        {


            var Details = await LsRateRepository.All().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (Details != null)
            {

                Details.MemoryRateForPerfectMatches = model.MemoryRateForPerfectMatches;
                Details.MemoryRateForExactMatches = model.MemoryRateForExactMatches;
                Details.MemoryRateForRepetitions = model.MemoryRateForRepetitions;
                Details.MemoryRateFor95To99Percent = model.MemoryRateFor95To99Percent;
                Details.MemoryRateFor85To94Percent = model.MemoryRateFor85To94Percent;
                Details.MemoryRateFor75To84Percent = model.MemoryRateFor75To84Percent;
                Details.MemoryRateFor50To74Percent = model.MemoryRateFor50To74Percent;
                Details.MemoryRateForClientSpecificPercent = model.MemoryRateForClientSpecificPercent;
                Details.LastModifiedByEmployeeId = model.LastModifiedByEmployeeId;
                Details.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
                LsRateRepository.Update(Details);
                await LsRateRepository.SaveChangesAsync();
            }

            return Details;

        }
        public async Task<LinguisticSupplier> AddLinguist(LinguisticSupplier model)
        {


            var linguistDetails = new LinguisticSupplier()
            {
                SupplierTypeId = model.SupplierTypeId,
                SupplierStatusId = 2,
                AgencyOrTeamName = model.AgencyOrTeamName == null? "" : model.AgencyOrTeamName,
                MainContactSurname = model.MainContactSurname,
                MainContactFirstName = model.MainContactFirstName,
                MainContactGender = model.MainContactGender,
                MainContactDob = model.MainContactDob,
                CountryId = model.CountryId,
                MainContactNationalityCountryId = model.MainContactNationalityCountryId,
                AgencyCompanyRegistrationNumber = model.AgencyCompanyRegistrationNumber,
                AgencyNumberOfLinguists = model.AgencyNumberOfLinguists,
                Address1 = model.Address1,
                CountyOrState = model.CountyOrState,
                PostcodeOrZip = model.PostcodeOrZip,
                MainLandlineCountryId = model.MainLandlineCountryId,
                MainLandlineNumber = model.MainLandlineNumber,
                SecondaryLandlineCountryId = model.SecondaryLandlineCountryId,
                SecondaryLandlineNumber = model.SecondaryLandlineNumber,
                MobileCountryId = model.MobileCountryId,
                MobileNumber = model.MobileNumber,
                EmailAddress = model.EmailAddress,
                SecondaryEmailAddress = model.SecondaryEmailAddress,
                SkypeId = model.SkypeId,
                MemoryRateForPerfectMatches = 0,
                MemoryRateForExactMatches = 5,
                MemoryRateForRepetitions = 5,
                MemoryRateFor95To99Percent = 10,
                MemoryRateFor85To94Percent=35,
                MemoryRateFor75To84Percent=50,
                MemoryRateFor50To74Percent=100,
                MotherTongueLanguageIanacode = "",
                IfBilingualOtherMotherTongueIanacode="",
                SubjectAreaSpecialismsAsDescribedBySupplier="",
                AgreedPaymentMethodId = 1,
                Vatnumber="",
                RatesNotes="",
                SupplierSourceId = 1,
                CreatedByEmployeeId = model.CreatedByEmployeeId,
                CreatedDate = GeneralUtils.GetCurrentUKTime(),
                VendorTypeID = model.VendorTypeID

        };
            await supplierRepository.AddAsync(linguistDetails);
            await supplierRepository.SaveChangesAsync();

            return linguistDetails;
        }
        public async Task<LinguistViewModel> FindLinguistByNameEmailAddOrSkype(string AgencyOrTeamName, string FirstName, string Surname, string EmailAddress, string SecondaryEmailAddress, string SkypeID)
        {
            var linguist = await supplierRepository.All().Where(x => x.AgencyOrTeamName == AgencyOrTeamName && x.MainContactFirstName == FirstName && x.MainContactSurname == Surname && x.EmailAddress == EmailAddress && x.SecondaryEmailAddress == SecondaryEmailAddress && x.SkypeId == SkypeID && x.DeletedDate == null)
                .Select(x => new LinguistViewModel()
                {
                    Id = x.Id,
                    SupplierTypeId = x.SupplierTypeId,
                    AgencyOrTeamName = x.AgencyOrTeamName,
                    MainContactSurname = x.MainContactSurname,
                    MainContactFirstName = x.MainContactFirstName,
                    MainContactGender = x.MainContactGender,
                    MainContactDob = x.MainContactDob,
                    MainContactNationalityCountryId = x.MainContactNationalityCountryId,
                    AgencyCompanyRegistrationNumber = x.AgencyCompanyRegistrationNumber,
                    AgencyNumberOfLinguists = x.AgencyNumberOfLinguists,
                    AgencyNumberOfDtporMultimediaOps = x.AgencyNumberOfDtporMultimediaOps,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    Address3 = x.Address3,
                    Address4 = x.Address4,
                    CountyOrState = x.CountyOrState,
                    PostcodeOrZip = x.PostcodeOrZip,
                    CountryId = x.CountryId,
                    MainLandlineCountryId = x.MainLandlineCountryId,
                    MainLandlineNumber = x.MainLandlineNumber,
                    SecondaryLandlineCountryId = x.SecondaryLandlineCountryId,
                    SecondaryLandlineNumber = x.SecondaryLandlineNumber,
                    SecondaryLandlineIsWorkNumber = x.SecondaryLandlineIsWorkNumber,
                    MobileCountryId = x.MobileCountryId,
                    MobileNumber = x.MobileNumber,
                    FaxCountryId = x.FaxCountryId,
                    FaxNumber = x.FaxNumber,
                    EmailAddress = x.EmailAddress,
                    SecondaryEmailAddress = x.SecondaryEmailAddress,
                    SecondaryEmailIsWorkAddress = x.SecondaryEmailIsWorkAddress,
                    SkypeId = x.SkypeId,
                    WebAddress = x.WebAddress,
                    Notes = x.Notes,
                    SupplierStatusId = x.SupplierStatusId,
                    SupplierSourceId = x.SupplierSourceId,
                    ApplicationFormSentToSupplierDate = x.ApplicationFormSentToSupplierDate,
                    ApplicationFormReceivedFromSupplierDate = x.ApplicationFormReceivedFromSupplierDate,
                    HasAccessToCar = x.HasAccessToCar,
                    WouldSignWitnessStatement = x.WouldSignWitnessStatement,
                    PrimaryOperatingSystemId = x.PrimaryOperatingSystemId,
                    PrimaryOperatingSystemVersionId = x.PrimaryOperatingSystemVersionId,
                    SubjectAreaSpecialismsAsDescribedBySupplier = x.SubjectAreaSpecialismsAsDescribedBySupplier,
                    MotherTongueLanguageIanacode = x.MotherTongueLanguageIanacode,
                    IfBilingualOtherMotherTongueIanacode = x.IfBilingualOtherMotherTongueIanacode,
                    CurrencyId = x.CurrencyId,
                    AgreedPaymentMethodId = x.AgreedPaymentMethodId,
                    Vatnumber = x.Vatnumber,
                    RatesNotes = x.RatesNotes,
                    MemoryRateForExactMatches = x.MemoryRateForExactMatches,
                    MemoryRateForRepetitions = x.MemoryRateForRepetitions,
                    MemoryRateFor95To99Percent = x.MemoryRateFor95To99Percent,
                    MemoryRateFor85To94Percent = x.MemoryRateFor85To94Percent,
                    MemoryRateFor75To84Percent = x.MemoryRateFor75To84Percent,
                    MemoryRateFor50To74Percent = x.MemoryRateFor50To74Percent,
                    MemoryRateForPerfectMatches = x.MemoryRateForPerfectMatches,
                    Referee1Name = x.Referee1Name,
                    Referee1FullAddress = x.Referee1FullAddress,
                    Referee1Phone = x.Referee1Phone,
                    Referee1EmailAddress = x.Referee1EmailAddress,
                    Referee2Name = x.Referee2Name,
                    Referee2FullAddress = x.Referee2FullAddress,
                    Referee2Phone = x.Referee2Phone,
                    Referee2EmailAddress = x.Referee2EmailAddress,
                    IsAclientInternalLinguist = x.IsAclientInternalLinguist,
                    CreatedDate = x.CreatedDate,
                    CreatedByEmployeeId = x.CreatedByEmployeeId,
                    LastModifiedDate = x.LastModifiedDate,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    DeletedDate = x.DeletedDate,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    NdaunlockedDateTime = x.NdaunlockedDateTime,
                    NdaunlockedByEmployeeId = x.NdaunlockedByEmployeeId,
                    NdauploadedDateTime = x.NdauploadedDateTime,
                    NdauploadedByEmployeeId = x.NdauploadedByEmployeeId,
                    LastExportedToSageDateTime = x.LastExportedToSageDateTime,
                    CurrencyOfFirstInvoiceExportedToSage = x.CurrencyOfFirstInvoiceExportedToSage,
                    HasEncryptedComputer = x.HasEncryptedComputer,
                    SupplierResponsivenessRating = x.SupplierResponsivenessRating,
                    SupplierFollowingTheInstructionsRating = x.SupplierFollowingTheInstructionsRating,
                    SupplierAttitudeRating = x.SupplierAttitudeRating,
                    SupplierQualityOfWorkRating = x.SupplierQualityOfWorkRating,
                    SupplierOnTimeDeliveryRating = x.SupplierOnTimeDeliveryRating,
                    OverallRating = x.OverallRating,
                    QualityRatingAvgForFirst2Jobs = x.QualityRatingAvgForFirst2Jobs,
                    IsQualityRatingPendingForFirst2Jobs = x.IsQualityRatingPendingForFirst2Jobs,
                    IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded = x.IsQualityRatingPendingForAnyJobsIflinguistIsNewlyAdded,
                    Ftpenabled = x.Ftpenabled,
                    GdpracceptedDateTime = x.GdpracceptedDateTime,
                    Gdprstatus = x.Gdprstatus,
                    GdpraccetedViaIplus = x.GdpraccetedViaIplus,
                    GdprrejectedDateTime = x.GdprrejectedDateTime,
                    NeedApprovalToBeAddedToDb = x.NeedApprovalToBeAddedToDb,
                    ApprovedToAbeAddedtoDbdatetime = x.ApprovedToAbeAddedtoDbdatetime,
                    ApprovedToAbeAddedtoDbbyEmpId = x.ApprovedToAbeAddedtoDbbyEmpId,
                    NonEeaclauseAcceptedDateTime = x.NonEeaclauseAcceptedDateTime,
                    NonEeaclauseDeclinedDateTime = x.NonEeaclauseDeclinedDateTime,
                    SapmasterDataReferenceNumber = x.SapmasterDataReferenceNumber,
                    ContractUnlockedDateTime = x.ContractUnlockedDateTime,
                    ContractUnlockedByEmployeeId = x.ContractUnlockedByEmployeeId,
                    ContractUploadedDateTime = x.ContractUploadedDateTime,
                    ContractUploadedByEmployeeId = x.ContractUploadedByEmployeeId,
                    MasterFieldID = x.MasterFieldID,
                    SubFieldID = x.SubFieldID,
                    MediaTypeID = x.MediaTypeID,
                    VendorTypeID = x.VendorTypeID,
                    Referee1Received = x.Referee1Received,
                    Referee2Received = x.Referee2Received,
                    RateNotes = x.RatesNotes
                }).FirstOrDefaultAsync();

            return linguist;
        }

    }
}
