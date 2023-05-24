using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class EmployeeAccessMatrixControl
    {
        public EmployeeAccessMatrixControl()
        {
            AccessControls = new HashSet<AccessControl>();
        }

        public int Id { get; set; }
        public string Page { get; set; }
        public string ControlName { get; set; }

        public virtual ICollection<AccessControl> AccessControls { get; set; }
    }
}
