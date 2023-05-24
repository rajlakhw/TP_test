using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class LikedEntity
    {
        public int EntityId { get; set; }
        public int? EntityType { get; set; }
        public short EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual BirthdayWish Entity { get; set; }
        public virtual WorkAnniversariesWish EntityNavigation { get; set; }
    }
}
