using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.Contact;
using ViewModels.ExtranetUsers;
using Data;

namespace Services.Interfaces
{
    public interface ITPContactsLogic : IService
    {
        Task<Data.Contact> GetContactDetails(int ID);

        Task<List<JobContactResults>> GetJobOrders(string ContactID);
        Task<IEnumerable<ContactTableViewModel>> GetAllContactsForOrg(int orgId);
        Task<List<string>> GetAllContactIdAndNameStringForOrg(int orgId);
        Task<List<string>> GetAllContactIdAndNameStringForGroup(int groupId);
        Task<List<EnquiriesContactResults>> GetEnquiries(string ContactID);
        Task<ContactViewModel> Update(ContactViewModel contact);
        Task<List<ContactCountry>> GetContactCountries();
        Task<List<ContactSource>> GetIntroductionSource();
        int? GetContactOrgGroup(int ContactID);
        string GetExtranetUserName(int contactID);
        Task<ExtranetUserModel> GetExtranetUser(int contactId);
        Task<ContactModel> GetById(int Id);
        Task<IEnumerable<ContactModel>> SearchByNameOrId(string searchTerm);
        Task<string> EmailAddressesForNotification(int contactId);

        Task<List<Contact>> GetContactsFromOrgWhoIsAllowedToUseChargeableSoftware(int orgId, int ChargeableSoftware);
    }
}
