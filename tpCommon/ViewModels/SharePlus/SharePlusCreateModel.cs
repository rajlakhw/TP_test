using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ViewModels.SharePlus
{
    public class SharePlusCreateModel
    {
        [Required]
        public string Title { get; set; }
        public string Htmlbody { get; set; }
        public string Contents { get => Regex.Replace(Htmlbody, "<.*?>", ""); }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmpId { get; set; }
        public bool IsPinnedArticle { get; set; }
    }
}
