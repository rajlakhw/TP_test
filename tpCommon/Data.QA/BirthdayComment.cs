using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class BirthdayComment
    {
        public int Id { get; set; }
        public int BirthdayWishId { get; set; }
        public string Content { get; set; }
        public short EmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public virtual BirthdayWish BirthdayWish { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
