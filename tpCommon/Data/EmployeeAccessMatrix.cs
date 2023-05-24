using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeAccessMatrix
    {
        public EmployeeAccessMatrix()
        {
            AccessControls = new HashSet<AccessControl>();
            Employees = new HashSet<Employee>();
        }

        public short Id { get; set; }
        public short EmployeeAccessLevel { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<AccessControl> AccessControls { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
