using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.JobItem;

namespace ViewModels.JobOrder
{
    public class JobOrderDetailsViewModel
    {
        public OrderDetailModel OrderDetails { get; set; }
        public List<JobItemDetailsModel> JobItems { get; set; }
    }
}
