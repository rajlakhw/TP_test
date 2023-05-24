using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class Entity
    {
        public int EntityId { get; set; }
        public int EntityType { get; set; }

        public virtual WorkAnniversariesWish Entity1 { get; set; }
        public virtual BirthdayWish EntityNavigation { get; set; }
        public virtual EntityComment EntityComment { get; set; }
        public virtual LikedEntity LikedEntity { get; set; }
    }
}
