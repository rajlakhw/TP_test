using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ExtranetUsersSecurityQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string LanguageIana { get; set; }
    }
}
