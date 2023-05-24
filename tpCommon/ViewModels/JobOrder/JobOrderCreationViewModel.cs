using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.EmployeeOwnerships;

namespace ViewModels.JobOrder
{
    public class JobOrderCreationViewModel
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public List<Employee> ListOfEmployees { get; set; }
        public IEnumerable<DropdownOptionViewModel> AllAvailableCurrencies { get; set; }
        public List<JobOrderChannel> JobOrdeChannels { get; set; }
        public IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory> SurchargesCategories { get; set; }
        public IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory> DiscountCategories { get; set; }
        public IEnumerable<TPEndClient.TPEndClientViewModel> EndClients { get; set; }
        public short? InvoiceCurrencyId { get; set; }
        public short? Ownership { get; set; }
    }
}
