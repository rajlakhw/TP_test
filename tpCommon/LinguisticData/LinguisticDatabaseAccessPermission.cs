using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class LinguisticDatabaseAccessPermission
    {
        public int LinguisticDatabaseId { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public byte AccessLevel { get; set; }
    }
}
