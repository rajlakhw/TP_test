using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.LinguisticSupplier;

namespace Services.Interfaces
{
    public interface ITPLinguistService : IService
    {
        Task<LinguistViewModel> GetById(int id);

        string getStatus(int id);
        string getType(int id);
        string getCurrencyName(int id);
        string getLastLogin(int id);
        string getPaymentType(int id);
        string getExtranetName(int id);
        string getCountryName(int id);
        Task<IEnumerable<LinguistViewModel>> SearchByNameOrId(string searchTerm, string sourceLangIanaCode, string targetLangIanaCode);
        Task<IEnumerable<LinguistViewModel>> SearchByNameOrIdOnly(string searchTerm);
        Task<InvoiceViewModel> GetSupplierInvoiceFromJobItemId(int jobItemId);
        Task<List<ViewModels.LinguisticSupplier.LinguistRateResults>> GetLinguistRates(string lingId);
        Task<List<ViewModels.LinguisticSupplier.LinguistItems>> GetLinguistItems(string lingId);
        Task<List<ViewModels.LinguisticSupplier.LinguistTop5Words>> GetLinguistTop5Words(string lingId);
        Task<List<ViewModels.LinguisticSupplier.LinguistTop5Items>> GetLinguistTop5Items(string lingId);
        Task<LinguistViewModel> Update(LinguistViewModel supplier, short EmployeCurrentlyLoggedInID);
        Task<List<ViewModels.LinguisticSupplier.LinguistApprovedOrgs>> GetApprovedOrgs(string lingId);
        Task<List<ViewModels.LinguisticSupplier.LinguistBlockedOrgs>> GetBlockedOrgs(string lingId);
        Task<int> GeoPoliticalGroup(short countryId);
        Task<LinguisticSupplier> UpdateLinguistExpertise(LinguisticSupplier model);
        Task<LinguisticSupplierSoftwareApplication> AddApplications(LinguisticSupplierSoftwareApplication model);
        Task<LinguisticSupplier> UpdateInvoicing(LinguisticSupplier model);
        Task<LinguisticSupplierSoftwareApplication> DeleteApplications(LinguisticSupplierSoftwareApplication model);
        Task<LinguisticSupplier> UpdateLinguistStudioBreakdown(LinguisticSupplier model);
        Task<LinguisticSupplierRate> UpdateLinguistClientSpecifcBreakdown(LinguisticSupplierRate model);
        Task<LinguisticSupplier> AddLinguist(LinguisticSupplier model);
        Task<LinguistViewModel> FindLinguistByNameEmailAddOrSkype(string AgencyOrTeamName, string FirstName, string Surname, string EmailAddress, string SecondaryEmailAddress, string SkypeID);
    }
}
