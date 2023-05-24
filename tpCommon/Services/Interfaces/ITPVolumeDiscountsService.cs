using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface ITPVolumeDiscountsService : IService
    {
        Task<List<VolumeDiscount>> GetAllVolumeDiscountsForDataObject(int DataObjectID, byte DataObjectTypeID);
        Task<VolumeDiscount> GetCurrentVolumeDiscountForDataObject(int DataObjectID, byte DataObjectTypeID);


    }
}
