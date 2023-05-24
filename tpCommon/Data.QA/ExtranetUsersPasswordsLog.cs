using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ExtranetUsersPasswordsLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public DateTime? PasswordSetDateTime { get; set; }
    }
}
