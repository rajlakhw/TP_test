using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class EmployeeAccessMatrix
    {
        public short Id { get; set; }
        public short EmployeeAccessLevel { get; set; }
        public string Notes { get; set; }
    }
}
