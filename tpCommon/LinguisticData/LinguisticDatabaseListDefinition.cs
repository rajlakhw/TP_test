using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class LinguisticDatabaseListDefinition
    {
        public int LinguisticDatabaseId { get; set; }
        public byte ListNumber { get; set; }
        public short ListItemIndex { get; set; }
        public string ListItemName { get; set; }
    }
}
