using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data;

namespace ViewModels.TMSClientSearch
{
    public class LinguistSearchViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class TMSClientTechnology
    {
        public string TechnologyName { get; set; }
        public int Id { get; set; }
    }

    public class OrgSearchResults
    {
        public int orgId { get; set; }
        public string orgName { get; set; }
        public string orgAddress { get; set; }
        public string orgCountry { get; set; }
        public string orgGroup { get; set; }
        public int orgGroupID { get; set; }
        public string orgEmail { get; set; }
        public int orgSalesLeadId { get; set; }
        public int orgEnqLeadId { get; set; }
        public int orgOpsLeadId { get; set; }
        public string orgSalesLeadName { get; set; }
        public string orgEnqLeadName { get; set; }
        public string orgOpsLeadName { get; set; }


    }
    public class ContactResults
    {
        public int contactID { get; set; }
        public string contactName { get; set; }
        public string orgName { get; set; }
        public string contactCountry { get; set; }
        public string orgGroup { get; set; }
        public string contactLandline { get; set; }
        public string contactMobile { get; set; }
        public string contactEmail { get; set; }
        public int orgID { get; set; }
        public int orgGroupID { get; set; }
        public byte? gdprStatusID { get; set; }
        public string contactTitle { get; set; }
    }

    public class OrgGroupResults
    {
        public int orgGroupID { get; set; }
        public string orgGroup { get; set; }
        public int numberOfMembers { get; set; }
    }

    public class TMSClientSearchList
    {
        public SelectList TMSTechnologyList { get; set; }
        public List<TMSClientTechnology> TMSTechnologyNames { get; set; }
        public List<TMSCountry> CountryNames { get; set; }
        public SelectList CountryList { get; set; }
        public int CountryID { get; set; }
        public int TMSTechnologyId { get; set; }
        public List<OrgSearchResults> OrgSearchList { get; set; }
        public List<ContactResults> ContactSearchList { get; set; }
        public List<OrgGroupResults> OrgGroupSearchList { get; set; }
        public bool isNumber { get; set; }
        public string exactNumber  { get; set; }
        public string orgName { get; set; }
        public string groupName { get; set; }
        public string emailValue { get; set; }
        public string postcodeValue { get; set; }
        public string contactValue { get; set; }
        public string itemTypeValue { get; set; }
        public string hfmCodeISValue { get; set; }
        public string hfmCodeBSValue { get; set; }
        public string countriesValue { get; set; }
        public string technologyValues { get; set; }
    }

    public class TMSCountry
    {
        public string CountryName { get; set; }
        public int Id { get; set; }
    }


}
