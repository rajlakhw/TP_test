using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public partial class flowPlusLicenceMapping
    {
        public int Id { get; set; }
        public int AccessForDataObjectID { get; set; }
        public Byte AccessForDataObjectTypeID { get; set; }
        public string Notes { get; set; }
        public int? flowplusLicenceID { get; set; }
        public int? reviewPlusLicenceID { get; set; }
        public int? translateOnlineLicenceID { get; set; }
        public int? designPlusLicenceID { get; set; }
        public int? AIOrMTLicenceID { get; set; }
        public int? CMSLicenceID { get; set; }
        public bool CreateSingleOrderForAllLicences { get; set; }
    }
}
