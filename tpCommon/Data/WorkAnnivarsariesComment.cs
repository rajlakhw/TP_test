using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class WorkAnnivarsariesComment
    {
        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public string Content { get; set; }
        public int WorkAnniversaryId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual WorkAnniversariesWish WorkAnniversary { get; set; }
    }
}
