using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class MarketingCampaignRecipientsNew
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public DateTime? FirstAccessedEmailImageDateTime { get; set; }
    }
}
