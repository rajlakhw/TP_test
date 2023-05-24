using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Data
{
    public partial class LinguisticSupplierSoftwareApplication
    {
        [Key]
        public int LinguisticSupplierId { get; set; }
        public short SoftwareApplicationId { get; set; }
    }
}
