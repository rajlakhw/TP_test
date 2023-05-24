using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels.flowplusLicences;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Services
{
    public class TPflowplusLicencingLogic : ITPflowplusLicencingLogic
    {
        private readonly IRepository<flowPlusApplications> flowPlusApplicationsRepo;
        private readonly IRepository<flowPlusChargeFrequency> flowPlusChargesFrequencyRepo;
        private readonly IRepository<flowPlusLicenceMapping> flowPlusLicenceMappingRepo;
        private readonly IRepository<flowPlusLicences> flowPlusLicencesRepo;

        private SemaphoreSlim gate = new SemaphoreSlim(1);

        //private readonly ITPContactsLogic contactService;
        //private readonly ITPOrgsLogic orgService;
        //private readonly ITPEmployeeOwnershipsLogic ownershipsLogicService;
        //private readonly ITPJobOrderService joborderService;
        //private readonly ITPJobItemService jobitemService;
        //private readonly GlobalVariables globalVariables;
        //private readonly IConfiguration configuration;

        //private readonly IRepository<Org> orgRepo;
        //private readonly ITPJobOrderService jobOrderService;

        public TPflowplusLicencingLogic(IRepository<flowPlusApplications> _flowPlusApplicationsRepo,
                                        IRepository<flowPlusChargeFrequency> _flowPlusChargesFrequencyRepo,
                                        IRepository<flowPlusLicenceMapping> _flowPlusLicenceMappingRepo,
                                        IRepository<flowPlusLicences> _flowPlusLicencesRepo)
        {
            this.flowPlusApplicationsRepo = _flowPlusApplicationsRepo;
            this.flowPlusChargesFrequencyRepo = _flowPlusChargesFrequencyRepo;
            this.flowPlusLicenceMappingRepo = _flowPlusLicenceMappingRepo;
            //gate.Wait();
            this.flowPlusLicencesRepo = _flowPlusLicencesRepo;
            //gate.Release();
            //this.contactService = contactsLogic;
            //this.orgService = tPOrgsLogic;
            //this.ownershipsLogicService = employeeOwnershipsLogic;
            //this.joborderService = tPJobOrderService;
            //this.jobitemService = tPJobItemService;


            //this.configuration = configuration;
            //globalVariables = new GlobalVariables();
            //configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
        }

        public async Task<flowPlusLicenceMapping> GetflowPlusLicencingDetailsForDataObject(int dataObjectID, short dataObjectTypeID)
        {

            var result = await flowPlusLicenceMappingRepo.All().Where(x => x.AccessForDataObjectID == dataObjectID && x.AccessForDataObjectTypeID == dataObjectTypeID).FirstOrDefaultAsync();

            return result;
        }

        public async Task<flowPlusLicenceMapping> GetflowPlusLicencingMappingDetails(int mappingId)
        {
            var result = await flowPlusLicenceMappingRepo.All().Where(x => x.Id == mappingId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<flowPlusLicenceModel> GetflowPlusLicence(int licenceID)
        {
            //gate.Wait();
            var result = await flowPlusLicencesRepo.All().Where(x => x.Id == licenceID)
                            .Select(x => new flowPlusLicenceModel()
                            {
                                Id = x.Id,
                                ApplicationId = x.ApplicationId,
                                AppCost = x.AppCost,
                                DemoEnabled = x.DemoEnabled,
                                IsEnabled = x.IsEnabled,
                                LastEnabledDateTime = x.LastEnabledDateTime,
                                LastEnabledByEmpID = x.LastEnabledByEmpID,
                                LastModifiedDateTime = x.LastModifiedDateTime,
                                LastModifiedByEmpID = x.LastModifiedByEmpID,
                                LastDisabledDateTime = x.LastDisabledDateTime,
                                LastDisabledByEmpID = x.LastDisabledByEmpID,
                                OrderContactID = x.OrderContactID,
                                PreviousOrderSetDate = x.PreviousOrderSetDate.Value,
                                NextOrderSetDate = x.NextOrderSetDate.Value
                            }).FirstOrDefaultAsync();
            //gate.Release();
            return result;
        }

        public async Task<flowPlusLicences> GetflowPlusLicenceObj(int licenceID)
        {
            gate.Wait();
            var result = await flowPlusLicencesRepo.All().Where(x => x.Id == licenceID).FirstOrDefaultAsync();

            gate.Release();
            return result;
        }

        public async Task<flowPlusApplications> GetflowPlusAppDetails(int appId)
        {
            var result = await flowPlusApplicationsRepo.All().Where(x => x.Id == appId).FirstOrDefaultAsync();
            return result;
        }
        public async Task<List<flowPlusChargeFrequency>> GetAllFlowPlusCostFrequencies()
        {
            var result = await flowPlusChargesFrequencyRepo.All().ToListAsync();
            return result;
        }

        public async Task<flowPlusLicenceMapping> CreateFlowPlusLicenceMapping(int AccessForDataObjectID, byte AccessForDataObjectTypeID, bool CreateSingleOrderForAllLicences, string Notes,
                                                                       int? flowplusLicenceID, int? reviewPlusLicenceID, int? translateOnlineLicenceID,
                                                                       int? designPlusLicenceID, int? AIOrMTLicenceID, int? CMSLicenceID)
        {
            var thisMapping = new flowPlusLicenceMapping()
            {
                AccessForDataObjectID = AccessForDataObjectID,
                AccessForDataObjectTypeID = AccessForDataObjectTypeID,
                CreateSingleOrderForAllLicences = CreateSingleOrderForAllLicences,
                Notes = Notes,
                flowplusLicenceID = flowplusLicenceID,
                reviewPlusLicenceID = reviewPlusLicenceID,
                translateOnlineLicenceID = translateOnlineLicenceID,
                designPlusLicenceID = designPlusLicenceID,
                AIOrMTLicenceID = AIOrMTLicenceID,
                CMSLicenceID = CMSLicenceID
            };

            await flowPlusLicenceMappingRepo.AddAsync(thisMapping);
            await flowPlusLicenceMappingRepo.SaveChangesAsync();

            return thisMapping;
        }

        public async Task<flowPlusLicenceMapping> UpdateFlowPlusLicenceMapping(int AccessForDataObjectID, byte AccessForDataObjectTypeID, bool CreateSingleOrderForAllLicences, string Notes,
                                                                       int? flowplusLicenceID, int? reviewPlusLicenceID, int? translateOnlineLicenceID,
                                                                       int? designPlusLicenceID, int? AIOrMTLicenceID, int? CMSLicenceID)
        {
            var thisMapping = await GetflowPlusLicencingDetailsForDataObject(AccessForDataObjectID, AccessForDataObjectTypeID);

            thisMapping.CreateSingleOrderForAllLicences = CreateSingleOrderForAllLicences;
            thisMapping.Notes = Notes;
            thisMapping.flowplusLicenceID = flowplusLicenceID;
            thisMapping.reviewPlusLicenceID = reviewPlusLicenceID;
            thisMapping.translateOnlineLicenceID = translateOnlineLicenceID;
            thisMapping.designPlusLicenceID = designPlusLicenceID;
            thisMapping.AIOrMTLicenceID = AIOrMTLicenceID;
            thisMapping.CMSLicenceID = CMSLicenceID;

            flowPlusLicenceMappingRepo.Update(thisMapping);
            await flowPlusLicenceMappingRepo.SaveChangesAsync();

            return thisMapping;
        }

        public async Task<flowPlusLicences> CreateFlowPlusLicence(byte ApplicationID, decimal AppCost, bool DemoEnabled, bool IsEnabled, short LastEnabledByEmpID,
                                                                      int? OrderContactID)
        {
            var thisLicence = new flowPlusLicences()
            {
                ApplicationId = ApplicationID,
                AppCost = AppCost,
                DemoEnabled = DemoEnabled,
                IsEnabled = IsEnabled,
                LastEnabledDateTime = GeneralUtils.GetCurrentUKTime(),
                LastEnabledByEmpID = LastEnabledByEmpID,
                OrderContactID = OrderContactID
            };
            //gate.Wait();
            await flowPlusLicencesRepo.AddAsync(thisLicence);
            //gate.Release();

            //gate.Wait();
            await flowPlusLicencesRepo.SaveChangesAsync();
            //gate.Release();

            return thisLicence;
        }

        public async Task<flowPlusLicences> UpdateFlowPlusLicence(int LicenceId, decimal AppCost, bool DemoEnabled, bool IsEnabled, short loggedInEmployee,
                                                                      int? OrderContactID)
        {
            //gate.Wait();
            var thisLicence = await flowPlusLicencesRepo.All().Where(x => x.Id == LicenceId).FirstOrDefaultAsync();
            //gate.Release();

            thisLicence.AppCost = AppCost;
            thisLicence.OrderContactID = OrderContactID;
            thisLicence.DemoEnabled = DemoEnabled;

            if (thisLicence.IsEnabled == false && IsEnabled == true)
            {
                thisLicence.LastEnabledDateTime = GeneralUtils.GetCurrentUKTime();
                thisLicence.LastEnabledByEmpID = loggedInEmployee;

            }
            else if (thisLicence.IsEnabled == true && IsEnabled == false)
            {
                thisLicence.LastDisabledDateTime = GeneralUtils.GetCurrentUKTime();
                thisLicence.LastDisabledByEmpID = loggedInEmployee;
            }

            thisLicence.IsEnabled = IsEnabled;

            thisLicence.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
            thisLicence.LastModifiedByEmpID = loggedInEmployee;

            //gate.Wait();
            flowPlusLicencesRepo.Update(thisLicence);
            //gate.Release();

            //gate.Wait();
            await flowPlusLicencesRepo.SaveChangesAsync();
            //gate.Release();

            return thisLicence;
        }

        public async Task<flowPlusLicences> UpdateOrderSetUpDatesOfLicence(int licenceId, DateTime previousOrderCreationDate, DateTime nextOrderCreationDate)
        {
            {
                //gate.Wait();
                var thisLicence = await flowPlusLicencesRepo.All().Where(x => x.Id == licenceId).FirstOrDefaultAsync(); ;
                //gate.Release();

                if (DateTime.Now < new DateTime(2023, 04, 01))
                {
                    thisLicence.NextOrderSetDate = new DateTime(2023, 04, 01);
                }
                else
                {
                    thisLicence.PreviousOrderSetDate = previousOrderCreationDate;
                    thisLicence.NextOrderSetDate = nextOrderCreationDate;
                }


                //gate.Wait();
                flowPlusLicencesRepo.Update(thisLicence);
                //gate.Release();

                //gate.Wait();
                await flowPlusLicencesRepo.SaveChangesAsync();
                //gate.Release();

                return thisLicence;
            }

        }
    }
}
