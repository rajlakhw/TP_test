using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TPLinguistsAltairDetails: ITPLinguistsAltairDetails
    {
        private readonly IRepository<LinguistsAltairDetail> altairDetailRepository;
       
        
        public TPLinguistsAltairDetails(IRepository<LinguistsAltairDetail> repository)
        {
            this.altairDetailRepository = repository;
           
        }

        public async Task<Data.LinguistsAltairDetail> GetTINNumber(int LinguistID)
        {
            var result = await altairDetailRepository.All().Where(o => o.LinguistId == LinguistID).FirstOrDefaultAsync();
            return result;
        }
        public async Task<Data.LinguistsAltairDetail> UpdateTINNumber(int LinguistID, string TINNumber)
        {
            var result = await altairDetailRepository.All().Where(o => o.LinguistId == LinguistID).FirstOrDefaultAsync();

            
            result.Tinnumber = TINNumber;
            altairDetailRepository.Update(result);
            await altairDetailRepository.SaveChangesAsync();

            return result;
        }
    }
}
