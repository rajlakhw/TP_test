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
    public class HolidayService : IHolidayService
    {
        private readonly IRepository<EmployeeHoliday> employeeHolidayRepo;
        private readonly IRepository<EmployeeHolidayRequest> employeeHolidayRequestRepo;
        private readonly IRepository<BankHoliday> bankHolidayRepo;
        private readonly IRepository<Employee> employeeRepo;
        private readonly IRepository<EmployeeTeam> employeeTeamRepo;
        private readonly IRepository<HraccessMatrix> hrAcceessMatrixRepo;

        public HolidayService(IRepository<EmployeeHoliday> repository, IRepository<EmployeeHolidayRequest> repository1,
                              IRepository<Employee> repository2, IRepository<BankHoliday> repository3, IRepository<HraccessMatrix> repository4,
                              IRepository<EmployeeTeam> repository5)
        {
            this.employeeHolidayRepo = repository;
            this.employeeHolidayRequestRepo = repository1;
            this.employeeRepo = repository2;
            this.bankHolidayRepo = repository3;
            this.hrAcceessMatrixRepo = repository4;
            this.employeeTeamRepo = repository5;
        }

        public DateTime GetNextWorkingDayForEmployee(int employeeId)
        {
            DateTime DateToReturn = DateTime.Today;
            while (DateToReturn.DayOfWeek == DayOfWeek.Saturday || DateToReturn.DayOfWeek == DayOfWeek.Sunday ||
                   IsFullDayBankHolidayForEmployee(employeeId, DateToReturn).Result == true ||
                   IsDateAlreadyBookedForHoliday(employeeId, DateToReturn, 1).Result == true ||
                   IsDateAlreadyBookedForHoliday(employeeId, DateToReturn, 2).Result == true)
            {
                DateToReturn = DateToReturn.AddDays(1);
            }
            return DateToReturn;
        }

        public string GetNextWorkingDayAfterHolidayString(int employeeId, bool lastDayAmPmSelectorVisible, short StartDateAmorPmorFullDay,
                                                      short EndDateAmorPmorFullDay, DateTime StartDate, DateTime EndDate)
        {
            string dateStringToReturn = "";
            DateTime NextDay = DateTime.MinValue;
            if (lastDayAmPmSelectorVisible == true)
            {
                if (EndDateAmorPmorFullDay == 1)//if last day is "morning" off
                {
                    NextDay = EndDate;
                    if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, AMPMOrFullDay: 2).Result == true ||
                        IsHalfDayBankHolidayForEmployee(employeeId, NextDay).Result == true)
                    {
                        NextDay = EndDate.AddDays(1);
                        //if next day is a bank holiday, weekend or already booked holiday for the employee then 
                        //show next available date as preselected value
                        while (IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == true ||
                              NextDay.DayOfWeek == DayOfWeek.Saturday || NextDay.DayOfWeek == DayOfWeek.Sunday ||
                              IsDateAlreadyBookedForHoliday(employeeId, NextDay).Result == true)
                        {
                            if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 1).Result == false &&
                                NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                                IsHalfDayBankHolidayForEmployee(employeeId, NextDay).Result == false &&
                                IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                            {
                                dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy");
                                break;
                            }

                            if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 2).Result == false &&
                                NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                                IsHalfDayBankHolidayForEmployee(employeeId, NextDay).Result == false &&
                                IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                            {
                                dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy") + " (Afternoon)";
                                break;
                            }
                            NextDay = NextDay.AddDays(1);
                        }
                    }
                    else
                    {
                        dateStringToReturn = EndDate.ToString("dddd d MMMM yyyy") + " (Afternoon)";
                    }
                }
                else
                {
                    NextDay = EndDate.AddDays(1);
                    //if next day is a bank holiday, weekend or already booked holiday for the employee then 
                    //show next available date
                    while (IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == true ||
                        NextDay.DayOfWeek == DayOfWeek.Saturday || NextDay.DayOfWeek == DayOfWeek.Sunday ||
                        IsDateAlreadyBookedForHoliday(employeeId, NextDay).Result == true)
                    {
                        if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 1).Result == false &&
                            NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                            IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                        {
                            dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy");
                            break;
                        }

                        if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 2).Result == false &&
                            NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                            IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false &&
                            IsHalfDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                        {
                            dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy") + " (Afternoon)";
                            break;
                        }
                        NextDay = NextDay.AddDays(1);
                    }
                }
            }
            else
            {
                if (StartDateAmorPmorFullDay == 1)
                {
                    NextDay = StartDate;
                    if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 2).Result == true ||
                        IsHalfDayBankHolidayForEmployee(employeeId, NextDay).Result == true)
                    {
                        NextDay = StartDate.AddDays(1);
                        //if next day is a bank holiday, weekend or already booked holiday for the employee then 
                        //show next available date as preselected value
                        while (IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == true ||
                            NextDay.DayOfWeek == DayOfWeek.Saturday || NextDay.DayOfWeek == DayOfWeek.Sunday ||
                            IsDateAlreadyBookedForHoliday(employeeId, NextDay).Result == true)
                        {
                            if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 1).Result == false &&
                                NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                                IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                            {
                                dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy");
                                break;
                            }

                            if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 2).Result == false &&
                                NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                                IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false &&
                                IsHalfDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                            {
                                dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy") + " (Afternoon)";
                                break;
                            }
                            NextDay = NextDay.AddDays(1);
                        }
                    }
                    else
                    {
                        dateStringToReturn = StartDate.ToString("dddd d MMMM yyyy") + " (Afternoon)";
                    }
                }
                else
                {
                    NextDay = StartDate.AddDays(1);
                    //if next day is a bank holiday, weekend or already booked holiday for the employee then 
                    //show next available date as preselected value
                    while (IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == true ||
                        NextDay.DayOfWeek == DayOfWeek.Saturday || NextDay.DayOfWeek == DayOfWeek.Sunday ||
                        IsDateAlreadyBookedForHoliday(employeeId, NextDay).Result == true)
                    {
                        if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 1).Result == false &&
                            NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                            IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                        {
                            dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy");
                            break;
                        }

                        if (IsDateAlreadyBookedForHoliday(employeeId, NextDay, 2).Result == false &&
                            NextDay.DayOfWeek != DayOfWeek.Saturday && NextDay.DayOfWeek != DayOfWeek.Sunday &&
                            IsFullDayBankHolidayForEmployee(employeeId, NextDay).Result == false &&
                            IsHalfDayBankHolidayForEmployee(employeeId, NextDay).Result == false)
                        {
                            dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy") + " (Afternoon)";
                            break;
                        }
                        NextDay = NextDay.AddDays(1);
                    }
                }
            }
            if (dateStringToReturn == "")
            {
                dateStringToReturn = NextDay.ToString("dddd d MMMM yyyy");
            }

            return dateStringToReturn;
        }
        public async Task<Boolean> IsFullDayBankHolidayForEmployee(int employeeId, DateTime DateToCheck)
        {
            byte? officeId = employeeRepo.All().Where(e => e.Id == employeeId).Select(e => e.OfficeId).FirstOrDefault();

            List<BankHoliday> result = new List<BankHoliday>();
            if (officeId == 4)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsAukbankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck).ToListAsync();
            }
            else if (officeId == 9)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsJapanBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck).ToListAsync();
            }
            else if (officeId == 11)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsBulgarianBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck).ToListAsync();
            }
            else if (officeId == 13)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsRomanianBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck).ToListAsync();
            }
            else if (officeId == 15)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsCostaRicaBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck).ToListAsync();
            }
            if (result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<Boolean> IsHalfDayBankHolidayForEmployee(int employeeId, DateTime DateToCheck)
        {
            byte? officeId = employeeRepo.All().Where(e => e.Id == employeeId).Select(e => e.OfficeId).FirstOrDefault();

            List<BankHoliday> result = new List<BankHoliday>();
            if (officeId == 4)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsAukbankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck && b.IsHalfDay == true).ToListAsync();
            }
            else if (officeId == 9)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsJapanBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck && b.IsHalfDay == true).ToListAsync();
            }
            else if (officeId == 11)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsBulgarianBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck && b.IsHalfDay == true).ToListAsync();
            }
            else if (officeId == 13)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsRomanianBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck && b.IsHalfDay == true).ToListAsync();
            }
            else if (officeId == 15)
            {
                result = await bankHolidayRepo.All().Where(b => b.IsCostaRicaBankHoliday == true && b.DeletedDateTime == null && b.BankHolidayDate == DateToCheck && b.IsHalfDay == true).ToListAsync();
            }

            if (result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<Boolean> IsDateAlreadyBookedForHoliday(int employeeId, DateTime DateToCheck, short? AMPMOrFullDay = 0)
        {
            Data.EmployeeHolidayRequest result = null;
            //Only return holidays that are not declined or deleted
            if (AMPMOrFullDay == 0) //Full day
            {
                result = await employeeHolidayRequestRepo.All().
                             Where(h => h.EmployeeId == employeeId && DateToCheck >= h.HolidayStartDateTime && DateToCheck <= h.HolidayEndDateTime
                                   && h.DeletedDateTime == null && h.Status != 2).FirstOrDefaultAsync();
            }
            else if (AMPMOrFullDay == 1) //AM
            {
                result = await employeeHolidayRequestRepo.All().
                         Where(h => h.EmployeeId == employeeId && DateToCheck >= h.HolidayStartDateTime && DateToCheck <= h.HolidayEndDateTime
                               && h.DeletedDateTime == null && h.Status != 2)
                         .Where(a => (a.HolidayStartDateTime == DateToCheck && (a.StartDateAmorPmorFullDay == 0 || a.StartDateAmorPmorFullDay == 1)) ||
                                     (a.HolidayEndDateTime == DateToCheck && (a.EndDateAmorPmorFullDay == 0 || a.EndDateAmorPmorFullDay == 1)))
                         .FirstOrDefaultAsync();
            }
            else if (AMPMOrFullDay == 2) //PM
            {
                result = await employeeHolidayRequestRepo.All()
                         .Where(h => h.EmployeeId == employeeId && DateToCheck >= h.HolidayStartDateTime && DateToCheck <= h.HolidayEndDateTime
                         && h.DeletedDateTime == null && h.Status != 2)
                         .Where(a => (a.HolidayStartDateTime == DateToCheck && (a.StartDateAmorPmorFullDay == 0 || a.StartDateAmorPmorFullDay == 2)) ||
                                     (a.HolidayEndDateTime == DateToCheck && (a.EndDateAmorPmorFullDay == 0 || a.EndDateAmorPmorFullDay == 2)))
                         .FirstOrDefaultAsync();

            }


            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<EmployeeHoliday> GetEmployeeHolidayDetailsForYear(int employeeId, int year)
        {
            var employeeHoliday = await employeeHolidayRepo.All().Where(e => e.EmployeeId == employeeId && e.Year == year).FirstOrDefaultAsync();
            return employeeHoliday;
        }

        public double GetTotalNumberOfDaysForHolidayRequest(int employeeId, DateTime StartDate, DateTime EndDate, short StartDateAmorPmorFullDay,
                                                          short EndDateAmorPmorFullDay, bool lastDayAmPmSelectorVisible)
        {

            if (lastDayAmPmSelectorVisible == false)
            {
                EndDate = StartDate;
            }

            bool isFirstDayWeekendOrBankHoliday = false;
            if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday ||
                IsFullDayBankHolidayForEmployee(employeeId, StartDate).Result == true)
            {
                isFirstDayWeekendOrBankHoliday = true;
            }


            bool isLastDayWeekendOrBankHoliday = false;
            double holidayDays = (EndDate - StartDate).TotalDays + 1;
            DateTime FirstDayForLoop = StartDate;
            while (EndDate >= FirstDayForLoop)
            {
                if (FirstDayForLoop.DayOfWeek == DayOfWeek.Saturday || FirstDayForLoop.DayOfWeek == DayOfWeek.Sunday ||
                    IsFullDayBankHolidayForEmployee(employeeId, FirstDayForLoop).Result == true)
                {
                    holidayDays -= 1;
                }

                if (IsHalfDayBankHolidayForEmployee(employeeId, FirstDayForLoop).Result == true)
                {
                    holidayDays -= 0.5;
                }
                FirstDayForLoop = FirstDayForLoop.AddDays(1);
            }

            //if the last day is select to be a weekend or bank holiday, then subtract the holiday days by 1, since
            //the do until loop does not run when first day = last day
            if (EndDate.DayOfWeek == DayOfWeek.Saturday || EndDate.DayOfWeek == DayOfWeek.Sunday ||
                IsFullDayBankHolidayForEmployee(employeeId, EndDate).Result == true)
            {
                holidayDays -= 1;
                isLastDayWeekendOrBankHoliday = true;
            }

            if (StartDate == EndDate)
            {
                if (StartDateAmorPmorFullDay == 1 || StartDateAmorPmorFullDay == 2)
                {
                    holidayDays -= 0.5;
                }
            }
            else
            {
                if (StartDateAmorPmorFullDay == 2 && isFirstDayWeekendOrBankHoliday == false)
                {
                    holidayDays -= 0.5;
                }

                if (EndDateAmorPmorFullDay == 1 && isLastDayWeekendOrBankHoliday == false)
                {
                    holidayDays -= 0.5;
                }
            }

            return holidayDays;
        }



        public async Task<EmployeeHolidayRequest> AddHolidayRequest(short employeeId, DateTime holidayStartDateTime, byte startDateAMOrPMOrFullDay,
                                                                    DateTime holidayEndDateTime, byte endDateAMOrPMOrFullDay, decimal totalDays,
                                                                    short requestedByEmployeeId, double holidaysInStartDateYear, double holidaysInEndDateYear)
        {
            var thisHolidayRequest = new Data.EmployeeHolidayRequest()
            {
                EmployeeId = employeeId,
                HolidayStartDateTime = holidayStartDateTime,
                StartDateAmorPmorFullDay = startDateAMOrPMOrFullDay,
                HolidayEndDateTime = holidayEndDateTime,
                EndDateAmorPmorFullDay = endDateAMOrPMOrFullDay,
                TotalDays = totalDays,
                RequestedDateTime = GeneralUtils.GetCurrentUKTime(),
                RequestedByEmployeeId = requestedByEmployeeId,
                HolidaysInStartDateYear = decimal.Parse(holidaysInStartDateYear.ToString()),
                HolidaysInEndDateYear = decimal.Parse(holidaysInEndDateYear.ToString())
            };

            await employeeHolidayRequestRepo.AddAsync(thisHolidayRequest);
            await employeeHolidayRequestRepo.SaveChangesAsync();

            return thisHolidayRequest;
        }


        public async Task<List<EmployeeHolidayRequest>> GetAllHolidayRequestsForEmployeeForYear(short employeeId, int year)
        {
            var result = await employeeHolidayRequestRepo.All().Where(hr => hr.EmployeeId == employeeId &&
                                                                (hr.HolidayStartDateTime.Year == year || hr.HolidayEndDateTime.Year == year) &&
                                                                hr.DeletedDateTime == null).OrderBy(o => o.HolidayStartDateTime).ToListAsync();
            return result;
        }

        public async Task<List<EmployeeHoliday>> GetAllHolidayDetailsForEmployee(short employeeId)
        {
            var result = await employeeHolidayRepo.All().Where(h => h.EmployeeId == employeeId).ToListAsync();
            return result;
        }

        public async Task<EmployeeHoliday> GetHolidayDetailsForYear(short employeeId, int year)
        {
            var result = await employeeHolidayRepo.All().Where(h => h.EmployeeId == employeeId && h.Year == year).FirstOrDefaultAsync();
            return result;
        }


        public async Task<EmployeeHolidayRequest> GetHolidayRequestDetails(int holidayRequestID)
        {
            var result = await employeeHolidayRequestRepo.All().Where(hr => hr.Id == holidayRequestID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<EmployeeHolidayRequest> DeleteHolidayRequest(int holidayRequestID, short deletedByEmployeeId)
        {
            var thisHolidayRequest = await GetHolidayRequestDetails(holidayRequestID);

            thisHolidayRequest.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
            thisHolidayRequest.DeletedByEmployeeId = deletedByEmployeeId;

            employeeHolidayRequestRepo.Update(thisHolidayRequest);
            await employeeHolidayRequestRepo.SaveChangesAsync();

            return thisHolidayRequest;

        }

        public async Task<List<EmployeeHolidayRequest>> GetAllTeamHolidaysInSameDateRange(int holidayRequestID)
        {
            DateTime startDate = await employeeHolidayRequestRepo.All().Where(hr => hr.Id == holidayRequestID).Select(hr => hr.HolidayStartDateTime).FirstOrDefaultAsync();

            DateTime endDate = await employeeHolidayRequestRepo.All().Where(hr => hr.Id == holidayRequestID).Select(hr => hr.HolidayEndDateTime).FirstOrDefaultAsync();

            int TeamID = await employeeHolidayRequestRepo.All()
                               .Where(hr => hr.Id == holidayRequestID)
                               .Join(employeeRepo.All(), //second table to inner join with
                                hr => hr.EmployeeId,
                                e => e.Id,
                                (hr, e) => new { HolidayReq = hr, Emp = e })
                               .Select(o => o.Emp.TeamId).FirstOrDefaultAsync();

            var results = await employeeHolidayRequestRepo.All()
                                  .Where(hr => hr.DeletedDateTime == null &&
                                  (hr.HolidayStartDateTime >= startDate && hr.HolidayStartDateTime <= endDate ||
                                  hr.HolidayEndDateTime >= startDate && hr.HolidayEndDateTime <= endDate ||
                                  startDate >= hr.HolidayStartDateTime && startDate <= hr.HolidayEndDateTime ||
                                  endDate >= hr.HolidayStartDateTime && endDate <= hr.HolidayEndDateTime) &&
                                  hr.Id != holidayRequestID // Ignore declined requests
                                  && hr.Status != 2)
                                .Join(employeeRepo.All(),
                                      hr => hr.EmployeeId,
                                      e => e.Id,
                                      (hr, e) => new { HolidayReq = hr, Emp = e })
                                .Where(e => e.Emp.TeamId == TeamID && e.Emp.TerminateDate == null)
                                .Select(e => e.HolidayReq).OrderByDescending(o => o.HolidayStartDateTime).ToListAsync();

            return results;
        }


        public async Task<EmployeeHolidayRequest> ApproveHolidayRequest(int holidayRequestID, short approvedByEmployeeId)
        {
            var thisHolidayRequest = await GetHolidayRequestDetails(holidayRequestID);

            thisHolidayRequest.ApprovedDateTime = GeneralUtils.GetCurrentUKTime();
            thisHolidayRequest.ApprovedByEmployeeId = approvedByEmployeeId;

            employeeHolidayRequestRepo.Update(thisHolidayRequest);
            await employeeHolidayRequestRepo.SaveChangesAsync();

            return thisHolidayRequest;
        }

        public async Task<EmployeeHolidayRequest> DeclineHolidayRequest(int holidayRequestID, short declinedByEmployeeId)
        {
            var thisHolidayRequest = await GetHolidayRequestDetails(holidayRequestID);

            thisHolidayRequest.RejectedDateTime = GeneralUtils.GetCurrentUKTime();
            thisHolidayRequest.RejectedByEmployeeId = declinedByEmployeeId;

            employeeHolidayRequestRepo.Update(thisHolidayRequest);
            await employeeHolidayRequestRepo.SaveChangesAsync();

            return thisHolidayRequest;
        }

        public async Task<bool> CheckIfEmployeeHasAccessToApproveHolidays(short employeeId, short employeeToBeApproved)
        {
            var employeeHasAccess = false;

            int employeeTeamId = await employeeRepo.All().Where(e => e.Id == employeeId).Select(e => e.TeamId).FirstOrDefaultAsync();

            short? employeeAccessLevelId = await employeeRepo.All().Where(e => e.Id == employeeId).Select(e => e.AccessLevel).FirstOrDefaultAsync();

            int employeeDepartmentId = await employeeTeamRepo.All().Where(e => e.Id == employeeTeamId).Select(e => e.DepartmentId).FirstOrDefaultAsync();

            int employeeToBeApprovedTeamId = await employeeRepo.All().Where(e => e.Id == employeeToBeApproved).Select(e => e.TeamId).FirstOrDefaultAsync();

            int employeeToBeApprovedDepartmentId = await employeeTeamRepo.All().Where(e => e.Id == employeeToBeApprovedTeamId).Select(e => e.DepartmentId).FirstOrDefaultAsync();

            short? employeeToBeApprovedAccessLevelId = await employeeRepo.All().Where(e => e.Id == employeeToBeApproved).Select(e => e.AccessLevel).FirstOrDefaultAsync();

            var results = await hrAcceessMatrixRepo.All().Where(hr => hr.EmployeeAccessLevelId == employeeAccessLevelId &&
                                                                ((hr.EmployeeFromDataObjectTypeId == 21 && hr.EmployeeFromDataObjectId == employeeTeamId) ||
                                                                 (hr.EmployeeFromDataObjectTypeId == 22 && hr.EmployeeFromDataObjectId == employeeDepartmentId) ||
                                                                 (hr.EmployeeFromDataObjectTypeId == 5 && hr.EmployeeFromDataObjectId == employeeDepartmentId) ||
                                                                 (hr.EmployeeFromDataObjectTypeId == null && hr.EmployeeFromDataObjectId == null)) &&
                                                                ((hr.AccessToApproveDataObjectTypeId == 21 && hr.AccessToApproveDataObjectId == employeeToBeApprovedTeamId) ||
                                                                 (hr.AccessToApproveDataObjectTypeId == 22 && hr.AccessToApproveDataObjectId == employeeToBeApprovedDepartmentId) ||
                                                                 (hr.AccessToApproveDataObjectTypeId == 5 && hr.AccessToApproveDataObjectId == employeeToBeApproved) ||
                                                                 (hr.AccessToApproveDataObjectTypeId == null && hr.AccessToApproveDataObjectId == null)) &&
                                                                ((hr.AccessToApproveAccessLevelId == null) ||
                                                                 (hr.AccessToApproveAccessLevelId == employeeToBeApprovedAccessLevelId)) &&
                                                                 hr.EnableAccessToHolidays == true).ToListAsync();

            if (results.Count > 0)
            {
                employeeHasAccess = true;
            }
            return employeeHasAccess;
        }

        public async Task<bool> CheckIfEmployeeHasAccessToAccessSickness(short employeeId, short employeeToBeApproved)
        {
            var employeeHasAccess = false;

            int employeeTeamId = await employeeRepo.All().Where(e => e.Id == employeeId).Select(e => e.TeamId).FirstOrDefaultAsync();

            short? employeeAccessLevelId = await employeeRepo.All().Where(e => e.Id == employeeId).Select(e => e.AccessLevel).FirstOrDefaultAsync();

            int employeeDepartmentId = await employeeTeamRepo.All().Where(e => e.Id == employeeTeamId).Select(e => e.DepartmentId).FirstOrDefaultAsync();

            int employeeToBeApprovedTeamId = await employeeRepo.All().Where(e => e.Id == employeeToBeApproved).Select(e => e.TeamId).FirstOrDefaultAsync();

            int employeeToBeApprovedDepartmentId = await employeeTeamRepo.All().Where(e => e.Id == employeeToBeApprovedTeamId).Select(e => e.DepartmentId).FirstOrDefaultAsync();

            short? employeeToBeApprovedAccessLevelId = await employeeRepo.All().Where(e => e.Id == employeeToBeApproved).Select(e => e.AccessLevel).FirstOrDefaultAsync();

            var results = await hrAcceessMatrixRepo.All().Where(hr => hr.EmployeeAccessLevelId == employeeAccessLevelId &&
                                                                ((hr.EmployeeFromDataObjectTypeId == 21 && hr.EmployeeFromDataObjectId == employeeTeamId) ||
                                                                 (hr.EmployeeFromDataObjectTypeId == 22 && hr.EmployeeFromDataObjectId == employeeDepartmentId) ||
                                                                 (hr.EmployeeFromDataObjectTypeId == 5 && hr.EmployeeFromDataObjectId == employeeDepartmentId) ||
                                                                 (hr.EmployeeFromDataObjectTypeId == null && hr.EmployeeFromDataObjectId == null)) &&
                                                                ((hr.AccessToApproveDataObjectTypeId == 21 && hr.AccessToApproveDataObjectId == employeeToBeApprovedTeamId) ||
                                                                 (hr.AccessToApproveDataObjectTypeId == 22 && hr.AccessToApproveDataObjectId == employeeToBeApprovedDepartmentId) ||
                                                                 (hr.AccessToApproveDataObjectTypeId == 5 && hr.AccessToApproveDataObjectId == employeeToBeApproved) ||
                                                                 (hr.AccessToApproveDataObjectTypeId == null && hr.AccessToApproveDataObjectId == null)) &&
                                                                ((hr.AccessToApproveAccessLevelId == null) ||
                                                                 (hr.AccessToApproveAccessLevelId == employeeToBeApprovedAccessLevelId)) &&
                                                                 hr.EnableAccessToSickness == true).ToListAsync();

            if (results.Count > 0)
            {
                employeeHasAccess = true;
            }
            return employeeHasAccess;
        }
    }
}
