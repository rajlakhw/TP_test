using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Services.Interfaces;
using Global_Settings;

namespace Services
{
    public class TPExchangeService : ITPExchangeService
    {
        private readonly IRepository<ExchangeRate> repository;
        private IConfiguration Configuration;
        private readonly GlobalVariables globalVariables;

        public TPExchangeService(IRepository<ExchangeRate> repository, IConfiguration _configuration)
        {
            this.repository = repository;
            Configuration = _configuration;
            globalVariables = new GlobalVariables();
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
        }

        public async Task<ExchangeRate> GetHistoricalExchangeRate(short SourceCurrencyId,
                                   short TargetCurrencyId,
                                   DateTime InForceDateTime)
        {
            var result = await repository.All().Where(a => a.SourceCurrencyId == SourceCurrencyId && a.TargetCurrencyId == TargetCurrencyId && InForceDateTime >= a.CreatedDate).FirstOrDefaultAsync();
            return result;
        }

        public decimal Convert(short SourceCurrencyId,
                                   short TargetCurrencyId,
                                   decimal InputAmount)
        {
            decimal convertedamount = 0;
            //string connectionstring = Configuration.GetConnectionString(GlobalVars.CurrentAppModeString);
            SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
            SqlDataAdapter ConversionDataAdapter = new SqlDataAdapter("procCurrencyConversion", SQLConn);

            ConversionDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            ConversionDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@SourceCurrencyID", SqlDbType.SmallInt)).Value = SourceCurrencyId;
            ConversionDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@TargetCurrencyID", SqlDbType.SmallInt)).Value = TargetCurrencyId;
            // SQLComm.Parameters.Add(new SqlParameter("@InputAmount", SqlDbType.Decimal)).Value = InputAmount;
            SqlParameter InputAmountParameter = ConversionDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@InputAmount", SqlDbType.Decimal));
            InputAmountParameter.Precision = 18;
            InputAmountParameter.Scale = 9;
            InputAmountParameter.Value = InputAmount;

            SqlParameter ConversionOutputParameter =
            ConversionDataAdapter.SelectCommand.Parameters.Add(
            new SqlParameter("@OutputAmount", SqlDbType.Decimal));
            ConversionOutputParameter.Precision = 38;
            ConversionOutputParameter.Scale = 18;
            ConversionOutputParameter.Direction = ParameterDirection.Output;

            try
            {
                ConversionDataAdapter.SelectCommand.Connection.Open();
                ConversionDataAdapter.SelectCommand.ExecuteScalar();
                convertedamount = (decimal)ConversionOutputParameter.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error running stored procedure to convert a currency value.");
            }
            finally
            {
                try
                {
                    ConversionDataAdapter.SelectCommand.Connection.Close();
                    ConversionDataAdapter.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw;
                }
            }

            return convertedamount;
        }
    }
}
