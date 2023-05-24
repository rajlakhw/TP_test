using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.QuoteTemplates;
using Data;

namespace Services.Interfaces
{
    public interface ITPQuoteTemplatesService : IService
    {
        Task<IEnumerable<QuoteTemplateTableViewModel>> GetQuoteTemplatesForDataObjectTypeAndId(int dataObjectId, int dataObjectType);

        Task<QuoteTemplate> GetDefaultQuoteTemplate(string LangIANACode);
    }
}
