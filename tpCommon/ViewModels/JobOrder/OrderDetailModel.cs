using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.JobOrder
{
    public class OrderDetailModel
    {
        public int JobOrderId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string JobName { get; set; }
        public string PONumber { get; set; }
        public string ClientNotes { get; set; }
        public string Currency { get; set; }
        public string OrderChannel { get; set; }
        public string Cost { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string Priority { get; set; }
        public string SourceFiles { get; set; }
        public string TargetFiles { get; set; }
        public string ReferenceFiles { get; set; }
        public bool IsSourceFileAvailable { get; set; }
        public bool IsTargetFileAvailable { get; set; }
        public bool IsReferenceFileAvailable { get; set; }

    }
}
