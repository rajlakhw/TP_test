using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Reports
{
    public class ReportExportModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FilterByJobItemOrJobOrder { get; set; }
        public string FilterByDate { get; set; }
        public string InternalExternalOrAll { get; set; }
        public string OrgGroupString { get; set; }
        public string OrgString { get; set; }
        public string EndClientString { get; set; }
        public string BrandString { get; set; }
        public string CategoryString { get; set; }
        public string CampaignString { get; set; }
        public string ResultsToInclude { get; set; }
    }
}
