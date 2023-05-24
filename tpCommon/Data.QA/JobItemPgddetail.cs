using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class JobItemPgddetail
    {
        public int Id { get; set; }
        public int JobItemId { get; set; }
        public string Markets { get; set; }
        public string Service { get; set; }
        public string AssetsOverview { get; set; }
        public DateTime? AirDate { get; set; }
        public bool CopydeckStored { get; set; }
        public string Votalent { get; set; }
        public bool BuyoutAgreementSigned { get; set; }
        public string UsageType { get; set; }
        public int? UsageDuration { get; set; }
        public DateTime? UsageStartDate { get; set; }
        public DateTime? UsageEndDate { get; set; }
    }
}
