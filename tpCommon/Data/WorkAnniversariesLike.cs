using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class WorkAnniversariesLike
    {
        public int Id { get; set; }
        public int WorkAnniversaryId { get; set; }
        public short EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual WorkAnniversariesWish WorkAnniversary { get; set; }
    }
}
