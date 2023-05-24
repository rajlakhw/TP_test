using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeExpenseClaimsMaster
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public decimal TotalBaseCurrencyClaimValue { get; set; }
        public decimal AmountInGbp { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public DateTime? ApprovedByManagerDateTime { get; set; }
        public short? ManagerEmployeeId { get; set; }
        public DateTime? ApprovedByAccountsDateTime { get; set; }
        public short? AccountsEmployeeId { get; set; }
        public string Remarks { get; set; }
        public DateTime? ExportedToSageDateTime { get; set; }
        public DateTime? PaidDateTime { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
