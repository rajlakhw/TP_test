using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class Ftpdetail
    {
        public int Id { get; set; }
        public int DataObjectId { get; set; }
        public byte DataObjectTypeId { get; set; }
        public string FtpuserName { get; set; }
        public string Ftppassword { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmpId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedEmpId { get; set; }
        public bool IsSupplierLoginForClient { get; set; }
        public string MiscFtpname { get; set; }
    }
}
