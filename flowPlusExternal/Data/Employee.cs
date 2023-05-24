using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Employee
    {
        public short Id { get; set; }
        public string NetworkUserName { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminateDate { get; set; }
        public string EmailAddress { get; set; }
        public string LandlineNumber { get; set; }
        public string InternalExtension { get; set; }
        public string MobileNumber { get; set; }
        public bool MobileNumberIsPersonalOnly { get; set; }
        public string Notes { get; set; }
        public byte TeamId { get; set; }
        public short? Manager { get; set; }
        public bool? IsTeamManager { get; set; }
        public decimal SalesNewBusinessCommissionPercentage { get; set; }
        public decimal SalesAccountManagementCommissionPercentage { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte? OfficeId { get; set; }
        public string WorkingHoursNotes { get; set; }
        public string NextOfKinName { get; set; }
        public string NextOfKinAddress { get; set; }
        public string NextOfKinContactPhoneNumber { get; set; }
        public string SkypeId { get; set; }
        public string LinkedInUrl { get; set; }
        public string JobTitle { get; set; }
        public bool IncludeInPerformancePlusFigures { get; set; }
        public bool IsSalesAccountManager { get; set; }
        public byte? EmployeeStatusType { get; set; }
        public int? HighFiveCount { get; set; }
        public string HrhighFiveNotes { get; set; }
        public int? ProbationDuration { get; set; }
        public DateTime? ResignationSubmitted { get; set; }
        public int? NoticePeriod { get; set; }
        public string AdditionalLoyaltyPerks { get; set; }
        public string ImageBase64 { get; set; }
    }
}
