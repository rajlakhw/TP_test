using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.flowplusLicences
{
    public class flowplusLicenceMappingModel
    {
        public int Id { get; set; }
        public int AccessForDataObjectID { get; set; }
        public Byte AccessForDataObjectTypeID { get; set; }
        public bool CreateSingleOrderForAllLicences { get; set; }
        public string Notes { get; set; }
        public flowPlusLicenceModel flowplusLicence { get; set; }
        public flowPlusLicenceModel reviewPlusLicence { get; set; }
        public flowPlusLicenceModel translateOnlineLicence { get; set; }
        public flowPlusLicenceModel designPlusLicence { get; set; }
        public flowPlusLicenceModel AIOrMTLicence { get; set; }
        public flowPlusLicenceModel CMSLicence { get; set; }
        public List<string> AllOrgs { get; set; }
        public List<string> AllContacts { get; set; }

    }

    public class flowPlusLicenceModel
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public decimal AppCost { get; set; }
        public bool DemoEnabled { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime LastEnabledDateTime { get; set; }
        public short LastEnabledByEmpID { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmpID { get; set; }
        public DateTime? LastDisabledDateTime { get; set; }
        public short? LastDisabledByEmpID { get; set; }
        public int? OrderContactID { get; set; }
        public DateTime? PreviousOrderSetDate { get; set; }
        public DateTime? NextOrderSetDate { get; set; }
    }



}
