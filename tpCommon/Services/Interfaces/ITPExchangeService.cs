using System;
using System.Threading.Tasks;
using Data;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPExchangeService : IService
    {
        Task<ExchangeRate> GetHistoricalExchangeRate(short SourceCurrencyId,
                                   short TargetCurrencyId,
                                   DateTime InForceDateTime);

        decimal Convert(short SourceCurrencyId,
                                   short TargetCurrencyId,
                                   decimal InputAmount);
    }
}
