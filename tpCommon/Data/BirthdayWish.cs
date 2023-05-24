using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class BirthdayWish
    {
        public BirthdayWish()
        {
            BirthdayComments = new HashSet<BirthdayComment>();
            BirthdayLikes = new HashSet<BirthdayLike>();
        }

        public int Id { get; set; }
        public short EmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<BirthdayComment> BirthdayComments { get; set; }
        public virtual ICollection<BirthdayLike> BirthdayLikes { get; set; }
    }
}
