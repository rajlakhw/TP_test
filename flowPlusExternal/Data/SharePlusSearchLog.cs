using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class SharePlusSearchLog
    {
        public int Id { get; set; }
        public string SearchText { get; set; }
        public DateTime SearchedDateTime { get; set; }
        public short SearchedByEmpId { get; set; }
        public bool IsSuccessfulSearch { get; set; }
    }
}
