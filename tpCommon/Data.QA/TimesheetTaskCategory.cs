using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class TimesheetTaskCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public byte? CategoryTypeId { get; set; }
        public bool? AppliesToAllDepartments { get; set; }
        public bool? AppliesToGls { get; set; }
        public bool? AppliesToTandP { get; set; }
        public bool? AppliesToVm { get; set; }
        public bool? AppliesToQa { get; set; }
        public bool? AppliesToSales { get; set; }
        public bool? AppliesToEnquiries { get; set; }
        public bool? AppliesToMarketing { get; set; }
        public bool? AppliesToAccounts { get; set; }
        public bool? AppliesToHr { get; set; }
        public bool? AppliesToTech { get; set; }
    }
}
