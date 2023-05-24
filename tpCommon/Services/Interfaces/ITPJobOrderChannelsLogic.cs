using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPJobOrderChannelsLogic : IService
    {
        Task<Data.JobOrderChannel> GetJobOrderChannelDetails(int ID);
        Task<List<Data.JobOrderChannel>> GetAllJobOrderChannels<JobOrderChannel>();
    }
}
