using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ClientSpecificChecklist
    {
        public int Id { get; set; }
        public short ChecklistType { get; set; }
        public short AppliesToDataObjectTypeId { get; set; }
        public int AppliesToDataObjectId { get; set; }
        public string ChecklistText { get; set; }
    }
}
