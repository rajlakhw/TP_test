using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class NotificationRecipients
    {
        public int NotificationTypeId { get; set; }
        public short RecipientEmployeeId { get; set; }
        public int DataObjectId { get; set; }
        public byte DataObjectTypeId { get; set; }
    }
}
