using System;
using Global_Settings;

namespace ViewModels.QuoteTemplates
{
    public class QuoteTemplateTableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public string CreatedByEmployeeName { get; set; }
        public string DataObjName { get; set; }
        public int DataObjId { get; set; }
        public int DataObjType { get; set; }
        public string ApplicableToHTMLLiteral
        {
            get
            {
                string result = "???";
                switch (this.DataObjType)
                {
                    case ((int)Enumerations.DataObjectTypes.Contact):
                        result = "only the individual contact <a href=\"https://myplusbeta.publicisgroupe.net/Contact?ContactID=" + this.DataObjId+ "\">" + this.DataObjId + " - " + this.DataObjName + "</a>";
                        break;
                    case ((int)Enumerations.DataObjectTypes.Org):
                        result = "any contacts at the organisation <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id=" + this.DataObjId+ "\">" + this.DataObjId + " - " + this.DataObjName + "</a>";
                        break;
                    case ((int)Enumerations.DataObjectTypes.OrgGroup):
                        result = "any contacts at any organisation within the group <a href=\"https://myplusbeta.publicisgroupe.net/OrgGroup?GroupID=" + this.DataObjId+ "\">" + this.DataObjId + " - " + this.DataObjName + "</a>";
                        break;

                    default:
                        break;
                }

                return result;
            }
            set { }
        }
    }
}
