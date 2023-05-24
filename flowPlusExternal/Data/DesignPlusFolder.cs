using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class DesignPlusFolder
    {
        public int Id { get; set; }
        public string FolderName { get; set; }
        public bool IsTranslationFolder { get; set; }
        public int? ParentFolderId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedBy { get; set; }
        public int ProjectId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedBy { get; set; }
    }
}
