namespace ViewModels.TMS.TeamsMgmt
{
    public class DisplayRevenueTableViewModel
    {
        public int PMId { get; set; }
        public string PMName { get; set; }
        public int ClosedOrdersCurrentYear { get; set; }
        public int ClosedOrdersCurrentMonth { get; set; }
        public int ClosedItemsCurrentYear { get; set; }
        public int ClosedItemsCurrentMonth { get; set; }
        public decimal TotalValueOfAllOpenJobOrders { get; set; }
        public decimal RecognisedRevenueCurrentMonthInGBP { get; set; }
        public decimal RecognisedRevenueCurrentYearInGBP { get; set; }
        public decimal PendingRevenueCurrentMonthInGBP { get; set; }
        public decimal MarginCurrentYear { get; set; }
    }
}
