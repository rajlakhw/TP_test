using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeExpenseClaim
    {
        public int Id { get; set; }
        public int? OrgId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime ExpenseEndDate { get; set; }
        public int CategoryId { get; set; }
        public string ExpenseDetails { get; set; }
        public decimal Amount { get; set; }
        public short CurrencyId { get; set; }
        public decimal AmountInBaseCurrency { get; set; }
        public short IsForEmployeeReimbursement { get; set; }
        public string ScanedReceiptName { get; set; }
        public int? ClaimMasterId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
