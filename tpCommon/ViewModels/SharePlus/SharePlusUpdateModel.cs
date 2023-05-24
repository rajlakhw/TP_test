using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.SharePlus
{
    public class SharePlusUpdateModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Htmlbody { get; set; }
        public string Contents { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmpId { get; set; }
    }
}
