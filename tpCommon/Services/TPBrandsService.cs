using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TPBrandsService : ITPBrandsService
    {
        private readonly IRepository<Brand> brandRepository;
        private readonly IRepository<OrgGroup> groupRepository;

        public TPBrandsService(IRepository<Brand> brandRepo,
                                IRepository<OrgGroup> groupRepo)
        {
            this.brandRepository = brandRepo;
            this.groupRepository = groupRepo;
        }
        public async Task<Brand> GetBrandForClient(int groupId)
        {

            var brandToReturn = await brandRepository.All().
                                        Join(groupRepository.All().Where(g => g.Id == groupId),
                                            b => b.Id,
                                            g => g.BrandId,
                                            (b, g) => new { brand = b }).FirstOrDefaultAsync();

            return brandToReturn.brand;


        }

        public async Task<Brand> GetBrandById(int brandId)
        {

            var brandToReturn = await brandRepository.All().
                                       Where(b => b.Id == (byte)brandId).FirstOrDefaultAsync();

            return brandToReturn;


        }

    }
}
