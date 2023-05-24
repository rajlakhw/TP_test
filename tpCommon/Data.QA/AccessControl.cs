using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class AccessControl
    {
        public int Id { get; set; }
        public short EmployeeAccessLevelId { get; set; }
        public int AccessMatrixControlId { get; set; }

        public virtual EmployeeAccessMatrixControl AccessMatrixControl { get; set; }
        public virtual EmployeeAccessMatrix EmployeeAccessLevel { get; set; }
    }
}
