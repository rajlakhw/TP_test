using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Contact
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string Name { get; set; }
        public short? LandlineCountryId { get; set; }
        public string LandlineNumber { get; set; }
        public short? MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
        public short? FaxCountryId { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string SkypeId { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string Notes { get; set; }
        public bool ExtranetOnlyNotifyOnDeliveryOfLastJobItem { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public bool HighLowMarginApprovalNeeded { get; set; }
        public bool ExcludeZeroMarginJobsFromApproval { get; set; }
        public int SpendFrequencyAlertDays { get; set; }
        public DateTime? SpendFrequencyAlertLastIssued { get; set; }
        public bool IncludeInNotificationsOn { get; set; }
        public string IncludeInNotifications { get; set; }
        public string InvoiceOrgName { get; set; }
        public string InvoiceAddress1 { get; set; }
        public string InvoiceAddress2 { get; set; }
        public string InvoiceAddress3 { get; set; }
        public string InvoiceAddress4 { get; set; }
        public string InvoiceCountyOrState { get; set; }
        public string InvoicePostcodeOrZip { get; set; }
        public short? InvoiceCountryId { get; set; }
        public DateTime? EmailUnsubscribedDateTime { get; set; }
        public string EmailUnsubscribedReason { get; set; }
        public DateTime? GdpracceptedDateTime { get; set; }
        public bool? OptedInForMarketingCampaign { get; set; }
        public DateTime? LastRespondedToOptInPopup { get; set; }
        public bool? GdpracceptedViaIplus { get; set; }
        public DateTime? GdprrejectedDateTime { get; set; }
        public bool? GdprverballyAccepted { get; set; }
        public byte? Gdprstatus { get; set; }
    }
}
