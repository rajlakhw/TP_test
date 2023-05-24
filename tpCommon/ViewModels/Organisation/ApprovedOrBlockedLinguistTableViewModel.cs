using System;
using Global_Settings;

namespace ViewModels.Organisation
{
    public class ApprovedOrBlockedLinguistTableViewModel
    {
        public string LanguageServiceName { get; set; }
        public string SourceLangName { get; set; }
        public string TargetLangName { get; set; }
        public int LinguistId { get; set; }
        public string LinguistName { get; set; }
        public int Status { get; set; }
        public int WorkingPatternId { get; set; }
        public short? WorkingTimeStartHours { get; set; }
        public short? WorkingTimeStartMinutes { get; set; }
        public short? WorkingTimeEndHours { get; set; }
        public short? WorkingTimeEndMinutes { get; set; }
        public string SoftwareApplication { get; set; }
        public int AppliesToDataObjectTypeID { get; set; }
        public int AppliesToDataObjectID { get; set; }
        public string Notes { get; set; }
        public string WorkingTime
        {
            get
            {
                if (this.WorkingTimeStartHours != null && this.WorkingTimeStartMinutes != null && this.WorkingTimeEndHours != null && this.WorkingTimeEndMinutes != null)
                {
                    return string.Format("{0} to {1}", $"{this.WorkingTimeStartHours?.ToString("D2")}:{this.WorkingTimeStartMinutes?.ToString("D2")}", $"{this.WorkingTimeEndHours?.ToString("D2")}:{this.WorkingTimeEndMinutes?.ToString("D2")}");
                }
                else { return ""; }
            }
            set{}
        }
        public string StatusString
        {
            get
            {
                return this.Status switch
                {
                    ((int)Enumerations.SupplierStatus.Blocked) => "Blocked (don't use for this client)",
                    ((int)Enumerations.SupplierStatus.TemporarilyUnavailable) => "Temporarily unavailable",
                    ((int)Enumerations.SupplierStatus.ApprovedByTheEndClient) => "Approved (by the client)",
                    ((int)Enumerations.SupplierStatus.ApprovedByUsOnly) => "Approved (by us, but not by the client)",
                    _ => "",
                };
            }
            set { }
        }

        //public string StatusString
        //{
        //    get
        //    {
        //        return this.Status switch
        //        {
        //            ((int)Enumerations.SupplierStatus.Blocked) => "<span style='color: red;'>Blocked (don't use for this client)</span>",
        //            ((int)Enumerations.SupplierStatus.TemporarilyUnavailable) => "<span style='color: red;'>Temporarily unavailable</span>",
        //            ((int)Enumerations.SupplierStatus.ApprovedByTheEndClient) => "<span style='color: green;'>Approved (by the client)</span>",
        //            ((int)Enumerations.SupplierStatus.ApprovedByUsOnly) => "<span style='color: green;'>Approved (by us, but not by the client)</span>",
        //            _ => "",
        //        };
        //    }
        //    set { }
        //}
        public string WorkingPatternString
        {
            get
            {
                switch (this.WorkingPatternId)
                {
                    case ((int)Enumerations.WorkingPattern.AnyTime):
                        return "Any day/time";
                    case ((int)Enumerations.WorkingPattern.WorkingDaysOnly):
                        return "Working days only (Mon-Fri excl. holidays)";
                    case ((int)Enumerations.WorkingPattern.WeekendsAndHolidayModeDaysOnly):
                        return "Weekends and holidays only";
                    case ((int)Enumerations.WorkingPattern.WeekendsAndHolidayModeDaysOnlyAndFridayAfternoons):
                        return "Weekends, holidays and Friday afternoons only";
                    default:
                        return "";
                }
            }
            set{}
        }
        public string AppliesToTypeString
        {
            get
            {
                string link = String.Empty;
                string data = String.Empty;
                if (this.AppliesToDataObjectTypeID == ((int)Enumerations.DataObjectTypes.Org))
                {
                    data = "Organisation";
                    link = $"https://myplusbeta.publicisgroupe.net/Organisation?Id={this.AppliesToDataObjectID}";
                }
                else if (this.AppliesToDataObjectTypeID == ((int)Enumerations.DataObjectTypes.Contact))
                {
                    data = "Contact";
                    link = $"https://myplusbeta.publicisgroupe.net/Contact?ContactID={this.AppliesToDataObjectID}";
                }
                return "<a href=" + link + " target='_blank' >" + data + "</a>";
            }
            set { }
        }
    }
}
