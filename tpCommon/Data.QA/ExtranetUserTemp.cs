using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Data.QA
{
    public class ExtranetUserTemp : IdentityUser
    {
        public string Salt { get; set; }
        public DateTime? PasswordLastSetDateTime { get; set; }
        public bool MustResetPasswordOnNextLogin { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public int AccessLevelId { get; set; }
        public string PreferredExtranetUilangIanacode { get; set; }
        public Guid? WebServiceGuid { get; set; }
        public string WebServiceNotificationEmailAddress { get; set; }
        public bool? WebServiceNotifyOnOrderSubmission { get; set; }
        public bool? WebServiceNotifyOnFileCollection { get; set; }
        public DateTime? FirstEverLoginDateTime { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public DateTime? PreviousLoginDateTime { get; set; }
        public byte? NumberOfFailedLoginAttempts { get; set; }
        public short UserSetupByEmployeeId { get; set; }
        public DateTime UserSetupDateTime { get; set; }
        public DateTime? LockedOutDateTime { get; set; }
        public string DefaultTimeZone { get; set; }
        public bool TranslateonlineAllowed { get; set; }
        public bool DesignplusEnabled { get; set; }
        public string UserProfileImagePath { get; set; }
        public bool? IsCustomizedHomePageSet { get; set; }
        public string CustomizedHomePageLayout { get; set; }
        public bool? CustomizedHomePageVisited { get; set; }
        public bool? ShowDesignPlusInfoBox { get; set; }
        public bool? ShowNewInfoPopUp { get; set; }
        public int? SecretQuestionId { get; set; }
        public string HashedSecretQuestionAnswer { get; set; }
        public string SecretQuestionAnswerSalt { get; set; }
        public int? NumberOfFailedAnswerAttempts { get; set; }
    }
}
