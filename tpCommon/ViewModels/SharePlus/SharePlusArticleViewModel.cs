using System;
using ViewModels.EmployeeModels;

namespace ViewModels.SharePlus
{
    public class SharePlusArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Htmlbody { get; set; }
        public string Contents { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmpId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string LastModifiedByEmpName { get; set; }
        public DateTime? LastViewedDateTime { get; set; }
        public short? LastViewedByEmpId { get; set; }
        public int? HistoricalNumberOfViews { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public string CreatedByEmployeeFirstName { get; set; }
        public string CreatedByEmployeeSurname { get; set; }
        public string CreatedByEmployeeImageBase64 { get; set; }
        public EmployeeViewModel EmployeeLoggedIn { get; set; }
        public int EmployeeLoggedInDepartmentId { get; set; }
        public bool EmployeeMarkedArticleAsHelpful { get; set; }
        public int HelpfulCount { get; set; }
        public bool IsAllowedToEdit { get; set; }
    }
}
