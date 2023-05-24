using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.FileSystem;

namespace Services.Interfaces
{
    public interface ITPFileSystemService : IService
    {
        Task<IEnumerable<DownloadbleFile>> GetDownloadableDirectoryContents(string absolutePath, int dataObjectId, int dataTypeId, int sortOrder = 0);
        string GetNetworkKeyClientInfoDirectoryPath(string jobServerLocation, int orgId);
        string UnMapTPNetworkPath(string pathToUnMap);
        string GetEnquiryDirectoryPath(string quoteServerLocation, int orgId, int enquiryId);
    }
}
