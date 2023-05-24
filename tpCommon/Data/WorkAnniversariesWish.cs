using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class WorkAnniversariesWish
    {
        public WorkAnniversariesWish()
        {
            WorkAnnivarsariesComments = new HashSet<WorkAnnivarsariesComment>();
            WorkAnniversariesLikes = new HashSet<WorkAnniversariesLike>();
        }

        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<WorkAnnivarsariesComment> WorkAnnivarsariesComments { get; set; }
        public virtual ICollection<WorkAnniversariesLike> WorkAnniversariesLikes { get; set; }
    }
}
