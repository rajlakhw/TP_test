using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ViewModels.flowplusLicences;


namespace SmartAdmin.WebUI.ViewComponents
{
    public class flowPlusLicencingViewComponent : ViewComponent
    {
        private readonly ITPOrgsLogic orgsService;
        private readonly ITPOrgGroupsLogic groupService;
        private readonly ITPContactsLogic contactsService;
        private readonly ITPCurrenciesLogic curencyService;
        private readonly ITPflowplusLicencingLogic flowplusLicenceService;

        public flowPlusLicencingViewComponent(ITPOrgsLogic orgsService, ITPContactsLogic contactsService,
                                              ITPCurrenciesLogic curencyService,
                                              ITPflowplusLicencingLogic flowplusLicenceService,
                                              ITPOrgGroupsLogic groupService)
        {
            this.orgsService = orgsService;
            this.contactsService = contactsService;
            this.curencyService = curencyService;
            this.flowplusLicenceService = flowplusLicenceService;
            this.groupService = groupService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int orgOrGroupId, int typeId)
        {


            ViewBag.DataObjectID = orgOrGroupId;
            ViewBag.DataObjectTypeID = typeId;

            var flowplusLicencingDetails = await flowplusLicenceService.GetflowPlusLicencingDetailsForDataObject(orgOrGroupId, (short)typeId);

            var allOrgsFromGroup = new List<string>();
            var allContactsToSelectFrom = new List<string>();

            if (typeId == 2)
            {
                var org = await orgsService.GetOrg(orgOrGroupId);
                ViewBag.OrgCurrencySymbol = org.InvoiceCurrencyId == null ? "£" : curencyService.GetById(org.InvoiceCurrencyId.Value).Result.Prefix;
                allContactsToSelectFrom = await contactsService.GetAllContactIdAndNameStringForOrg(orgOrGroupId);
                if (org.OrgGroupId == null)
                {
                    ViewBag.flowplusLicencingDisabled = false;
                }
                else
                {
                    ViewBag.flowplusLicencingDisabled = await groupService.CheckIfGroupLevelLicenceExists(org.OrgGroupId.Value);
                }

            }
            else if (typeId == 3)
            {
                allOrgsFromGroup = await orgsService.GetAllOrgsForOrgGroupString(orgOrGroupId.ToString(), false);
                allContactsToSelectFrom = await contactsService.GetAllContactIdAndNameStringForGroup(orgOrGroupId);
                ViewBag.flowplusLicencingDisabled = await groupService.CheckOrgLevelLicencesForGroup(orgOrGroupId);
                //var org = await orgsService.GetOrg(orgOrGroupId);
                //ViewBag.OrgCurrencySymbol = org.InvoiceCurrencyId == null ? "£" : curencyService.GetById(org.InvoiceCurrencyId.Value).Result.Prefix;
            }

            var flowPlusLicenceMapping = new flowplusLicenceMappingModel();
            if (flowplusLicencingDetails != null)
            {
                flowPlusLicenceMapping = new flowplusLicenceMappingModel()
                {
                    Id = flowplusLicencingDetails.Id,
                    AccessForDataObjectID = flowplusLicencingDetails.AccessForDataObjectID,
                    AccessForDataObjectTypeID = flowplusLicencingDetails.AccessForDataObjectTypeID,
                    Notes = flowplusLicencingDetails.Notes,
                    flowplusLicence = flowplusLicencingDetails.flowplusLicenceID == null ? null : await flowplusLicenceService.GetflowPlusLicence(flowplusLicencingDetails.flowplusLicenceID.Value),
                    reviewPlusLicence = flowplusLicencingDetails.reviewPlusLicenceID == null ? null : await flowplusLicenceService.GetflowPlusLicence(flowplusLicencingDetails.reviewPlusLicenceID.Value),
                    translateOnlineLicence = flowplusLicencingDetails.translateOnlineLicenceID == null ? null : await flowplusLicenceService.GetflowPlusLicence(flowplusLicencingDetails.translateOnlineLicenceID.Value),
                    designPlusLicence = flowplusLicencingDetails.designPlusLicenceID == null ? null : await flowplusLicenceService.GetflowPlusLicence(flowplusLicencingDetails.designPlusLicenceID.Value),
                    AIOrMTLicence = flowplusLicencingDetails.AIOrMTLicenceID == null ? null : await flowplusLicenceService.GetflowPlusLicence(flowplusLicencingDetails.AIOrMTLicenceID.Value),
                    CMSLicence = flowplusLicencingDetails.CMSLicenceID == null ? null : await flowplusLicenceService.GetflowPlusLicence(flowplusLicencingDetails.CMSLicenceID.Value),
                    CreateSingleOrderForAllLicences = flowplusLicencingDetails.CreateSingleOrderForAllLicences
                };
            }

            flowPlusLicenceMapping.AllContacts = allContactsToSelectFrom;
            flowPlusLicenceMapping.AllOrgs = allOrgsFromGroup;


            return View(flowPlusLicenceMapping);
        }


    }
}
