using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class JobOrderPgddetail
    {
        public int Id { get; set; }
        public int JobOrderId { get; set; }
        public string ThirdPartyId { get; set; }
        public string CampaignName { get; set; }
        public string ProductionContact { get; set; }
        public string EndClientName { get; set; }
        public string EndClientContact { get; set; }
        public string ProjectStatus { get; set; }
        public string Icponumber { get; set; }
        public bool GlossaryUpdated { get; set; }
        public string Bshfmnumber { get; set; }
        public string Isnumber { get; set; }
        public decimal ApprovedEndClientCharge { get; set; }
        public short EndClientChargeCurrencyId { get; set; }
        public decimal ApprovedEndClientChargeGbp { get; set; }
        public decimal? Variance { get; set; }
    }
}
