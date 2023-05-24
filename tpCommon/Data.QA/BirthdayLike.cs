using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class BirthdayLike
    {
        public int Id { get; set; }
        public int BirthdayWishId { get; set; }
        public short EmployeeId { get; set; }

        public virtual BirthdayWish BirthdayWish { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
