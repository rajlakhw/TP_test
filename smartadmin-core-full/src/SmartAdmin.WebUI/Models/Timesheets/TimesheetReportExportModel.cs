using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAdmin.WebUI.Models.Timesheets
{
    public class TimesheetReportExportModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DepartmentString { get; set; }
        public string TeamString { get; set; }
        public string EmployeeString { get; set; }
        public string OrgGroupString { get; set; }
        public string OrgString { get; set; }
        public string EndClientString { get; set; }
        public string BrandString { get; set; }
        public string CategoryString { get; set; }
        public string CampaignString { get; set; }
        public string ActivityTypeString { get; set; }
        public string ActivityNameString { get; set; }
        public string ApprovalStatusString { get; set; }

    }
}
