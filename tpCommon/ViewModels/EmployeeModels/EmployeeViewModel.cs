using System;
using System.Collections.Generic;
using ViewModels.EmployeeModels.EmployeeAccessMatrix;

namespace ViewModels.EmployeeModels
{
    public class EmployeeViewModel
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
        public short? AccessLevel { get; set; }
        public bool? ShowAnniversariesOnHomePage { get; set; }
        public bool? ShowLikesOrCommentsOnHomePage { get; set; }

        //additional props
        public Data.EmployeeDepartment Department { get; set; }
        public bool AttendsBoardMeetings
        {
            get
            {
                var DoesAttend = false;
                if(this.Department.Id == ((byte)Global_Settings.Enumerations.Departments.CompanyDirectors) ||
                this.EmailAddress == "Adrian.Metcalf@translateplus.com" ||
                this.EmailAddress == "Liina.Puust@translateplus.com" ||
                this.EmailAddress == "Svenja.Muller@translateplus.com" ||
                this.EmailAddress == "Umer.Nizam@translateplus.com")
                    DoesAttend = true;
                return DoesAttend;
            }
            private set { }
        }
        public IEnumerable<AccessLevelConrolsForEmployeeViewModel> AccessLevelControls { get; set; }
    }
}
