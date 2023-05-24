using System;
using Global_Settings;

namespace ViewModels.PriceLists
{
    public class PriceListTableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CurrencyName { get; set; }
        public DateTime InForce { get; set; }
        public int AppliesToDataObjectType { get; set; }
        public int AppliesToDataObjectId { get; set; }

        public string AppliesToName { get; set; }
        public string AppliesToTypeString
        {
            get
            {
                string link = String.Empty;
                string data = String.Empty;
                if (this.AppliesToDataObjectType == ((int)Enumerations.DataObjectTypes.Org))
                {
                   
                    data = AppliesToName;
                    link = $"https://myplusbeta.publicisgroupe.net/Organisation?Id={this.AppliesToDataObjectId}";
                                }
                else if (this.AppliesToDataObjectType == ((int)Enumerations.DataObjectTypes.Contact))
                {
                    data = AppliesToName;
                    link = $"https://myplusbeta.publicisgroupe.net/Contact?ContactID={this.AppliesToDataObjectId}";
                                }
                else if (this.AppliesToDataObjectType == ((int)Enumerations.DataObjectTypes.OrgGroup))
                {
                    data = AppliesToName;
                    link = $"https://myplusbeta.publicisgroupe.net/OrgGroup?GroupId={this.AppliesToDataObjectId}";
                }
                return "<a href=" + link + " target=" + "_blank" + ">" + data + "</a>";
            }
            set { }
        }
    }
}
