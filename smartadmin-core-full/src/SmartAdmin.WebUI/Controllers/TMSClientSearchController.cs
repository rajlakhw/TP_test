using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;

namespace SmartAdmin.WebUI.Controllers
{
    public class TMSClientSearchController : Controller
    {
        //public IActionResult ContactSearch()
        //{
        //    return View("Views/TMS/ClientSearch.cshtml");
        //}

        private IConfiguration Configuration;


        private ITPEmployeesService tpservice;
        
        private ITMSClientSearch tMSservice;


        public TMSClientSearchController(IConfiguration _configuration, ITPEmployeesService _empservice,  ITMSClientSearch _tMSservice)
        {
            Configuration = _configuration;
            tpservice = _empservice;
            tMSservice = _tMSservice;
       
        }


        public async Task<IActionResult> OrgSearch()
        {
            var model = new ViewModels.TMSClientSearch.TMSClientSearchList();
            var TechnologyNames = await tMSservice.GetTechnologies();
            model.TMSTechnologyList = new SelectList(TechnologyNames, "ID", "Name");
            model.TMSTechnologyNames = TechnologyNames;

            var CountryNames = await tMSservice.GetCountries();
            model.CountryList = new SelectList(CountryNames, "ID", "Name");
            model.CountryNames = CountryNames;

            return View("Views/flow plus/ClientSearch.cshtml", model);
        }
        public async Task<IActionResult> GroupSearch()
        {

            var model = new ViewModels.TMSClientSearch.TMSClientSearchList();
            var TechnologyNames = await tMSservice.GetTechnologies();
            model.TMSTechnologyList = new SelectList(TechnologyNames, "ID", "Name");
            model.TMSTechnologyNames = TechnologyNames;

            var CountryNames = await tMSservice.GetCountries();
            model.CountryList = new SelectList(CountryNames, "ID", "Name");
            model.CountryNames = CountryNames;

            return View("Views/flow plus/ClientSearch.cshtml", model);
        }



        public async Task<IActionResult> ContactSearch()
        {
            var model = new ViewModels.TMSClientSearch.TMSClientSearchList();
            var TechnologyNames = await tMSservice.GetTechnologies();
            model.TMSTechnologyList = new SelectList(TechnologyNames, "ID", "Name");
            model.TMSTechnologyNames = TechnologyNames;

            var CountryNames = await tMSservice.GetCountries();
            model.CountryList = new SelectList(CountryNames, "ID", "Name");
            model.CountryNames = CountryNames;

            return View("Views/flow plus/ClientSearch.cshtml", model);
            //return View("Views/TMS/ClientSearch.cshtml");
        }

        public async Task<IActionResult> Results(string rbSearch, string NumberValue, string ContactValue,
            string OrgValue, string GroupValue, string ItemType, string EmailValue, string PostcodeValue, string HFMCodeISValue,
            string HFMCodeBSValue, string CountryValues, string TechnologyValues)
        {
            var model = new ViewModels.TMSClientSearch.TMSClientSearchList();
            var TechnologyNames = await tMSservice.GetTechnologies();
            model.TMSTechnologyList = new SelectList(TechnologyNames, "ID", "Name");
            model.TMSTechnologyNames = TechnologyNames;

            var CountryNames = await tMSservice.GetCountries();
            model.CountryList = new SelectList(CountryNames, "ID", "Name");
            model.CountryNames = CountryNames;
            ViewBag.fromPage = rbSearch;
            if (NumberValue != null && NumberValue != "")
            {
                model.exactNumber = NumberValue;
                model.isNumber = true;
            }
            else
            {
                model.isNumber = false;
                model.contactValue = ContactValue;
                model.emailValue = EmailValue;
                model.postcodeValue = PostcodeValue;
                model.orgName = OrgValue;
                model.groupName = GroupValue;
                model.hfmCodeBSValue = HFMCodeBSValue;
                model.hfmCodeISValue = HFMCodeISValue;
                model.itemTypeValue = ItemType;
                model.technologyValues = TechnologyValues;
                model.countriesValue = CountryValues;

            }
            if (rbSearch == "Contact")
            {
                if (NumberValue != null && NumberValue != "")
                {
                    var SearchResults = await tMSservice.GetContactResultsByID(Int32.Parse(NumberValue));
                    model.ContactSearchList = SearchResults;
                }
                else
                {
                    var SearchResults = await tMSservice.GetContactResults(ContactValue ?? "", EmailValue ?? "", OrgValue ?? "", GroupValue ?? "", CountryValues ?? "");
                    model.ContactSearchList = SearchResults;
                }

            }
            else if (rbSearch == "OrgGroup")
            {
                if (NumberValue != null && NumberValue != "")
                {
                    var SearchResults = await tMSservice.GetOrgGroupResultsByID(Int32.Parse(NumberValue));
                    model.OrgGroupSearchList = SearchResults;
                }
                else
                {
                    var SearchResults = await tMSservice.GetOrgGroupResultsByType(ItemType, GroupValue);
                    model.OrgGroupSearchList = SearchResults;
                }

            }
            else if (rbSearch == "Org")
            {
                if (NumberValue != null && NumberValue != "")
                {
                    var SearchResults = await tMSservice.GetOrgResultsByID(NumberValue);
                    model.OrgSearchList = SearchResults;
                }
                else
                {
                    var SearchResults = await tMSservice.GetOrgResults(OrgValue, GroupValue, EmailValue, PostcodeValue, HFMCodeISValue, HFMCodeBSValue, CountryValues, TechnologyValues);
                    model.OrgSearchList = SearchResults;
                }
            }



            return View("Views/flow plus/ClientSearch.cshtml", model);
        }


    }
}
