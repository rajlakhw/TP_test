using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using ViewModels.Common;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TranslationMemoriesService : ITranslationMemoriesService
    {
        private readonly IRepository<TradosTemplate> templateRepository;
        public TranslationMemoriesService(IRepository<TradosTemplate> repository)
        {
            this.templateRepository = repository;
        }
        public async Task<IEnumerable<DropdownOptionViewModel>> GetAllTradosTemplatesForAnOrg(int orgId, bool templatesToShowForiPlus)
        {
            var templates = new List<DropdownOptionViewModel>();

            var result = await templateRepository.All().Where(t => t.OrgId == orgId && t.DeletedDateTime == null &&
                                                               t.ToShowOnIplus == templatesToShowForiPlus).ToListAsync();

            for (var i = 0; i < result.Count; i++)
            {
                templates.Add(new DropdownOptionViewModel()
                {
                    Id = result.ElementAt(i).Id,
                    Name = result.ElementAt(i).TradosTemplateName,
                    StringValue = result.ElementAt(i).TradosTemplateFilePath
                });
            }

            return templates;
        }
    }
}
