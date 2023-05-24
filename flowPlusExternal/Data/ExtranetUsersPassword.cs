using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ExtranetUsersPassword
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool? IsActivePassword { get; set; }
        public DateTime? PasswordSetDateTime { get; set; }
        public DateTime? PasswordExpiryDateTime { get; set; }
    }
}
