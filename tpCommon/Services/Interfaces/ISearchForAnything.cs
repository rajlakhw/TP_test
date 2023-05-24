using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.SearchForAnything;

namespace Services.Interfaces
{
    public interface ISearchForAnything : IService
    {
        Task<SearchForAnythingResults> GetAllResults(string searchedFor);
    }
}
