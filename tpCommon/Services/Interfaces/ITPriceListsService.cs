using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.PriceLists;

namespace Services.Interfaces
{
    public interface ITPriceListsService : IService
    {
        Task<IEnumerable<PriceListTableViewModel>> GetAllPriceListstForDataObjectType(int dataObjectId, int dataObjectType, bool forExternalDisplayOnly);
    }
}
