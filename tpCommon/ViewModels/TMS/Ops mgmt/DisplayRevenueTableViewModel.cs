namespace ViewModels.TMS.Ops_mgmt
{
    public class DisplayRevenueTableViewModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int ClosedOrdersCurrentYear { get; set; }
        public int ClosedOrdersCurrentMonth { get; set; }
        public int ClosedItemsCurrentYear { get; set; }
        public int ClosedItemsCurrentMonth { get; set; }
        public decimal TotalValueOfAllOpenJobOrders { get; set; }
        public decimal RecognisedRevenueCurrentMonthInGBP { get; set; }
        public decimal RecognisedRevenueCurrentYearInGBP { get; set; }
        public decimal PendingRevenueCurrentMonthInGBP { get; set; }
        public decimal MarginCurrentYear { get; set; }
        public decimal ClientChargeForMargin { get; set; }
        public decimal PaymentToSupplierForMargin { get; set; }
        public int DepartmentId { get; set; }
    }
}
