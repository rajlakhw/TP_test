using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ExpenseCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
