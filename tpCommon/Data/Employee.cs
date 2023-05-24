using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Employee
    {
        public Employee()
        {
            BirthdayComments = new HashSet<BirthdayComment>();
            BirthdayLikes = new HashSet<BirthdayLike>();
            BirthdayWishes = new HashSet<BirthdayWish>();
            WorkAnnivarsariesComments = new HashSet<WorkAnnivarsariesComment>();
            WorkAnniversariesLikes = new HashSet<WorkAnniversariesLike>();
            WorkAnniversariesWishes = new HashSet<WorkAnniversariesWish>();
        }

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

        public virtual EmployeeAccessMatrix AccessLevelNavigation { get; set; }
        public virtual ICollection<BirthdayComment> BirthdayComments { get; set; }
        public virtual ICollection<BirthdayLike> BirthdayLikes { get; set; }
        public virtual ICollection<BirthdayWish> BirthdayWishes { get; set; }
        public virtual ICollection<WorkAnnivarsariesComment> WorkAnnivarsariesComments { get; set; }
        public virtual ICollection<WorkAnniversariesLike> WorkAnniversariesLikes { get; set; }
        public virtual ICollection<WorkAnniversariesWish> WorkAnniversariesWishes { get; set; }
    }
}
