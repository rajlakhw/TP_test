using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class TPJobOrderChannelsLogic : ITPJobOrderChannelsLogic
    {
        private readonly IRepository<JobOrderChannel> joborderchannelRepository;
        public TPJobOrderChannelsLogic(IRepository<JobOrderChannel> repository)
        {
            this.joborderchannelRepository = repository;
        }
        public async Task<JobOrderChannel> GetJobOrderChannelDetails(int ID)
        {
            var result = await joborderchannelRepository.All().Where(a => a.Id == ID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Data.JobOrderChannel>> GetAllJobOrderChannels<JobOrderChannel>()
        {
            var result = await joborderchannelRepository.All().Where(a => a.Active == true).OrderBy(x => x.Name).ToListAsync();
            return result;
        }
    }
}
