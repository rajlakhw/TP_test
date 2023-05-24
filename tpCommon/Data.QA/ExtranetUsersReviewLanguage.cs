using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ExtranetUsersReviewLanguage
    {
        public string ExtranetUserName { get; set; }
        public string TargetLangIanacode { get; set; }
        public bool IsLeadReviewer { get; set; }
    }
}
