using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Global_Settings;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.QuoteTemplates;

namespace Services
{
    public class TPQuoteTemplatesService : ITPQuoteTemplatesService
    {
        private readonly IRepository<QuoteTemplate> repository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<LocalLanguageInfo> localLangRepository;
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<OrgGroup> orgGroupRepository;

        public TPQuoteTemplatesService(IRepository<QuoteTemplate> repository, IRepository<OrgGroup> _orgGroupRepository, IRepository<Contact> contactRepository, IRepository<Employee> employeeRepository, IRepository<LocalLanguageInfo> localLangRepository, IRepository<Org> orgRepository)
        {
            this.repository = repository;
            this.employeeRepository = employeeRepository;
            this.localLangRepository = localLangRepository;
            this.orgRepository = orgRepository;
            this.contactRepository = contactRepository;
            this.orgGroupRepository = _orgGroupRepository;
        }

        public async Task<IEnumerable<QuoteTemplateTableViewModel>> GetQuoteTemplatesForDataObjectTypeAndId(int dataObjectId, int dataObjectType)
        {
            IEnumerable<QuoteTemplateTableViewModel> templates = new List<QuoteTemplateTableViewModel>();
            if (dataObjectType == ((int)Enumerations.DataObjectTypes.Org))
            {
                templates = await repository.All()
                   .Where(x => x.DataObjectId == dataObjectId &&
                   x.DeletedDateTime == null &&
                   x.DataObjectTypeId == ((int)Enumerations.DataObjectTypes.Org))

                   .Join(employeeRepository.All(), quoteTemplate => quoteTemplate.CreatedByEmployeeId, employee => employee.Id,
                   (quoteTemplate, employee) => new { quoteTemplate, employee })

                   .Join(localLangRepository.All().Where(x => x.LanguageIanacode == "en"), obj => obj.quoteTemplate.LanguageIanacode, llInfo => llInfo.LanguageIanacodeBeingDescribed, (rootObj, llInfoTable) => new { rootObj, llInfoTable })

                   .Join(orgRepository.All(), obj => obj.rootObj.quoteTemplate.DataObjectId, org => org.Id, (orgobj, orgTable) =>
                       new QuoteTemplateTableViewModel()
                   {
                       Id = orgobj.rootObj.quoteTemplate.Id,
                       Name = orgobj.rootObj.quoteTemplate.TemplateName,
                       CreatedByEmployeeId = orgobj.rootObj.quoteTemplate.CreatedByEmployeeId,
                       CreatedByEmployeeName = orgobj.rootObj.employee.FirstName + " " + orgobj.rootObj.employee.Surname,
                       CreatedOn = orgobj.rootObj.quoteTemplate.CreatedDateTime,
                       ModifiedOn = orgobj.rootObj.quoteTemplate.LastModifiedDateTime,
                       Language = orgobj.llInfoTable.Name,
                       DataObjName = orgTable.OrgName,
                       DataObjId = orgTable.Id,
                       DataObjType = dataObjectType
                   })
                   .ToListAsync();
            }
            else if (dataObjectType == ((int)Enumerations.DataObjectTypes.Contact))
            {
                templates = await repository.All()
                  .Where(x => x.DataObjectId == dataObjectId &&
                  x.DeletedDateTime == null &&
                  x.DataObjectTypeId == ((int)Enumerations.DataObjectTypes.Contact))

                  .Join(employeeRepository.All(), quoteTemplate => quoteTemplate.CreatedByEmployeeId, employee => employee.Id,
                  (quoteTemplate, employee) => new { quoteTemplate, employee })

                  .Join(localLangRepository.All().Where(x => x.LanguageIanacode == "en"), obj => obj.quoteTemplate.LanguageIanacode, llInfo => llInfo.LanguageIanacodeBeingDescribed, (rootObj, llInfoTable) => new { rootObj, llInfoTable })

                  .Join(contactRepository.All(), obj => obj.rootObj.quoteTemplate.DataObjectId, contact => contact.Id, (contactobj, contactTable) =>
                      new QuoteTemplateTableViewModel()
                      {
                          Id = contactobj.rootObj.quoteTemplate.Id,
                          Name = contactobj.rootObj.quoteTemplate.TemplateName,
                          CreatedByEmployeeId = contactobj.rootObj.quoteTemplate.CreatedByEmployeeId,
                          CreatedByEmployeeName = contactobj.rootObj.employee.FirstName + " " + contactobj.rootObj.employee.Surname,
                          CreatedOn = contactobj.rootObj.quoteTemplate.CreatedDateTime,
                          ModifiedOn = contactobj.rootObj.quoteTemplate.LastModifiedDateTime,
                          Language = contactobj.llInfoTable.Name,
                          DataObjName = contactTable.Name,
                          DataObjId = contactTable.Id,
                          DataObjType = dataObjectType
                      })
                  .ToListAsync();
            }
            else if (dataObjectType == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                templates = await repository.All()
                  .Where(x => x.DataObjectId == dataObjectId &&
                  x.DeletedDateTime == null &&
                  x.DataObjectTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))

                  .Join(employeeRepository.All(), quoteTemplate => quoteTemplate.CreatedByEmployeeId, employee => employee.Id,
                  (quoteTemplate, employee) => new { quoteTemplate, employee })

                  .Join(localLangRepository.All().Where(x => x.LanguageIanacode == "en"), obj => obj.quoteTemplate.LanguageIanacode, llInfo => llInfo.LanguageIanacodeBeingDescribed, (rootObj, llInfoTable) => new { rootObj, llInfoTable })

                  .Join(orgGroupRepository.All(), obj => obj.rootObj.quoteTemplate.DataObjectId, OrgGroup => OrgGroup.Id, (groupobj, groupTable) =>
                      new QuoteTemplateTableViewModel()
                      {
                          Id = groupobj.rootObj.quoteTemplate.Id,
                          Name = groupobj.rootObj.quoteTemplate.TemplateName,
                          CreatedByEmployeeId = groupobj.rootObj.quoteTemplate.CreatedByEmployeeId,
                          CreatedByEmployeeName = groupobj.rootObj.employee.FirstName + " " + groupobj.rootObj.employee.Surname,
                          CreatedOn = groupobj.rootObj.quoteTemplate.CreatedDateTime,
                          ModifiedOn = groupobj.rootObj.quoteTemplate.LastModifiedDateTime,
                          Language = groupobj.llInfoTable.Name,
                          DataObjName = groupTable.Name,
                          DataObjId = groupTable.Id,
                          DataObjType = dataObjectType
                      })
                  .ToListAsync();
            }

            return templates;
        }

        public async Task<QuoteTemplate> GetDefaultQuoteTemplate(string LangIANACode)
        {
            var result = await repository.All().Where(t => t.LanguageIanacode == LangIANACode && t.DeletedDateTime == null && t.DataObjectId == 0 && t.DataObjectTypeId == 0).FirstOrDefaultAsync();
            return result;
        }
    }
}
