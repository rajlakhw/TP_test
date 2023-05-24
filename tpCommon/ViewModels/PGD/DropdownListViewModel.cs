using System;

namespace ViewModels.PGD
{
    public class DropdownListViewModel
    {
        public int Id { get; set; }
        public string VodropdownListName { get; set; }
        public bool Required { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByEmployeeId { get; set; }
    }
}
