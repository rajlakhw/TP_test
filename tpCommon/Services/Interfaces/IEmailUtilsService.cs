using System.Collections.Generic;
using Services.Common;

namespace Services.Interfaces
{
    public interface IEmailUtilsService : IService
    {
        bool SendMail(string MsgFrom, string MsgTo, string MsgSubject, string MsgBody,
            bool MsgIsHTML = true, bool SuppressSignatureForMarketingReasons = false, List<string> AttachmentPathsList = null,
            string CCRecipients = "", bool DeliverViaAmazon = false, bool RequestReadReceipt = false,
            bool MsgHighPriority = false, bool IsExternalNotification = false);
    }
}
