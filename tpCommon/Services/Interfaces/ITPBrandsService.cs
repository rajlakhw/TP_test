using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPBrandsService : IService
    {
        Task<Brand> GetBrandForClient(int groupId);

        Task<Brand> GetBrandById(int brandId);
    }
}
