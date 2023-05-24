using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Data;
using Services;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace SmartAdmin.WebUI.Controllers
{

    public class SalesController : Controller
    {
        private String SalesFigureMessage = "";
        private String SalesFigureHistoricallyAdjustedMessage = "";

        public Int16 TypeToUse = -1;

        private IConfiguration Configuration;
        private ITPEmployeesService tpservice;

        public SalesController(IConfiguration _configuration, ITPEmployeesService _service)
        {
            Configuration = _configuration;
            tpservice = _service;
        }

        public async Task<IActionResult> SalesIntro()
        {
            //var employeeLoggedIn = HttpContext.Session.Get("EmployeeLoggedIn");
            //string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split("\\")[1];
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            //int EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUserID<int>(LogonUserName: username);
            Employee EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUser<Employee>(LogonUserName: username);
            // int rowno = 0;
            // int cellno = 0;
            // string labelname = "Row" + rowno + "Cell" + cellno + "Label";
            //ViewData[labelname] = "Row0Cell0Label Text";
            //Configuration.GetConnectionString("QAandCustomerWSTesting");
            // ViewBag.test = "<b>" + GetGBPValueOfOrders(new DateTime(2021, 08, 30), new DateTime(2021, 09, 06)) + "</b>";


            if (EmployeeCurrentlyLoggedIn.AccessLevel == 3 || EmployeeCurrentlyLoggedIn.AccessLevel == 4)
                //if (EmployeeCurrentlyLoggedIn == 41 || EmployeeCurrentlyLoggedIn == 22 || EmployeeCurrentlyLoggedIn == 1298 || EmployeeCurrentlyLoggedIn == 1305 || EmployeeCurrentlyLoggedIn == 475 || EmployeeCurrentlyLoggedIn == 185)
                
            {
                DateTime CurrentTimeAtTimeZone = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                //ViewBag.Type = TypeToUse;
                //try
                //{
                //    var DataPassedOver = HttpContext.Request.Body;
                //    var streamreader = new StreamReader(DataPassedOver);
                //    var content = streamreader.ReadToEndAsync();
                //    Type = Convert.ToInt16(content.Result);
                //}
                //catch { }

                //ViewBag.Type = Type;
                //if (salesfigures != "")
                //{
                //    ViewBag.SalesFigure = salesfigures.Split("¦")[0];
                //    ViewBag.SalesFigureHistoricallyAdjusted = salesfigures.Split("¦")[1];
                //    ViewBag.StartDateSelected = DateTime.Parse(salesfigures.Split("¦")[2]).ToString("yyyy-MM-dd");
                //    ViewBag.EndDateSelected = DateTime.Parse(salesfigures.Split("¦")[3]).ToString("yyyy-MM-dd");
                //}
                //else
                //{
                DateTime PrevMonthStart = CurrentTimeAtTimeZone.AddMonths(-1).AddDays(-(CurrentTimeAtTimeZone.Day - 1));
                    DateTime PrevMonthEnd = CurrentTimeAtTimeZone.AddDays(-(CurrentTimeAtTimeZone.Day));

                    ViewBag.StartDateSelected = PrevMonthStart.ToString("yyyy-MM-dd");
                    ViewBag.EndDateSelected = PrevMonthEnd.ToString("yyyy-MM-dd");
               // }
                                       

                    DateTime TodaysDate = CurrentTimeAtTimeZone;

                    while (TodaysDate.DayOfWeek != DayOfWeek.Monday)
                    {
                        TodaysDate = TodaysDate.AddDays(-1);
                    }
                    DateTime StartDateOfCurrentWeek = new DateTime(year: TodaysDate.Year, month: TodaysDate.Month, day: TodaysDate.Day,
                                         hour: 0, minute: 0, second: 0, millisecond: 0);

                    TodaysDate = CurrentTimeAtTimeZone;

                    while (TodaysDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        TodaysDate = TodaysDate.AddDays(1);
                    }
                    DateTime EndDateOfCurrentWeek = new DateTime(year: TodaysDate.Year, month: TodaysDate.Month, day: TodaysDate.Day,
                                         hour: 23, minute: 59, second: 59, millisecond: 999);


                    ViewBag.ThisWeekDatesLabel = "(" + StartDateOfCurrentWeek.ToString("d MMM yy") +
                                            " &ndash; " + EndDateOfCurrentWeek.ToString("d MMM yy") + ")";

                    ViewBag.LastWeekDatesLabel = "(" + StartDateOfCurrentWeek.AddDays(-7).ToString("d MMM yy") +
                                            " &ndash; " + EndDateOfCurrentWeek.AddDays(-7).ToString("d MMM yy") + ")";

                    ViewBag.PreviousWeek1kDatesLabel = "(" + StartDateOfCurrentWeek.AddDays(-14).ToString("d MMM yy") +
                                                " &ndash; " + EndDateOfCurrentWeek.AddDays(-14).ToString("d MMM yy") + ")";

                    ViewBag.PreviousWeek2kDatesLabel = "(" + StartDateOfCurrentWeek.AddDays(-21).ToString("d MMM yy") +
                                                " &ndash; " + EndDateOfCurrentWeek.AddDays(-21).ToString("d MMM yy") + ")";


                ViewBag.EndColumnHeaderLabel = "All +10%";

                int RowCount = 0;
                    int CellCount = 0;
                    Double RunningTotalForWeek = 0d;
                    string LabelToUpdate;
                    Double ThisDaysTotal = 0d;
                    for (RowCount = 0; RowCount < 4; RowCount++)
                    {
                        DateTime StartOfThisWeek = StartDateOfCurrentWeek.AddDays(0 - (7 * RowCount));

                        // do each day's figures(or total) in turn
                        RunningTotalForWeek = 0d;
                        CellCount = 0;
                        for (CellCount = 0; CellCount < 9; CellCount++)
                        {

                            LabelToUpdate = "Row" + RowCount.ToString() + "Cell" + CellCount.ToString() + "Label";

                            // for Mon-Sun, enter daily figures
                            if (CellCount < 7)
                            {

                                DateTime StartOfThisDay = StartOfThisWeek.AddDays(CellCount);
                                DateTime EndOfThisDay = new DateTime(year: StartOfThisDay.Year, month: StartOfThisDay.Month, day: StartOfThisDay.Day,
                                                                        hour: 23, minute: 59, second: 59, millisecond: 0);

                                ThisDaysTotal = 0d;
                                ThisDaysTotal = GetGBPValueOfOrders(StartOfThisDay, EndOfThisDay);
                                ViewData[LabelToUpdate] = "£" + ThisDaysTotal.ToString("N2");
                                RunningTotalForWeek += ThisDaysTotal;


                                // format top row (this week's figures) in red / green if higher / lower than last week
                                if (RowCount == 0)
                                {
                                    Double EquivDayLastWeekTotal = GetGBPValueOfOrders(StartOfThisDay.AddDays(-7), EndOfThisDay.AddDays(-7));


                                if (ThisDaysTotal == EquivDayLastWeekTotal)
                                {
                                    ViewData[LabelToUpdate] = "<font color='black'>" + ViewData[LabelToUpdate] + "</font>";
                                }
                                else if (ThisDaysTotal > EquivDayLastWeekTotal)
                                {
                                    ViewData[LabelToUpdate] = "<font color='green'>" + ViewData[LabelToUpdate] + "</font>";
                                }
                                else
                                {
                                    ViewData[LabelToUpdate] = "<font color='red'>" + ViewData[LabelToUpdate] + "</font>";
                                }
                            }
                            }


                            else
                            {
                                if (CellCount == 7)
                                {
                                    // for Total column, add total for week
                                    ViewData[LabelToUpdate] = "£" + RunningTotalForWeek.ToString("N2");

                                    // format top row (this week's total) in red / green if higher / lower than last week's total
                                    if (RowCount == 0)
                                    {
                                        Double EquivLastFullWeekTotal = GetGBPValueOfOrders(StartOfThisWeek.AddDays(-7), EndDateOfCurrentWeek.AddDays(-7));
                                        if (RunningTotalForWeek == EquivLastFullWeekTotal)
                                        {
                                            ViewData[LabelToUpdate] = "<font color='black'>" + ViewData[LabelToUpdate] + "</font>";
                                        }
                                        else if (RunningTotalForWeek > EquivLastFullWeekTotal)
                                        {
                                            ViewData[LabelToUpdate] = "<font color='green'>" + ViewData[LabelToUpdate] + "</font>";
                                        }
                                        else
                                        {
                                            ViewData[LabelToUpdate] = "<font color='red'>" + ViewData[LabelToUpdate] + "</font>";
                                        }
                                    }
                                }
                                else
                                {
                                    DateTime StartOfThisDay = StartOfThisWeek.AddYears(-1);
                                    DateTime EndOfThisWeek = StartOfThisDay.AddDays(6);
                                    DateTime EndOfThisDay = new DateTime(year: EndOfThisWeek.Year, month: EndOfThisWeek.Month, day: EndOfThisWeek.Day,
                                                                            hour: 23, minute: 59, second: 59, millisecond: 0);

                                    ThisDaysTotal = 0d;
                                    ThisDaysTotal = GetGBPValueOfOrders(StartOfThisDay, EndOfThisDay, true);
                                    ViewData[LabelToUpdate] = "£" + ThisDaysTotal.ToString("N2");
                                    String NextLabel = "Row" + RowCount.ToString() + "Cell" + (CellCount + 1).ToString() + "Label";
                                    Double NextDaysTotal = ThisDaysTotal * 1.1;
                                    ViewData[NextLabel] = "£" + NextDaysTotal.ToString("N2");


                                    // format top row (this week's figures) in red / green if higher / lower than last week
                                    //if (RowCount == 0)
                                    //{
                                    //    Double EquivDayLastWeekTotal = GetGBPValueOfOrders(StartOfThisDay.AddDays(-7), EndOfThisDay.AddDays(-7), true);
                                    //    Double NextEquivDayLastWeekTotal = EquivDayLastWeekTotal * 1.1;


                                    //    if (ThisDaysTotal == EquivDayLastWeekTotal)
                                    //    {
                                    //        ViewData[LabelToUpdate] = "<font color='black'>" + ViewData[LabelToUpdate] + "</font>";
                                    //    }
                                    //    else if (ThisDaysTotal > EquivDayLastWeekTotal)
                                    //    {
                                    //        ViewData[LabelToUpdate] = "<font color='green'>" + ViewData[LabelToUpdate] + "</font>";
                                    //    }
                                    //    else
                                    //    {
                                    //        ViewData[LabelToUpdate] = "<font color='red'>" + ViewData[LabelToUpdate] + "</font>";
                                    //    }

                                    //    if (NextDaysTotal == NextEquivDayLastWeekTotal)
                                    //    {
                                    //        ViewData[NextLabel] = "<font color='black'>" + ViewData[NextLabel] + "</font>";
                                    //    }
                                    //    else if (NextDaysTotal > NextEquivDayLastWeekTotal)
                                    //    {
                                    //        ViewData[NextLabel] = "<font color='green'>" + ViewData[NextLabel] + "</font>";
                                    //    }
                                    //    else
                                    //    {
                                    //        ViewData[NextLabel] = "<font color='red'>" + ViewData[NextLabel] + "</font>";
                                    //    }
                                    //}
                                }


                            }

                        }
                    }
                //}
                return View();
            }

            else
            {
                return Redirect("/Page/Locked");
            }
        }

        //        @using(Html.BeginForm())
        //        {
        //   < input type = "text" name = "reportName" />

        //      < input type = "submit" />
        //}
        //        and in your HttpPost action, use a parameter with same name as the textbox name.


        //        [HttpPost]
        //public ActionResult Report(string reportName)
        //        {
        //            //check for reportName parameter value now
        //            //to do : Return something
        //        }
       //[HttpPost]
       // public IActionResult Calculate(IFormCollection form)
       // {
       //     string type = form["TypeDropdownlist"].ToString();
       //     DateTime StartDateTime = DateTime.Parse(form["startdate"].ToString());
       //     DateTime EndDate = DateTime.Parse(form["enddate"].ToString());
       //     // string type = TypeDropdownlist.ToString();


       //     //DateTime StartDateTime = DateTime.Parse(ViewBag.StartDateSelected.ToString);
       //     //DateTime EndDate = DateTime.Parse(ViewBag.EndDateSelected.ToString);
       //     DateTime EndDateTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59, 999);            

       //     Double SalesFigure = GetGBPValueOfOrders(StartDateTime, EndDateTime, false, short.Parse(type));
       //     Double SalesFigureHistoricallyAdjusted = GetGBPValueOfOrders(StartDateTime, EndDateTime, true, short.Parse(type));

       //     SalesFigureMessage = "Value of orders gone ahead during the above period: <b>£" + SalesFigure.ToString("N2") + "</b>";
       //     SalesFigureHistoricallyAdjustedMessage = "Value of orders gone ahead during the above period (historically adjusted): <b>£" + SalesFigureHistoricallyAdjusted.ToString("N2") + "</b>";

       //     return RedirectToAction("SalesIntro", new { @salesfigures = SalesFigureMessage + "¦" + SalesFigureHistoricallyAdjustedMessage + "¦" + form["startdate"].ToString() + "¦" + form["enddate"].ToString() });
       // }

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
            if (Type == -1) {
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


        [Route("[controller]/[action]/{typeToShow}")]
        public async Task<IActionResult> SalesIntro(short typeToShow)
        {
            //var DataPassedOver = HttpContext.Request.Body;
            //var streamreader = new StreamReader(DataPassedOver);
            //var content = streamreader.ReadToEndAsync();
            //var Type = Convert.ToInt16(content.Result);

            TypeToUse = typeToShow;
            ViewBag.Type = typeToShow;
            //return Ok();
            //return RedirectToAction("SalesIntro");
            //var employeeLoggedIn = HttpContext.Session.Get("EmployeeLoggedIn");
            //string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split("\\")[1];
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            ////int EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUserID<int>(LogonUserName: username);
            Employee EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUser<Employee>(LogonUserName: username);
            // int rowno = 0;
            // int cellno = 0;
            // string labelname = "Row" + rowno + "Cell" + cellno + "Label";
            //ViewData[labelname] = "Row0Cell0Label Text";
            //Configuration.GetConnectionString("QAandCustomerWSTesting");
            // ViewBag.test = "<b>" + GetGBPValueOfOrders(new DateTime(2021, 08, 30), new DateTime(2021, 09, 06)) + "</b>";


            if (EmployeeCurrentlyLoggedIn.AccessLevel == 3 || EmployeeCurrentlyLoggedIn.AccessLevel == 4)
            //if (EmployeeCurrentlyLoggedIn == 41 || EmployeeCurrentlyLoggedIn == 22 || EmployeeCurrentlyLoggedIn == 1298 || EmployeeCurrentlyLoggedIn == 1305 || EmployeeCurrentlyLoggedIn == 475 || EmployeeCurrentlyLoggedIn == 185)

            {
                DateTime CurrentTimeAtTimeZone = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

                //if (salesfigures != "")
                //{
                //    ViewBag.SalesFigure = salesfigures.Split("¦")[0];
                //    ViewBag.SalesFigureHistoricallyAdjusted = salesfigures.Split("¦")[1];
                //    ViewBag.StartDateSelected = DateTime.Parse(salesfigures.Split("¦")[2]).ToString("yyyy-MM-dd");
                //    ViewBag.EndDateSelected = DateTime.Parse(salesfigures.Split("¦")[3]).ToString("yyyy-MM-dd");
                //}
                //else
                //{
                DateTime PrevMonthStart = CurrentTimeAtTimeZone.AddMonths(-1).AddDays(-(CurrentTimeAtTimeZone.Day - 1));
                DateTime PrevMonthEnd = CurrentTimeAtTimeZone.AddDays(-(CurrentTimeAtTimeZone.Day));

                ViewBag.StartDateSelected = PrevMonthStart.ToString("yyyy-MM-dd");
                ViewBag.EndDateSelected = PrevMonthEnd.ToString("yyyy-MM-dd");
                // }


                DateTime TodaysDate = CurrentTimeAtTimeZone;

                while (TodaysDate.DayOfWeek != DayOfWeek.Monday)
                {
                    TodaysDate = TodaysDate.AddDays(-1);
                }
                DateTime StartDateOfCurrentWeek = new DateTime(year: TodaysDate.Year, month: TodaysDate.Month, day: TodaysDate.Day,
                                     hour: 0, minute: 0, second: 0, millisecond: 0);

                TodaysDate = CurrentTimeAtTimeZone;

                while (TodaysDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    TodaysDate = TodaysDate.AddDays(1);
                }
                DateTime EndDateOfCurrentWeek = new DateTime(year: TodaysDate.Year, month: TodaysDate.Month, day: TodaysDate.Day,
                                     hour: 23, minute: 59, second: 59, millisecond: 999);


                ViewBag.ThisWeekDatesLabel = "(" + StartDateOfCurrentWeek.ToString("d MMM yy") +
                                        " &ndash; " + EndDateOfCurrentWeek.ToString("d MMM yy") + ")";

                ViewBag.LastWeekDatesLabel = "(" + StartDateOfCurrentWeek.AddDays(-7).ToString("d MMM yy") +
                                        " &ndash; " + EndDateOfCurrentWeek.AddDays(-7).ToString("d MMM yy") + ")";

                ViewBag.PreviousWeek1kDatesLabel = "(" + StartDateOfCurrentWeek.AddDays(-14).ToString("d MMM yy") +
                                            " &ndash; " + EndDateOfCurrentWeek.AddDays(-14).ToString("d MMM yy") + ")";

                ViewBag.PreviousWeek2kDatesLabel = "(" + StartDateOfCurrentWeek.AddDays(-21).ToString("d MMM yy") +
                                            " &ndash; " + EndDateOfCurrentWeek.AddDays(-21).ToString("d MMM yy") + ")";

                var target = 1.1;
                if (typeToShow == -1)
                {
                    ViewBag.EndColumnHeaderLabel = "All +10%";
                    target = 1.1;
                }
                if (typeToShow == 0)
                {
                    ViewBag.EndColumnHeaderLabel = "Internal +10%";
                    target = 1.1;
                }

                if (typeToShow == 1)
                {
                    ViewBag.EndColumnHeaderLabel = "External +10%";
                    target = 1.1;
                }

                int RowCount = 0;
                int CellCount = 0;
                Double RunningTotalForWeek = 0d;
                string LabelToUpdate;
                Double ThisDaysTotal = 0d;
                for (RowCount = 0; RowCount < 4; RowCount++)
                {
                    DateTime StartOfThisWeek = StartDateOfCurrentWeek.AddDays(0 - (7 * RowCount));

                    // do each day's figures(or total) in turn
                    RunningTotalForWeek = 0d;
                    CellCount = 0;
                    for (CellCount = 0; CellCount < 9; CellCount++)
                    {

                        LabelToUpdate = "Row" + RowCount.ToString() + "Cell" + CellCount.ToString() + "Label";

                        // for Mon-Sun, enter daily figures
                        if (CellCount < 7)
                        {

                            DateTime StartOfThisDay = StartOfThisWeek.AddDays(CellCount);
                            DateTime EndOfThisDay = new DateTime(year: StartOfThisDay.Year, month: StartOfThisDay.Month, day: StartOfThisDay.Day,
                                                                    hour: 23, minute: 59, second: 59, millisecond: 0);

                            ThisDaysTotal = 0d;
                            ThisDaysTotal = GetGBPValueOfOrders(StartOfThisDay, EndOfThisDay, Type: typeToShow);
                            ViewData[LabelToUpdate] = "£" + ThisDaysTotal.ToString("N2");
                            RunningTotalForWeek += ThisDaysTotal;


                            // format top row (this week's figures) in red / green if higher / lower than last week
                            if (RowCount == 0)
                            {
                                Double EquivDayLastWeekTotal = GetGBPValueOfOrders(StartOfThisDay.AddDays(-7), EndOfThisDay.AddDays(-7), Type: typeToShow);


                                if (ThisDaysTotal == EquivDayLastWeekTotal)
                                {
                                    ViewData[LabelToUpdate] = "<font color='black'>" + ViewData[LabelToUpdate] + "</font>";
                                }
                                else if (ThisDaysTotal > EquivDayLastWeekTotal)
                                {
                                    ViewData[LabelToUpdate] = "<font color='green'>" + ViewData[LabelToUpdate] + "</font>";
                                }
                                else
                                {
                                    ViewData[LabelToUpdate] = "<font color='red'>" + ViewData[LabelToUpdate] + "</font>";
                                }
                            }
                        }


                        else
                        {
                            if (CellCount == 7)
                            {
                                // for Total column, add total for week
                                ViewData[LabelToUpdate] = "£" + RunningTotalForWeek.ToString("N2");

                                // format top row (this week's total) in red / green if higher / lower than last week's total
                                if (RowCount == 0)
                                {
                                    Double EquivLastFullWeekTotal = GetGBPValueOfOrders(StartOfThisWeek.AddDays(-7), EndDateOfCurrentWeek.AddDays(-7), Type: typeToShow);
                                    if (RunningTotalForWeek == EquivLastFullWeekTotal)
                                    {
                                        ViewData[LabelToUpdate] = "<font color='black'>" + ViewData[LabelToUpdate] + "</font>";
                                    }
                                    else if (RunningTotalForWeek > EquivLastFullWeekTotal)
                                    {
                                        ViewData[LabelToUpdate] = "<font color='green'>" + ViewData[LabelToUpdate] + "</font>";
                                    }
                                    else
                                    {
                                        ViewData[LabelToUpdate] = "<font color='red'>" + ViewData[LabelToUpdate] + "</font>";
                                    }
                                }
                            }
                            else
                            {
                                DateTime StartOfThisDay = StartOfThisWeek.AddYears(-1);
                                DateTime EndOfThisWeek = StartOfThisDay.AddDays(6);
                                DateTime EndOfThisDay = new DateTime(year: EndOfThisWeek.Year, month: EndOfThisWeek.Month, day: EndOfThisWeek.Day,
                                                                        hour: 23, minute: 59, second: 59, millisecond: 0);

                                ThisDaysTotal = 0d;
                                ThisDaysTotal = GetGBPValueOfOrders(StartOfThisDay, EndOfThisDay, true, Type: typeToShow);
                                ViewData[LabelToUpdate] = "£" + ThisDaysTotal.ToString("N2");
                                String NextLabel = "Row" + RowCount.ToString() + "Cell" + (CellCount + 1).ToString() + "Label";
                                Double NextDaysTotal = ThisDaysTotal * target;
                                ViewData[NextLabel] = "£" + NextDaysTotal.ToString("N2");


                                // format top row (this week's figures) in red / green if higher / lower than last week
                                //if (RowCount == 0)
                                //{
                                //    Double EquivDayLastWeekTotal = GetGBPValueOfOrders(StartOfThisDay.AddDays(-7), EndOfThisDay.AddDays(-7), true);
                                //    Double NextEquivDayLastWeekTotal = EquivDayLastWeekTotal * 1.1;


                                //    if (ThisDaysTotal == EquivDayLastWeekTotal)
                                //    {
                                //        ViewData[LabelToUpdate] = "<font color='black'>" + ViewData[LabelToUpdate] + "</font>";
                                //    }
                                //    else if (ThisDaysTotal > EquivDayLastWeekTotal)
                                //    {
                                //        ViewData[LabelToUpdate] = "<font color='green'>" + ViewData[LabelToUpdate] + "</font>";
                                //    }
                                //    else
                                //    {
                                //        ViewData[LabelToUpdate] = "<font color='red'>" + ViewData[LabelToUpdate] + "</font>";
                                //    }

                                //    if (NextDaysTotal == NextEquivDayLastWeekTotal)
                                //    {
                                //        ViewData[NextLabel] = "<font color='black'>" + ViewData[NextLabel] + "</font>";
                                //    }
                                //    else if (NextDaysTotal > NextEquivDayLastWeekTotal)
                                //    {
                                //        ViewData[NextLabel] = "<font color='green'>" + ViewData[NextLabel] + "</font>";
                                //    }
                                //    else
                                //    {
                                //        ViewData[NextLabel] = "<font color='red'>" + ViewData[NextLabel] + "</font>";
                                //    }
                                //}
                            }


                        }

                    }
                }
                //}
                return View();
            }

            else
            {
                return Redirect("/Page/Locked");
            }
        }
    }
}



