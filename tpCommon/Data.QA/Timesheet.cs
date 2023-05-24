using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class Timesheet
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? OrgId { get; set; }
        public DateTime TimeLogDate { get; set; }
        public int? EndClientId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? CampaignId { get; set; }
        public short? ClientChargeInHours { get; set; }
        public short? ClientChargeInMinutes { get; set; }
        public short? NonChargeableTimeInHours { get; set; }
        public short? NonChargeableTimeInMinutes { get; set; }
        public short? NonClientChargeInHours { get; set; }
        public short? NonClientChargeInMinutes { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public short? CreatedByEmpId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmpId { get; set; }
    }
}
