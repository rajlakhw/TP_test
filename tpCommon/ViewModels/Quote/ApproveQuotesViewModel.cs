using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Quote
{
    public partial class ApproveQuotesViewModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string RequestedBy { get; set; }
        public string ProjectName { get; set; }
        public string Division { get; set; }
        public string Currency { get; set; }
        public string Value { get; set; }
    }
    
}
