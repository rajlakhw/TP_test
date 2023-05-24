using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Services;
using ViewModels.SharePlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SmartAdmin.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesAPIController : Controller
    {
        private readonly IConfiguration Configuration;

        public SalesAPIController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesFiguresAsync(string startdate, string enddate, string type)
        {
            DateTime StartDateTime = DateTime.Parse(startdate);
            DateTime EndDate = DateTime.Parse(enddate);
            DateTime EndDateTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59, 999);

            Double SalesFigure = GetGBPValueOfOrders(StartDateTime, EndDateTime, false, short.Parse(type));
            Double SalesFigureHistoricallyAdjusted = GetGBPValueOfOrders(StartDateTime, EndDateTime, true, short.Parse(type));

            var result = "Value of orders gone ahead during the above period: <b>£" + SalesFigure.ToString("N2") + "</b><br />" +
             "Value of orders gone ahead during the above period (historically adjusted): <b>£" + SalesFigureHistoricallyAdjusted.ToString("N2") + "</b>";           

            return Ok(result);
        }

        public Double GetGBPValueOfOrders(DateTime StartDateTime, DateTime EndDateTime, Boolean? HistoricallyAdjusted = false, short? Type = -1)
        {
            string connectionstring = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection SQLConn = new SqlConnection(connectionstring);
            SqlCommand SQLComm = new SqlCommand("procGetValueOfOrdersInGBPOverPeriod", SQLConn);
            if (HistoricallyAdjusted == true)
            {
                SQLComm = new SqlCommand("procGetValueOfOrdersInHistoricallyAdjustedGBPOverPeriod", SQLConn);
            }

            SQLComm.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            SQLComm.Parameters.Add(new SqlParameter("@StartDateTime", SqlDbType.DateTime)).Value = StartDateTime;
            SQLComm.Parameters.Add(new SqlParameter("@EndDateTime", SqlDbType.DateTime)).Value = EndDateTime;
            //If GroupTypeDropdown = "External" Then
            //    SQLComm.Parameters.Add("@IsExternal", SqlDbType.Bit).Value = 1
            //ElseIf GroupTypeDropdown = "Internal" Then
            //    SQLComm.Parameters.Add("@IsExternal", SqlDbType.Bit).Value = 0
            //Else
            if (Type == -1)
            {
                SQLComm.Parameters.Add("@IsExternal", SqlDbType.Bit).Value = null;
            }
            if (Type == 0)
            {
                SQLComm.Parameters.Add("@IsExternal", SqlDbType.Bit).Value = 0;
            }
            if (Type == 1)
            {
                SQLComm.Parameters.Add("@IsExternal", SqlDbType.Bit).Value = 1;
            }
            //End If

            SqlDataReader SQLReader = null;

            try
            {
                SQLConn.Open();
                SQLReader = SQLComm.ExecuteReader();

                // Retrieve single result and assign data
                if ((SQLReader != null) && SQLReader.Read())
                {
                    return Convert.ToDouble(SQLReader["TotalGBPValue"]);
                }
                else
                {
                    throw new Exception("Total order value could not be calculated due to a database stored procedure error.");
                }
            }
            finally
            {
                try
                {
                    if (SQLReader != null)
                    {
                        SQLReader.Close();
                    }
                    SQLConn.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw;
                }
            }
        }
    }
}
