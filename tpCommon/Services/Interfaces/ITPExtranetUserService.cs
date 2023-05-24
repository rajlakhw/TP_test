using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface ITPExtranetUserService : IService
    {
        Task<ExtranetUsersTemp> GetExtranetUserByUsername(string extranetUserName);
        Task<ExtranetUser> GetOldIPlusUser(string extranetUserName);
        Task<ExtranetAccessLevels> GetAccessLevelInfo(string extranetUserName);
        Task<int> GetPermittedDataObjectID(string extranetUserName);
        Task<int> GetPermittedDataObjectTypeID(string extranetUserName);
        Task<Org> GetExtranetUserOrg(string extranetUserName);
        Task<Contact> GetExtranetUserContact(string extranetUserName);
        Task<OrgGroup> GetExtranetUserOrgGroup(string extranetUserName);
        Task<ExtranetUsersTemp> GetExtranetUserByContactId(int contactId);
        Task<bool> DoPasswordsMatch(string extranetUserName, string SuppliedPlainTextPassword);
        Task<ExtranetUsersTemp> UpdateflowPlusExternalDetails(string extranetUserName);
        Task<ExtranetUsersTemp> UpdatePasswordResetCode(string extranetUserName, Guid? PasswordResetCode = null);
        Task<ExtranetUser> UpdatePasswordResetCodeForOldIplus(string extranetUserName, Guid? PasswordResetCode = null);
        Task<ExtranetUser> ResetPasswordForIPlusLogin(string extranetUserName, string PlainTextPassword);
        Task<ExtranetAccessLevels> GetAccessLevelInfoForOldIplusUser(string extranetUserName);
        Task<ExtranetUser> EnableAsUser(byte DataObjectTypeId, int DataObjectID, string PlainTextPassword,
                                        int AccessLevelID, string UserTimeZone, bool translateonlineAllowed,
                                        bool designplusEnabled, short enabledByEmployeeId);
        Task<LocalLanguageInfo> GetCurrentExtranetUserDefaultLanguage();
        Task<ExtranetUsersTemp> GetCurrentExtranetUser();
        string GetCurrentExtranetUserName();
        Task<LocalLanguageInfo> UpdateCurrentExtranetUserDefaultLanguage(string LangIANACode);
    }
}
