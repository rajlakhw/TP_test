using System;

namespace ViewModels.EmployeeOwnerships
{
    public class EmployeeOwnershipDetailsViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeJobTitle { get; set; }
        public int OwnershipId { get; set; }
        public string ProfilePicture { get; set; }
        public string OwnershipType { get; set; }
        public int OwnershipTypeId { get; set; }
        public DateTime InForceSince { get; set; }
        public String InForceSinceString { get; set; }
        public String EmployeeOwnershipTypeName { get; set; }
        public int OwnershipOrgId { get; set; }
        public String OwnershipOrgName { get; set; }
        public String CommissionPercentage { get; set; }
    }

    public class OwnershipDataTable
    {
        public ParametersForDataTable parameters { get; set; }
        public int dataObjectId { get; set; }
        public int dataTypeId { get; set; }
    }

    public class ParametersForDataTable
    {
        public int start { get; set; }
        public int length { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}
