using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;


namespace Services
{
    public class TPExtranetUserService : ITPExtranetUserService
    {
        private readonly IRepository<ExtranetUsersTemp> extranetUserRepo;
        private readonly IRepository<ExtranetUser> oldExtranetUserRepo;
        private readonly IRepository<ExtranetAccessLevels> extranetAccessLevelsRepo;
        private readonly IRepository<Contact> contactRepo;
        private readonly IRepository<Org> orgRepo;
        private readonly IRepository<OrgGroup> orgGroupRepo;
        private readonly GlobalVariables globalVariables;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITPLanguageLogic languageLogic;


        public TPExtranetUserService(IRepository<ExtranetUsersTemp> repository1,
            IRepository<ExtranetAccessLevels> repository2,
            IRepository<Contact> repository3,
            IRepository<Org> repository4,
            IRepository<OrgGroup> _orgGroupRepo,
            IRepository<ExtranetUser> extranetUserRepo,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ITPLanguageLogic _languageLogic)
        {
            this.extranetUserRepo = repository1;
            this.extranetAccessLevelsRepo = repository2;
            this.contactRepo = repository3;
            this.orgRepo = repository4;
            this.orgGroupRepo = _orgGroupRepo;
            this.oldExtranetUserRepo = extranetUserRepo;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.languageLogic = _languageLogic;


            globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
        }

        public async Task<ExtranetUsersTemp> GetExtranetUserByUsername(string extranetUserName)
        {
            var result = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ExtranetUser> GetOldIPlusUser(string extranetUserName)
        {
            var result = await oldExtranetUserRepo.All().Where(e => e.UserName == extranetUserName).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ExtranetAccessLevels> GetAccessLevelInfo(string extranetUserName)
        {
            var result = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(a => a.AccessLevelId)
                                    .Join(extranetAccessLevelsRepo.All(),
                                          u => u,
                                          a => a.Id,
                                          (u, a) => new { accessLevel = a }).Select(a => a.accessLevel).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ExtranetAccessLevels> GetAccessLevelInfoForOldIplusUser(string extranetUserName)
        {
            var result = await oldExtranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(a => a.AccessLevelId)
                                    .Join(extranetAccessLevelsRepo.All(),
                                          u => u,
                                          a => a.Id,
                                          (u, a) => new { accessLevel = a }).Select(a => a.accessLevel).FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Gets DataObjectID that is allowed to be accessed for the ExtranetUser
        /// Eg: If use is allowed to access all group orders, this function will return groupID
        /// </summary>
        /// <param name="extranetUserName"></param>
        /// <returns></returns>
        public async Task<int> GetPermittedDataObjectID(string extranetUserName)
        {
            var accessLevel = await GetAccessLevelInfo(extranetUserName);

            if (accessLevel.CanViewDetailsOfOtherGroupOrders == true)
            {
                int? groupId = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All().Select(c => new { c.Id, c.OrgId }),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All().Select(c => new { c.Id, c.OrgGroupId }),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();
                return groupId.Value;
            }
            else if (accessLevel.CanViewDetailsOfOtherOrgOrders == true)
            {
                int? orgId = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All().Select(c => new { c.Id, c.OrgId }),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                return orgId.Value;
            }
            else
            {
                int? contactID = GetExtranetUserByUsername(extranetUserName).Result.DataObjectId;

                return contactID.Value;
            }


        }

        /// <summary>
        /// Gets DataObjectTypeID that is allowed to be accessed for the ExtranetUser
        /// Eg: If use is allowed to access all group orders, this function will return data object type ID of the group
        /// </summary>
        /// <param name="extranetUserName"></param>
        /// <returns></returns>
        public async Task<int> GetPermittedDataObjectTypeID(string extranetUserName)
        {
            var accessLevel = await GetAccessLevelInfo(extranetUserName);

            if (accessLevel.CanViewDetailsOfOtherGroupOrders == true)
            {
                return (int)Global_Settings.Enumerations.DataObjectTypes.OrgGroup;

            }
            else if (accessLevel.CanViewDetailsOfOtherOrgOrders == true)
            {
                return (int)Global_Settings.Enumerations.DataObjectTypes.Org;
            }
            else
            {
                return (int)Global_Settings.Enumerations.DataObjectTypes.Contact;
            }


        }

        public async Task<Org> GetExtranetUserOrg(string extranetUserName)
        {
            var result = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All().Select(c => new { c.Id, c.OrgId }),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId)
                                    .Join(orgRepo.All(),
                                          c => c,
                                          o => o.Id,
                                          (c, o) => new { org = o }).Select(o => o.org).FirstOrDefaultAsync();

            return result;
        }

        public async Task<OrgGroup> GetExtranetUserOrgGroup(string extranetUserName)
        {
            var result = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All().Select(c => new { c.Id, c.OrgId }),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId)
                                    .Join(orgRepo.All(),
                                          c => c,
                                          o => o.Id,
                                          (c, o) => new { org = o }).Select(o => o.org)
                                    .Join(orgGroupRepo.All(),
                                          o => o.OrgGroupId,
                                          g => g.Id,
                                          (o, g) => new { group = g }).Select(g => g.group)
                                    .FirstOrDefaultAsync();

            return result;
        }
        public async Task<Contact> GetExtranetUserContact(string extranetUserName)
        {
            var result = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { contact = c }).Select(c => c.contact).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ExtranetUsersTemp> GetExtranetUserByContactId(int contactId)
        {
            var result = await extranetUserRepo.All().Where(e => e.DataObjectId == contactId && e.DataObjectTypeId == 1).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> DoPasswordsMatch(string extranetUserName, string SuppliedPlainTextPassword)
        {
            var oldExtranetUser = await oldExtranetUserRepo.All().Where(e => e.UserName == extranetUserName).FirstOrDefaultAsync();
            byte[] SuppliedPasswordByteData = Encoding.UTF8.GetBytes(SuppliedPlainTextPassword);
            byte[] SaltByteData = Encoding.UTF8.GetBytes(oldExtranetUser.Salt);

            byte[] SaltedSuppliedPasswordByteData = new byte[SuppliedPasswordByteData.Length + SaltByteData.Length + 1];

            Array.Copy(SuppliedPasswordByteData, 0, SaltedSuppliedPasswordByteData, 0, SuppliedPasswordByteData.Length);
            Array.Copy(SaltByteData, 0, SaltedSuppliedPasswordByteData, SuppliedPasswordByteData.Length, SaltByteData.Length);

            SHA512Managed SHAm = new SHA512Managed();
            byte[] HashByteData = SHAm.ComputeHash(SaltedSuppliedPasswordByteData);

            var HashStringData = Convert.ToBase64String(HashByteData);

            return (oldExtranetUser.HashedPassword == HashStringData);
        }

        public async Task<ExtranetUsersTemp> UpdateflowPlusExternalDetails(string extranetUserName)
        {
            var oldExtranetUser = await oldExtranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefaultAsync();

            var flowPlusExternalUser = await extranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefaultAsync();

            var useprofilePhoto = "";
            if (oldExtranetUser.UserProfileImagePath != "" && oldExtranetUser.UserProfileImagePath != null)
            {
                useprofilePhoto = oldExtranetUser.UserProfileImagePath.Replace("~/NewStyle/Images/UserProfileImages/", "");
            }

            flowPlusExternalUser.WebServiceGuid = oldExtranetUser.WebServiceGuid;
            flowPlusExternalUser.AccessLevelId = oldExtranetUser.AccessLevelId;
            flowPlusExternalUser.DataObjectId = oldExtranetUser.DataObjectId;
            flowPlusExternalUser.DataObjectTypeId = oldExtranetUser.DataObjectTypeId;
            flowPlusExternalUser.DefaultTimeZone = oldExtranetUser.DefaultTimeZone;
            flowPlusExternalUser.DesignplusEnabled = oldExtranetUser.DesignplusEnabled;
            flowPlusExternalUser.FirstEverLoginDateTime = oldExtranetUser.FirstEverLoginDateTime;
            flowPlusExternalUser.LastLoginDateTime = oldExtranetUser.LastLoginDateTime;
            flowPlusExternalUser.LockedOutDateTime = oldExtranetUser.LockedOutDateTime;
            flowPlusExternalUser.MustResetPasswordOnNextLogin = oldExtranetUser.MustResetPasswordOnNextLogin;
            flowPlusExternalUser.NumberOfFailedAnswerAttempts = oldExtranetUser.NumberOfFailedAnswerAttempts;
            flowPlusExternalUser.NumberOfFailedLoginAttempts = oldExtranetUser.NumberOfFailedLoginAttempts;
            flowPlusExternalUser.PasswordLastSetDateTime = oldExtranetUser.PasswordLastSetDateTime;
            flowPlusExternalUser.PreferredExtranetUilangIanacode = oldExtranetUser.PreferredExtranetUilangIanacode;
            flowPlusExternalUser.PreviousLoginDateTime = oldExtranetUser.PreviousLoginDateTime;
            flowPlusExternalUser.SecretQuestionId = oldExtranetUser.SecretQuestionId;
            //flowPlusExternalUser.SecretQuestionAnswerSalt = oldExtranetUser.SecretQuestionAnswerSalt;
            flowPlusExternalUser.ShowNewInfoPopUp = oldExtranetUser.ShowNewInfoPopUp;
            flowPlusExternalUser.TranslateonlineAllowed = oldExtranetUser.TranslateonlineAllowed;
            flowPlusExternalUser.UserProfileImagePath = useprofilePhoto;
            flowPlusExternalUser.UserSetupByEmployeeId = oldExtranetUser.UserSetupByEmployeeId;
            flowPlusExternalUser.UserSetupDateTime = oldExtranetUser.UserSetupDateTime;
            flowPlusExternalUser.WebServiceNotificationEmailAddress = oldExtranetUser.WebServiceNotificationEmailAddress;
            flowPlusExternalUser.WebServiceNotifyOnFileCollection = oldExtranetUser.WebServiceNotifyOnFileCollection;
            flowPlusExternalUser.WebServiceNotifyOnOrderSubmission = oldExtranetUser.WebServiceNotifyOnOrderSubmission;


            extranetUserRepo.Update(flowPlusExternalUser);
            await extranetUserRepo.SaveChangesAsync();

            return flowPlusExternalUser;
        }

        public async Task<ExtranetUsersTemp> UpdatePasswordResetCode(string extranetUserName, Guid? PasswordResetCode = null)
        {

            var flowPlusExternalUser = await extranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefaultAsync();

            flowPlusExternalUser.PasswordResetCode = PasswordResetCode;


            extranetUserRepo.Update(flowPlusExternalUser);
            await extranetUserRepo.SaveChangesAsync();

            return flowPlusExternalUser;
        }

        public async Task<ExtranetUser> UpdatePasswordResetCodeForOldIplus(string extranetUserName, Guid? PasswordResetCode = null)
        {

            var flowPlusExternalUser = await oldExtranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefaultAsync();

            flowPlusExternalUser.PasswordResetCode = PasswordResetCode;


            oldExtranetUserRepo.Update(flowPlusExternalUser);
            await oldExtranetUserRepo.SaveChangesAsync();

            return flowPlusExternalUser;
        }

        public async Task<ExtranetUser> ResetPasswordForIPlusLogin(string extranetUserName, string plainTextPassword)
        {

            if (plainTextPassword == "")
            {
                throw new Exception("Cannot use an empty password.");
            }

            var iPlusExternalUser = await oldExtranetUserRepo.All().Where(x => x.UserName == extranetUserName).FirstOrDefaultAsync();

            //Generate the random number using the cryptographic service provider
            //RNGCryptoServiceProvider RandomNumberGenerator = new RNGCryptoServiceProvider();
            //Byte[] SaltByteArray = new Byte[16];
            //RandomNumberGenerator.GetBytes(SaltByteArray);

            //Return a Base64 string representation of the random number
            //string newPasswordSalt = Convert.ToBase64String(SaltByteArray);

            string newPasswordSalt = iPlusExternalUser.Salt;

            Byte[] PasswordByteData = Encoding.UTF8.GetBytes(plainTextPassword);
            Byte[] SaltByteData = Encoding.UTF8.GetBytes(newPasswordSalt);

            Byte[] SaltedPasswordByteData = new Byte[PasswordByteData.Length + SaltByteData.Length + 1];

            Array.Copy(PasswordByteData, 0, SaltedPasswordByteData, 0, PasswordByteData.Length);
            Array.Copy(SaltByteData, 0, SaltedPasswordByteData, PasswordByteData.Length, SaltByteData.Length);

            SHA512Managed SHAm = new SHA512Managed();
            Byte[] HashByteData = SHAm.ComputeHash(SaltedPasswordByteData);

            string newHashedPassword = Convert.ToBase64String(HashByteData);

            iPlusExternalUser.HashedPassword = newHashedPassword;
            iPlusExternalUser.Salt = newPasswordSalt;
            iPlusExternalUser.PasswordLastSetDateTime = GeneralUtils.GetCurrentGMT();

            oldExtranetUserRepo.Update(iPlusExternalUser);
            await oldExtranetUserRepo.SaveChangesAsync();

            return iPlusExternalUser;
        }

        public async Task<ExtranetUser> EnableAsUser(byte DataObjectTypeId, int DataObjectID, string PlainTextPassword,
                                                     int AccessLevelID, string UserTimeZone, bool translateonlineAllowed,
                                                     bool designplusEnabled, short enabledByEmployeeId)
        {
            var newPasswordSalt = GeneralUtils.CreateSaltForPasswordHashing();

            Byte[] PasswordByteData = Encoding.UTF8.GetBytes(PlainTextPassword);
            Byte[] SaltByteData = Encoding.UTF8.GetBytes(newPasswordSalt);

            Byte[] SaltedPasswordByteData = new Byte[PasswordByteData.Length + SaltByteData.Length + 1];

            Array.Copy(PasswordByteData, 0, SaltedPasswordByteData, 0, PasswordByteData.Length);
            Array.Copy(SaltByteData, 0, SaltedPasswordByteData, PasswordByteData.Length, SaltByteData.Length);

            SHA512Managed SHAm = new SHA512Managed();
            Byte[] HashByteData = SHAm.ComputeHash(SaltedPasswordByteData);

            string newHashedPassword = Convert.ToBase64String(HashByteData);

            string newUserName = "";
            if (DataObjectTypeId == 1)
            {
                var contactObj = await contactRepo.All().Where(c => c.Id == DataObjectID).FirstOrDefaultAsync();
                newUserName = contactObj.EmailAddress;

                var extranetUserObj = await GetOldIPlusUser(newUserName);

                int i = 1;
                while (extranetUserObj != null)
                {
                    newUserName = contactObj.EmailAddress + "-" + i.ToString();
                    extranetUserObj = await GetOldIPlusUser(newUserName);
                    i += 1;
                }


                //create user in old ExtranetUser table
                var newExtranetUserObj = new ExtranetUser()
                {
                    UserName = newUserName,
                    HashedPassword = newHashedPassword,
                    Salt = newPasswordSalt,
                    PasswordLastSetDateTime = GeneralUtils.GetCurrentGMT(),
                    MustResetPasswordOnNextLogin = false,
                    AccessLevelId = AccessLevelID,
                    DataObjectTypeId = DataObjectTypeId,
                    DataObjectId = DataObjectID,
                    DefaultTimeZone = UserTimeZone,
                    TranslateonlineAllowed = translateonlineAllowed,
                    DesignplusEnabled = designplusEnabled,
                    UserSetupDateTime = GeneralUtils.GetCurrentGMT(),
                    UserSetupByEmployeeId = enabledByEmployeeId
                };

                await oldExtranetUserRepo.AddAsync(newExtranetUserObj);
                await oldExtranetUserRepo.SaveChangesAsync();

                return newExtranetUserObj;


            }

            else
            {
                return null;
            }

        }

        public async Task<LocalLanguageInfo> GetCurrentExtranetUserDefaultLanguage()
        {
            var localLanguageInfo = httpContextAccessor.HttpContext.Session.Get<LocalLanguageInfo>("CurrentUILang");

            ExtranetUsersTemp extranetUser;
            try
            {
                if (localLanguageInfo == null)
                {
                    string extranetUserName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                    extranetUser = await GetExtranetUserByUsername(extranetUserName);

                    localLanguageInfo = languageLogic.GetLanguageInfo(extranetUser.PreferredExtranetUilangIanacode, "en");
                    httpContextAccessor.HttpContext.Session.Set("CurrentUILang", localLanguageInfo);
                }
            }
            catch
            {
                localLanguageInfo = languageLogic.GetLanguageInfo("en", "en");
            }


            return localLanguageInfo;
        }

        public async Task<ExtranetUsersTemp> GetCurrentExtranetUser()
        {
            ExtranetUsersTemp extranetUser = httpContextAccessor.HttpContext.Session.Get<ExtranetUsersTemp>("LoggedInExtranetUser");

            if (extranetUser == null)
            {
                string extranetUserName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                extranetUser = await GetExtranetUserByUsername(extranetUserName);

                httpContextAccessor.HttpContext.Session.Set("LoggedInExtranetUser", extranetUser);

            }

            return extranetUser;
        }

        public string GetCurrentExtranetUserName()
        {
            string extranetUserName = httpContextAccessor.HttpContext.Session.Get<string>("LoggedInExtranetUserName");
            //string extranetUserName = Microsoft.AspNetCore.Mvc..User.FindFirst(ClaimTypes.Name).Value;

            if (extranetUserName == null)
            {
                extranetUserName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

                httpContextAccessor.HttpContext.Session.Set("LoggedInExtranetUserName", extranetUserName);

            }

            return extranetUserName;
        }


        public async Task<LocalLanguageInfo> UpdateCurrentExtranetUserDefaultLanguage(string LangIANACode)
        {

            var localLanguageInfo = languageLogic.GetLanguageInfo(LangIANACode, "en");
            httpContextAccessor.HttpContext.Session.Set("CurrentUILang", localLanguageInfo);

            return localLanguageInfo;
        }
    }
}
