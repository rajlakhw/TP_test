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
    public class TPVolumeDiscountsService : ITPVolumeDiscountsService
    {
        private readonly IRepository<VolumeDiscount> volumeDiscountRepo;
        public TPVolumeDiscountsService(IRepository<VolumeDiscount> volumeDisRepo)
        {
            this.volumeDiscountRepo = volumeDisRepo;
        }
        public async Task<List<VolumeDiscount>> GetAllVolumeDiscountsForDataObject(int DataObjectID, byte DataObjectTypeID)
        {
            var result = await volumeDiscountRepo.All().Where(v => v.DataObjectId == DataObjectID && v.DataObjectTypeId == DataObjectTypeID).ToListAsync();

            return result;
        }

        public async Task<VolumeDiscount> GetCurrentVolumeDiscountForDataObject(int DataObjectID, byte DataObjectTypeID)
        {
            var result = await volumeDiscountRepo.All().Where(v => v.DataObjectId == DataObjectID && v.DataObjectTypeId == DataObjectTypeID && v.IsCurrentDiscount == true).FirstOrDefaultAsync();

            return result;
        }

    }
}
