using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Repositories;
using Microsoft.Data.SqlClient;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;

namespace Services
{
    public class TPQuotesLogic : ITPQuotesLogic
    {
        private readonly IRepository<Quote> quoteRepository;
        private readonly IRepository<QuoteItem> quoteItemRepository;
        private readonly ITPTimeZonesService timeZonesService;
        private readonly GlobalVariables globalVariables;
        private readonly IConfiguration configuration;
        private readonly IRepository<Enquiry> enquiriesRepo;

        public TPQuotesLogic(IRepository<Quote> repository, IRepository<QuoteItem> tpQuoteItemRepo,
                            ITPTimeZonesService tPTimeZonesService, IConfiguration configuration, IRepository<Enquiry> enquiriesRepo)
        {
            this.quoteRepository = repository;
            this.quoteItemRepository = tpQuoteItemRepo;
            this.timeZonesService = tPTimeZonesService;
            globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
            this.enquiriesRepo = enquiriesRepo;
        }

        public async Task<Quote> CreateQuote(int EnquiryID, bool IsCurrentVersion, short QuoteCurrencyID, string LangIANACode, string Title,
                                string QuoteFileName, string InternalNotes, DateTime QuoteDate, string QuoteOrgName,
                                string QuoteAddress1, string QuoteAddress2, string QuoteAddress3, string QuoteAddress4,
                                string QuoteCountyOrState, string QuotePostcodeOrZip, short QuoteCountryID, string AddresseeSalutationName,
                                string OpeningSectionText, string ClosingSectionText, byte TimelineUnit, double TimelineValue,
                                byte WordCountPresentationOption, bool ShowInterpretingDurationInBreakdown, bool ShowWorkDurationInBreakdown,
                                bool ShowPagesOrSlidesInBreakdown, bool ShowNumberOfCharactersInBreakdown, bool ShowNumberOfDocumentsInBreakdown,
                                short SalesContactEmployeeID, string CustomerSpecificField1Value, string CustomerSpecificField2Value,
                                string CustomerSpecificField3Value, string CustomerSpecificField4Value, bool ShowCustomerSpecificField1Value,
                                bool ShowCustomerSpecificField2Value, bool ShowCustomerSpecificField3Value, bool ShowCustomerSpecificField4Value,
                                string ClientPONumber, short CreatedByEmployeeID, DateTime CreatedDateTime, bool PrintingProject = false)
        {
            //bool enqExists = await enquiriesService.EnquiryExists(EnquiryID);
            //if (enqExists == false)
            //{
            //	throw new Exception("The enquiry ID, " + EnquiryID.ToString() + ", does not exist in the database. A quote must be created against a valid enquiry.");
            //}
            //         else
            //         {
            int newQuoteID;
            Quote newQuote;

            // trim strings as a precaution

            LangIANACode = LangIANACode.Trim();
            if (LangIANACode.Length > 20)
            {
                LangIANACode = LangIANACode.Substring(0, 20);

            }

            if (Title != null)
            {
                Title = Title.Trim();
                if (Title.Length > 500)
                {
                    Title = Title.Substring(0, 500);
                }
            }

            if (QuoteOrgName != null)
            {
                QuoteOrgName = QuoteOrgName.Trim();
                if (QuoteOrgName.Length > 200)
                {
                    QuoteOrgName = QuoteOrgName.Substring(0, 200);
                }
            }

            if (QuoteAddress1 != null)
            {
                QuoteAddress1 = QuoteAddress1.Trim();
                if (QuoteAddress1.Length > 100)
                {
                    QuoteAddress1 = QuoteAddress1.Substring(0, 100);
                }
            }

            QuoteAddress2 = QuoteAddress2.Trim();
            if (QuoteAddress2.Length > 100)
            {
                QuoteAddress2 = QuoteAddress2.Substring(0, 100);
            }

            if (QuoteAddress3 != null)
            {
                QuoteAddress3 = QuoteAddress3.Trim();
                if (QuoteAddress3.Length > 100)
                {
                    QuoteAddress3 = QuoteAddress3.Substring(0, 100);
                }

            }

            if (QuoteAddress4 != null)
            {
                QuoteAddress4 = QuoteAddress4.Trim();
                if (QuoteAddress4.Length > 100)
                {
                    QuoteAddress4 = QuoteAddress4.Substring(0, 100);
                }
            }

            if (QuoteCountyOrState != null)
            {
                QuoteCountyOrState = QuoteCountyOrState.Trim();
                if (QuoteCountyOrState.Length > 100)
                {
                    QuoteCountyOrState = QuoteCountyOrState.Substring(0, 100);
                }
            }


            QuotePostcodeOrZip = QuotePostcodeOrZip.Trim();
            if (QuotePostcodeOrZip.Length > 20)
            {
                QuotePostcodeOrZip = QuotePostcodeOrZip.Substring(0, 20);
            }

            AddresseeSalutationName = AddresseeSalutationName.Trim();
            if (AddresseeSalutationName.Length > 100)
            {
                AddresseeSalutationName = AddresseeSalutationName.Substring(0, 100);
            }

            OpeningSectionText = OpeningSectionText.Trim();
            ClosingSectionText = ClosingSectionText.Trim();
            if (InternalNotes != null)
            {
                InternalNotes = InternalNotes.Trim();
            }

            if (ClientPONumber != null)
            {
                ClientPONumber = ClientPONumber.Trim();

                if (ClientPONumber.Length > 100)
                {
                    ClientPONumber = ClientPONumber.Substring(0, 100);
                }
            }

            if (CustomerSpecificField1Value != null)
            {
                CustomerSpecificField1Value = CustomerSpecificField1Value.Trim();
            }

            if (CustomerSpecificField2Value != null)
            {
                CustomerSpecificField2Value = CustomerSpecificField2Value.Trim();
            }

            if (CustomerSpecificField3Value != null)
            {
                CustomerSpecificField3Value = CustomerSpecificField3Value.Trim();
            }

            if (CustomerSpecificField4Value != null)
            {
                CustomerSpecificField4Value = CustomerSpecificField4Value.Trim();
            }


            if (QuoteDate == null)
            {
                QuoteDate = DateTime.MinValue;
            }


            newQuote = new Quote()
            {
                EnquiryId = EnquiryID,
                IsCurrentVersion = IsCurrentVersion,
                QuoteCurrencyId = QuoteCurrencyID,
                LangIanacode = LangIANACode,
                Title = Title,
                QuoteFileName = GeneralUtils.MakeStringSafeForFileSystemPath(QuoteFileName),
                InternalNotes = InternalNotes,
                QuoteDate = QuoteDate,
                QuoteOrgName = QuoteOrgName,
                QuoteAddress1 = QuoteAddress1,
                QuoteAddress2 = QuoteAddress2,
                QuoteAddress3 = QuoteAddress3,
                QuoteAddress4 = QuoteAddress4,
                QuoteCountyOrState = QuoteCountyOrState,
                QuotePostcodeOrZip = QuotePostcodeOrZip,
                QuoteCountryId = QuoteCountryID,
                AddresseeSalutationName = AddresseeSalutationName,
                OpeningSectionText = OpeningSectionText,
                ClosingSectionText = ClosingSectionText,
                TimelineUnit = TimelineUnit,
                TimelineValue = TimelineValue,
                WordCountPresentationOption = WordCountPresentationOption,
                ShowInterpretingDurationInBreakdown = ShowInterpretingDurationInBreakdown,
                ShowWorkDurationInBreakdown = ShowWorkDurationInBreakdown,
                ShowPagesOrSlidesInBreakdown = ShowPagesOrSlidesInBreakdown,
                ShowNumberOfCharactersInBreakdown = ShowNumberOfCharactersInBreakdown,
                ShowNumberOfDocumentsInBreakdown = ShowNumberOfDocumentsInBreakdown,
                SalesContactEmployeeId = SalesContactEmployeeID,
                CustomerSpecificField1Value = CustomerSpecificField1Value,
                CustomerSpecificField2Value = CustomerSpecificField2Value,
                CustomerSpecificField3Value = CustomerSpecificField3Value,
                CustomerSpecificField4Value = CustomerSpecificField4Value,
                ShowCustomerSpecificField1Value = ShowCustomerSpecificField1Value,
                ShowCustomerSpecificField2Value = ShowCustomerSpecificField2Value,
                ShowCustomerSpecificField3Value = ShowCustomerSpecificField3Value,
                ShowCustomerSpecificField4Value = ShowCustomerSpecificField4Value,
                ClientPonumber = ClientPONumber,
                PrintingProject = PrintingProject,
                CreatedDateTime = timeZonesService.GetCurrentGMT(),
                CreatedByEmployeeId = CreatedByEmployeeID

            };

            await quoteRepository.AddAsync(newQuote);
            await quoteRepository.SaveChangesAsync();

            return newQuote;
            //}

        }


        public async Task<bool> QuoteExists(int QuoteId)
        {
            var result = await quoteRepository.All().Where(q => q.Id == QuoteId).FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<QuoteItem> CreateQuoteItem(int QuoteID, byte LanguageServiceID, string SourceLanguageIANAcode, string TargetLanguageIANAcode,
                                        int WordCountNew, int WordCountFuzzyBand1, int WordCountFuzzyBand2, int WordCountFuzzyBand3, int WordCountFuzzyBand4,
                                        int WordCountExact, int WordCountRepetitions, int WordCountPerfectMatches, int Pages, int Characters,
                                        int Documents, int InterpretingExpectedDurationMinutes, string InterpretingLocationOrgName,
                                        string InterpretingLocationAddress1, string InterpretingLocationAddress2,
                                        string InterpretingLocationAddress3, string InterpretingLocationAddress4,
                                        string InterpretingLocationCountyOrState, string InterpretingLocationPostcodeOrZip,
                                        short? InterpretingLocationCountryID, int AudioMinutes, int WorkMinutes, string ExternalNotes,
                                        decimal? ChargeToClient, short CreatedByEmployeeID, int SupplierWordCountNew, int SupplierWordCountFuzzyBand1,
                                        int SupplierWordCountFuzzyBand2, int SupplierWordCountFuzzyBand3, int SupplierWordCountFuzzyBand4,
                                        int SupplierWordCountExact, int SupplierWordCountRepetitions, int SupplierWordCountPerfectMatches,
                                        int WordCountClientSpecific = 0, int SupplierWordCountClientSpecific = 0, int languageServiceCategoryId = 0)
        {
            if (await QuoteExists(QuoteID) == false)
            {
                throw new Exception("The quote ID, " + QuoteID.ToString() + ", does not exist in the database. A quote item must be created against a valid quote.");
            }
            else
            {
                QuoteItem NewQuoteItem;

                //trim strings as a precaution
                SourceLanguageIANAcode = SourceLanguageIANAcode.Trim();
                if (SourceLanguageIANAcode.Length > 20)
                {
                    SourceLanguageIANAcode = SourceLanguageIANAcode.Substring(0, 20);
                }

                TargetLanguageIANAcode = TargetLanguageIANAcode.Trim();
                if (TargetLanguageIANAcode.Length > 20)
                {
                    TargetLanguageIANAcode = TargetLanguageIANAcode.Substring(0, 20);
                }

                InterpretingLocationOrgName = InterpretingLocationOrgName.Trim();
                if (InterpretingLocationOrgName.Length > 200)
                {
                    InterpretingLocationOrgName = InterpretingLocationOrgName.Substring(0, 200);
                }

                InterpretingLocationAddress1 = InterpretingLocationAddress1.Trim();
                if (InterpretingLocationAddress1.Length > 100)
                {
                    InterpretingLocationAddress1 = InterpretingLocationAddress1.Substring(0, 100);
                }

                InterpretingLocationAddress2 = InterpretingLocationAddress2.Trim();
                if (InterpretingLocationAddress2.Length > 100)
                {
                    InterpretingLocationAddress2 = InterpretingLocationAddress2.Substring(0, 100);
                }

                InterpretingLocationAddress3 = InterpretingLocationAddress3.Trim();
                if (InterpretingLocationAddress3.Length > 100)
                {
                    InterpretingLocationAddress3 = InterpretingLocationAddress3.Substring(0, 100);
                }

                InterpretingLocationAddress4 = InterpretingLocationAddress4.Trim();
                if (InterpretingLocationAddress4.Length > 100)
                {
                    InterpretingLocationAddress4 = InterpretingLocationAddress4.Substring(0, 100);
                }

                InterpretingLocationCountyOrState = InterpretingLocationCountyOrState.Trim();
                if (InterpretingLocationCountyOrState.Length > 100)
                {
                    InterpretingLocationCountyOrState = InterpretingLocationCountyOrState.Substring(0, 100);
                }

                InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip.Trim();
                if (InterpretingLocationPostcodeOrZip.Length > 20)
                {
                    InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip.Substring(0, 20);
                }

                if (InterpretingLocationCountryID == 0)
                {
                    InterpretingLocationCountryID = null;
                }

                if (ExternalNotes != null)
                {
                    ExternalNotes = ExternalNotes.Trim();
                }


                NewQuoteItem = new QuoteItem()
                {
                    QuoteId = QuoteID,
                    LanguageServiceId = LanguageServiceID,
                    SourceLanguageIanacode = SourceLanguageIANAcode,
                    TargetLanguageIanacode = TargetLanguageIANAcode,
                    WordCountNew = WordCountNew,
                    WordCountFuzzyBand1 = WordCountFuzzyBand1,
                    WordCountFuzzyBand2 = WordCountFuzzyBand2,
                    WordCountFuzzyBand3 = WordCountFuzzyBand3,
                    WordCountFuzzyBand4 = WordCountFuzzyBand4,
                    WordCountExact = WordCountExact,
                    WordCountRepetitions = WordCountRepetitions,
                    WordCountPerfectMatches = WordCountPerfectMatches,
                    WordCountClientSpecific = WordCountClientSpecific,
                    Pages = Pages,
                    Characters = Characters,
                    Documents = Documents,
                    InterpretingExpectedDurationMinutes = InterpretingExpectedDurationMinutes,
                    InterpretingLocationOrgName = InterpretingLocationOrgName,
                    InterpretingLocationAddress1 = InterpretingLocationAddress1,
                    InterpretingLocationAddress2 = InterpretingLocationAddress2,
                    InterpretingLocationAddress3 = InterpretingLocationAddress3,
                    InterpretingLocationAddress4 = InterpretingLocationAddress4,
                    InterpretingLocationCountyOrState = InterpretingLocationCountyOrState,
                    InterpretingLocationPostcodeOrZip = InterpretingLocationPostcodeOrZip,
                    InterpretingLocationCountryId = InterpretingLocationCountryID,
                    AudioMinutes = AudioMinutes,
                    WorkMinutes = WorkMinutes,
                    ExternalNotes = ExternalNotes,
                    ChargeToClient = ChargeToClient,
                    SupplierWordCountNew = SupplierWordCountNew,
                    SupplierWordCountFuzzyBand1 = SupplierWordCountFuzzyBand1,
                    SupplierWordCountFuzzyBand2 = SupplierWordCountFuzzyBand2,
                    SupplierWordCountFuzzyBand3 = SupplierWordCountFuzzyBand3,
                    SupplierWordCountFuzzyBand4 = SupplierWordCountFuzzyBand4,
                    SupplierWordCountExact = SupplierWordCountExact,
                    SupplierWordCountRepetitions = SupplierWordCountRepetitions,
                    SupplierWordCountPerfectMatches = SupplierWordCountPerfectMatches,
                    SupplierWordCountClientSpecific = SupplierWordCountClientSpecific,
                    CreatedDateTime = timeZonesService.GetCurrentGMT(),
                    CreatedByEmployeeId = CreatedByEmployeeID,
                    LanguageServiceCategoryId = (byte?)languageServiceCategoryId
                };

                await quoteItemRepository.AddAsync(NewQuoteItem);
                await quoteItemRepository.SaveChangesAsync();

                return NewQuoteItem;

            }
        }

        public async Task<Quote> GetCurrentQuote(int enquiryId)
        {
            var result = await quoteRepository.All().Where(e => e.IsCurrentVersion == true && e.EnquiryId == enquiryId && e.DeletedDateTime == null).FirstOrDefaultAsync();

            return result;
        }

        public async Task<Quote> GetQuoteById(int quoteId)
        {
            var result = await quoteRepository.All().Where(q => q.Id == quoteId && q.DeletedDateTime == null).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<QuoteItem>> GetAllQuoteItems(int quoteId)
        {
            var result = await quoteItemRepository.All().Where(q => q.QuoteId == quoteId && q.DeletedDateTime == null).ToListAsync();

            return result;
        }
        public List<int> GetAllQuotesForApprovalPage(int contactID, int orgID, int orgGroupID, int start, int pageSize)
        {
            List<int> model = new List<int>();
            SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
            SqlDataAdapter QuotesDataAdapter = new SqlDataAdapter("procGetQuoteIDsForContact", SQLConn);

            QuotesDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            QuotesDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ContactID", SqlDbType.Int)).Value = contactID;
            QuotesDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.Int)).Value = orgID;
            QuotesDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@OrgGroupID", SqlDbType.Int)).Value = orgGroupID;
            QuotesDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StartRowIndex", SqlDbType.Int)).Value = start;
            QuotesDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@MaximumRows", SqlDbType.Int)).Value = pageSize;




            try
            {
                QuotesDataAdapter.SelectCommand.Connection.Open();
                QuotesDataAdapter.SelectCommand.ExecuteScalar();
                DataSet QuoteIDs = new DataSet();

                QuotesDataAdapter.Fill(QuoteIDs);

                if (QuoteIDs.Tables.Count > 0)
                {
                    int count = QuoteIDs.Tables[0].Rows.Count;
                    foreach (DataRow QuoteRow in QuoteIDs.Tables[0].Rows)
                    {
                        int QuoteID = (int)QuoteRow["ID"];
                        model.Add(QuoteID);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error running stored procedure.");
            }
            finally
            {
                try
                {
                    QuotesDataAdapter.SelectCommand.Connection.Close();
                    QuotesDataAdapter.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw;
                }
            }

            return model;
        }
        public string ExtranetAndWSDirectoryPathForApp(int quoteId)
        {

            var quote = quoteRepository.All().Where(x => x.Id == quoteId).FirstOrDefault();
            var enquiry = enquiriesRepo.All().Where(x => x.Id == quote.EnquiryId).FirstOrDefault();

            string ContactDirSearchPattern = enquiry.ContactId.ToString() + "*";

            string EnquiryDirSearchPattern = enquiry.Id.ToString() + "*";

            string ContactDirPath = Path.Combine(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"), enquiry.ContactId.ToString());
            string ExtranetBaseDir = Path.Combine(ContactDirPath, enquiry.Id.ToString() + " - " + quote.QuoteDate.ToString("dd MMM yy"));

            if (System.IO.Directory.Exists(ExtranetBaseDir) == true)
            {
                string FileToDownloadFullPath = Path.Combine(ExtranetBaseDir, string.Format("{0} - {1}.pdf", quote.Id.ToString(), quote.QuoteFileName));
                if (System.IO.File.Exists(FileToDownloadFullPath) == true)
                {
                    return FileToDownloadFullPath;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }
        public async Task<int> UpdateQuote(int QuoteID, int EnquiryID, bool IsCurrentVersion, Int16 QuoteCurrencyID, string LangIANACode, string Title, string QuoteFileName, string InternalNotes, DateTime QuoteDate, string QuoteOrgName, string QuoteAddress1, string QuoteAddress2, string QuoteAddress3, string QuoteAddress4, string QuoteCountyOrState, string QuotePostcodeOrZip, Int16 QuoteCountryID, string AddresseeSalutationName, string OpeningSectionText, string ClosingSectionText, Int16 TimelineUnit, decimal TimelineValue, Int16 WordCountPresentationOption, bool ShowInterpretingDurationInBreakdown, bool ShowWorkDurationInBreakdown, bool ShowPagesOrSlidesInBreakdown, bool ShowNumberOfCharactersInBreakdown, bool ShowNumberOfDocumentsInBreakdown, Int16 SalesContactEmployeeID, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, bool ShowCustomerSpecificField1Value, bool ShowCustomerSpecificField2Value, bool ShowCustomerSpecificField3Value, bool ShowCustomerSpecificField4Value, string ClientPONumber, DateTime LastModifiedDateTime, Int16 LastModifiedByEmployee, bool PrintingProject = false, Int16 AssignedToEmployee = 0)
        {

            // ensure the quote is being created against a valid enquiry ID
            if (await QuoteExists(QuoteID) == false)
            {
                throw new Exception("The quote ID, " + QuoteID.ToString() + ", does not exist in the database. A quote item must be created against a valid quote.");
            }
            else
            {

                // trim strings as a precaution
                LangIANACode = LangIANACode.Trim();
                if (LangIANACode.Length > 20)
                {
                    LangIANACode = LangIANACode.Substring(0, 20);

                }
                Title = Title.Trim();
                if (Title.Length > 500)
                {
                    Title = Title.Substring(0, 500);
                }

                QuoteOrgName = QuoteOrgName.Trim();
                if (QuoteOrgName.Length > 200)
                {
                    QuoteOrgName = QuoteOrgName.Substring(0, 200);
                }

                QuoteAddress1 = QuoteAddress1.Trim();
                if (QuoteAddress1.Length > 100)
                {
                    QuoteAddress1 = QuoteAddress1.Substring(0, 100);
                }

                QuoteAddress2 = QuoteAddress2.Trim();
                if (QuoteAddress2.Length > 100)
                {
                    QuoteAddress2 = QuoteAddress2.Substring(0, 100);
                }

                QuoteAddress3 = QuoteAddress3.Trim();
                if (QuoteAddress3.Length > 100)
                {
                    QuoteAddress3 = QuoteAddress3.Substring(0, 100);
                }

                QuoteAddress4 = QuoteAddress4.Trim();
                if (QuoteAddress4.Length > 100)
                {
                    QuoteAddress4 = QuoteAddress4.Substring(0, 100);
                }

                QuoteCountyOrState = QuoteCountyOrState.Trim();
                if (QuoteCountyOrState.Length > 100)
                {
                    QuoteCountyOrState = QuoteCountyOrState.Substring(0, 100);
                }

                QuotePostcodeOrZip = QuotePostcodeOrZip.Trim();
                if (QuotePostcodeOrZip.Length > 20)
                {
                    QuotePostcodeOrZip = QuotePostcodeOrZip.Substring(0, 20);
                }

                AddresseeSalutationName = AddresseeSalutationName.Trim();
                if (AddresseeSalutationName.Length > 100)
                {
                    AddresseeSalutationName = AddresseeSalutationName.Substring(0, 100);
                }

                OpeningSectionText = OpeningSectionText.Trim();
                ClosingSectionText = ClosingSectionText.Trim();
                if (InternalNotes != null)
                {
                    InternalNotes = InternalNotes.Trim();
                }

                if (ClientPONumber != null)
                {
                    ClientPONumber = ClientPONumber.Trim();

                    if (ClientPONumber.Length > 100)
                    {
                        ClientPONumber = ClientPONumber.Substring(0, 100);
                    }
                }

                if (CustomerSpecificField1Value != null)
                {
                    CustomerSpecificField1Value = CustomerSpecificField1Value.Trim();
                }

                if (CustomerSpecificField2Value != null)
                {
                    CustomerSpecificField2Value = CustomerSpecificField2Value.Trim();
                }

                if (CustomerSpecificField3Value != null)
                {
                    CustomerSpecificField3Value = CustomerSpecificField3Value.Trim();
                }

                if (CustomerSpecificField4Value != null)
                {
                    CustomerSpecificField4Value = CustomerSpecificField4Value.Trim();
                }



                SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
                var QuoteDataAdapter = new SqlDataAdapter();
                QuoteDataAdapter.UpdateCommand = new SqlCommand("procUpdateQuoteDetails", SQLConn);
                {
                    var withBlock = QuoteDataAdapter.UpdateCommand;
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.Parameters.Add("@QuoteID", SqlDbType.Int).Value = QuoteID;
                    withBlock.Parameters.Add("@EnquiryID", SqlDbType.Int).Value = EnquiryID;
                    withBlock.Parameters.Add("@IsCurrentVersion", SqlDbType.Bit).Value = IsCurrentVersion;
                    withBlock.Parameters.Add("@QuoteCurrencyID", SqlDbType.SmallInt).Value = QuoteCurrencyID;
                    withBlock.Parameters.Add("@LangIANACode", SqlDbType.NVarChar, 20).Value = LangIANACode;
                    withBlock.Parameters.Add("@Title", SqlDbType.NVarChar, 500).Value = Title;
                    withBlock.Parameters.Add("@QuoteFileName", SqlDbType.NVarChar, 500).Value = GeneralUtils.MakeStringSafeForFileSystemPath(QuoteFileName);
                    withBlock.Parameters.Add("@InternalNotes", SqlDbType.NVarChar, -1).Value = InternalNotes;
                    if (QuoteDate != DateTime.MinValue)
                        withBlock.Parameters.Add("@QuoteDate", SqlDbType.Date).Value = QuoteDate;
                    else
                        withBlock.Parameters.Add("@QuoteDate", SqlDbType.Date).Value = DBNull.Value;
                    withBlock.Parameters.Add("@QuoteOrgName", SqlDbType.NVarChar, 200).Value = QuoteOrgName;
                    withBlock.Parameters.Add("@QuoteAddress1", SqlDbType.NVarChar, 100).Value = QuoteAddress1;
                    withBlock.Parameters.Add("@QuoteAddress2", SqlDbType.NVarChar, 100).Value = QuoteAddress2;
                    withBlock.Parameters.Add("@QuoteAddress3", SqlDbType.NVarChar, 100).Value = QuoteAddress3;
                    withBlock.Parameters.Add("@QuoteAddress4", SqlDbType.NVarChar, 100).Value = QuoteAddress4;
                    withBlock.Parameters.Add("@QuoteCountyOrState", SqlDbType.NVarChar, 100).Value = QuoteCountyOrState;
                    withBlock.Parameters.Add("@QuotePostcodeOrZip", SqlDbType.NVarChar, 20).Value = QuotePostcodeOrZip;
                    withBlock.Parameters.Add("@QuoteCountryID", SqlDbType.SmallInt).Value = QuoteCountryID;
                    withBlock.Parameters.Add("@AddresseeSalutationName", SqlDbType.NVarChar, 100).Value = AddresseeSalutationName;
                    withBlock.Parameters.Add("@OpeningSectionText", SqlDbType.NVarChar, -1).Value = OpeningSectionText;
                    withBlock.Parameters.Add("@ClosingSectionText", SqlDbType.NVarChar, -1).Value = ClosingSectionText;
                    withBlock.Parameters.Add("@TimelineUnit", SqlDbType.TinyInt).Value = TimelineUnit;
                    withBlock.Parameters.Add("@TimelineValue", SqlDbType.Float).Value = TimelineValue;
                    withBlock.Parameters.Add("@WordCountPresentationOption", SqlDbType.TinyInt).Value = WordCountPresentationOption;
                    withBlock.Parameters.Add("@ShowInterpretingDurationInBreakdown", SqlDbType.Bit).Value = ShowInterpretingDurationInBreakdown;
                    withBlock.Parameters.Add("@ShowWorkDurationInBreakdown", SqlDbType.Bit).Value = ShowWorkDurationInBreakdown;
                    withBlock.Parameters.Add("@ShowPagesOrSlidesInBreakdown", SqlDbType.Bit).Value = ShowPagesOrSlidesInBreakdown;
                    withBlock.Parameters.Add("@ShowNumberOfCharactersInBreakdown", SqlDbType.Bit).Value = ShowNumberOfCharactersInBreakdown;
                    withBlock.Parameters.Add("@ShowNumberOfDocumentsInBreakdown", SqlDbType.Bit).Value = ShowNumberOfDocumentsInBreakdown;
                    withBlock.Parameters.Add("@PrintingProject", SqlDbType.Bit).Value = PrintingProject;
                    withBlock.Parameters.Add("@SalesContactEmployeeID", SqlDbType.SmallInt).Value = SalesContactEmployeeID;
                    withBlock.Parameters.Add("@CustomerSpecificField1Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField1Value.ToString();
                    withBlock.Parameters.Add("@CustomerSpecificField2Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField2Value.ToString();
                    withBlock.Parameters.Add("@CustomerSpecificField3Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField3Value.ToString();
                    withBlock.Parameters.Add("@CustomerSpecificField4Value", SqlDbType.NVarChar, -1).Value = CustomerSpecificField4Value.ToString();

                    withBlock.Parameters.Add("@ShowCustomerSpecificField1Value", SqlDbType.Bit).Value = ShowCustomerSpecificField1Value;
                    withBlock.Parameters.Add("@ShowCustomerSpecificField2Value", SqlDbType.Bit).Value = ShowCustomerSpecificField2Value;
                    withBlock.Parameters.Add("@ShowCustomerSpecificField3Value", SqlDbType.Bit).Value = ShowCustomerSpecificField3Value;
                    withBlock.Parameters.Add("@ShowCustomerSpecificField4Value", SqlDbType.Bit).Value = ShowCustomerSpecificField4Value;
                    withBlock.Parameters.Add("@ClientPONumber", SqlDbType.NVarChar, 100).Value = ClientPONumber;
                    if (AssignedToEmployee != 0)
                        withBlock.Parameters.Add("@AssignedToEmployeeID", SqlDbType.SmallInt).Value = AssignedToEmployee;
                    else
                        withBlock.Parameters.Add("@AssignedToEmployeeID", SqlDbType.SmallInt).Value = DBNull.Value;

                    // log addition
                    withBlock.Parameters.Add("@LastModifiedDateTime", SqlDbType.DateTime).Value = GeneralUtils.GetCurrentGMT();
                    withBlock.Parameters.Add("@LastModifiedByEmployeeID", SqlDbType.SmallInt).Value = LastModifiedByEmployee;

                    // return value parameter:
                    SqlParameter ReturnValParam = withBlock.Parameters.Add("@RowCount", SqlDbType.Int);
                    ReturnValParam.Direction = ParameterDirection.ReturnValue;
                }

                try
                {
                    QuoteDataAdapter.UpdateCommand.Connection.Open();
                    QuoteDataAdapter.UpdateCommand.ExecuteNonQuery();
                    if (System.Convert.ToInt32(QuoteDataAdapter.UpdateCommand.Parameters["@RowCount"].Value) != 1)
                    {
                        throw new Exception("Quote update was not successful.");
                    }
                    await MarkQuoteCurrentVersion(QuoteID, EnquiryID);
                    return QuoteID;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    try
                    {
                        // Clean up
                        QuoteDataAdapter.UpdateCommand.Connection.Close();
                        QuoteDataAdapter.Dispose();
                    }
                    catch (SqlException SE)
                    {
                        throw new Exception("SQL exception: " + SE.Message);
                    }
                }
            }// if quote is being updated against a valid quote ID // if quote is being updated against a valid enquiry ID
        }
        public async Task<int> MarkQuoteCurrentVersion(int QuoteID, int EnquiryID)
        {

            var Details = await enquiriesRepo.All().Where(x => x.Id == EnquiryID).FirstOrDefaultAsync();

            // ensure a valid order ID has been passed
            if (await QuoteExists(QuoteID) == false)
            {
                throw new Exception("The quote ID, " + QuoteID.ToString() + ", does not exist in the database.");
            }
            else if (Details == null)
            {
                throw new Exception("The enquiry ID, " + QuoteID.ToString() + ", does not exist in the database.");
            }
            else
            {

                SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
                var DataAdapter = new SqlDataAdapter();
                DataAdapter.UpdateCommand = new SqlCommand("procUpdateQuoteCurrentVersion", SQLConn);
                DataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;

                DataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_QUOTE_ID", SqlDbType.Int)).Value = QuoteID;
                DataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_ENQUIRY_ID", SqlDbType.Int)).Value = EnquiryID;

                // return value parameter:
                SqlParameter ReturnValParam = DataAdapter.UpdateCommand.Parameters.Add("@RowCount", SqlDbType.Int);
                ReturnValParam.Direction = ParameterDirection.ReturnValue;

                try
                {
                    DataAdapter.UpdateCommand.Connection.Open();
                    DataAdapter.UpdateCommand.ExecuteNonQuery();

                    if (System.Convert.ToInt32(DataAdapter.UpdateCommand.Parameters["@RowCount"].Value) < 1)
                    {
                        throw new Exception("Marking quote as current version was not successful.");
                    }
                    return QuoteID;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error running stored procedure to mark quote as curent version.");
                }
                finally
                {
                    try
                    {
                        // Clean up
                        DataAdapter.UpdateCommand.Connection.Close();
                        DataAdapter.Dispose();
                    }
                    catch (SqlException SE)
                    {
                        throw new Exception("SQL exception: " + SE.Message);
                    }
                }
            }
        }

        public async Task<ViewModels.Quotes.QuotesViewModel> GetViewModelById(int quoteId)
        {
            var result = await quoteRepository.All().Where(e => e.Id == quoteId)
                .Select(x => new ViewModels.Quotes.QuotesViewModel()
                {
                    Id = x.Id,
                    EnquiryId = x.EnquiryId,
                    IsCurrentVersion = x.IsCurrentVersion,
                    QuoteCurrencyId = x.QuoteCurrencyId,
                    LangIanacode = x.LangIanacode,
                    Title = x.Title,
                    QuoteFileName = x.QuoteFileName,
                    InternalNotes = x.InternalNotes,
                    QuoteDate = x.QuoteDate,
                    QuoteOrgName = x.QuoteOrgName,
                    QuoteAddress1 = x.QuoteAddress1,
                    QuoteAddress2 = x.QuoteAddress2,
                    QuoteAddress3 = x.QuoteAddress3,
                    QuoteAddress4 = x.QuoteAddress4,
                    QuoteCountyOrState = x.QuoteCountyOrState,
                    QuotePostcodeOrZip = x.QuotePostcodeOrZip,
                    QuoteCountryId = x.QuoteCountryId,
                    AddresseeSalutationName = x.AddresseeSalutationName,
                    OpeningSectionText = x.OpeningSectionText,
                    ClosingSectionText = x.ClosingSectionText,
                    TimelineUnit = x.TimelineUnit,
                    TimelineValue = x.TimelineValue,
                    WordCountPresentationOption = x.WordCountPresentationOption,
                    ShowInterpretingDurationInBreakdown = x.ShowInterpretingDurationInBreakdown,
                    ShowWorkDurationInBreakdown = x.ShowWorkDurationInBreakdown,
                    ShowPagesOrSlidesInBreakdown = x.ShowPagesOrSlidesInBreakdown,
                    ShowNumberOfCharactersInBreakdown = x.ShowNumberOfCharactersInBreakdown,
                    ShowNumberOfDocumentsInBreakdown = x.ShowNumberOfDocumentsInBreakdown,
                    SalesContactEmployeeId = x.SalesContactEmployeeId,
                    CreatedByEmployeeId = x.CreatedByEmployeeId,
                    CreatedDateTime = x.CreatedDateTime,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    LastModifiedDateTime = x.LastModifiedDateTime,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    DeletedDateTime = x.DeletedDateTime,
                    OverallChargeToClient = x.OverallChargeToClient,
                    DiscountId = x.DiscountId,
                    SurchargeId = x.SurchargeId,
                    SubTotalOverallChargeToClient = x.SubTotalOverallChargeToClient,
                    DiscountAmount = x.DiscountAmount,
                    SurchargeAmount = x.SurchargeAmount,
                    CustomerSpecificField1Value = x.CustomerSpecificField1Value,
                    CustomerSpecificField2Value = x.CustomerSpecificField2Value,
                    CustomerSpecificField3Value = x.CustomerSpecificField3Value,
                    CustomerSpecificField4Value = x.CustomerSpecificField4Value,
                    ShowCustomerSpecificField1Value = x.ShowCustomerSpecificField1Value,
                    ShowCustomerSpecificField2Value = x.ShowCustomerSpecificField2Value,
                    ShowCustomerSpecificField3Value = x.ShowCustomerSpecificField3Value,
                    ShowCustomerSpecificField4Value = x.ShowCustomerSpecificField4Value,
                    ClientPonumber = x.ClientPonumber,
                    AssignedToEmployeeId = x.AssignedToEmployeeId,
                    CreatedAutomatically = x.CreatedAutomatically,
                    PrintingProject = x.PrintingProject,
                })
                .FirstOrDefaultAsync();


            return result;
        }

        public async Task<Quote> UpdateQuoteInformation(int quoteID, short QuoteCurrencyId, string LangIanacode,
                                    string Title, string QuoteFileName, string InternalNotes, string OpeningSectionText, string ClosingSectionText, short lastModifiedByEmployeeID)
        {
            var quote = await GetQuoteById(quoteID);

            // maybe add security check later here and autoMapper
            quote.QuoteCurrencyId = QuoteCurrencyId;
            quote.LangIanacode = LangIanacode;
            quote.Title = Title.Replace("\n","#");
            quote.QuoteFileName = QuoteFileName;
            quote.InternalNotes = InternalNotes;
            quote.LastModifiedByEmployeeId = lastModifiedByEmployeeID;

            if (OpeningSectionText != "")
            {
                quote.OpeningSectionText = OpeningSectionText;

            }
            if (ClosingSectionText != "")
            {
                quote.ClosingSectionText = ClosingSectionText;

            }

            quoteRepository.Update(quote);
            await quoteRepository.SaveChangesAsync();

            return quote;
        }


        public async Task<Quote> UpdateQuoteOptions(int quoteID, double TimelineValue, byte TimelineUnit, byte WordCountPresentationOption, bool ShowInterpretingDurationInBreakdown, bool ShowWorkDurationInBreakdown, bool ShowPagesOrSlidesInBreakdown, bool ShowNumberOfCharactersInBreakdown, bool ShowNumberOfDocumentsInBreakdown,
                                     string ClientPonumber, string CustomerSpecificField1Value, string CustomerSpecificField2Value, string CustomerSpecificField3Value, string CustomerSpecificField4Value, bool ShowCustomerSpecificField1Value,
                                     bool ShowCustomerSpecificField2Value, bool ShowCustomerSpecificField3Value, bool ShowCustomerSpecificField4Value, string AddresseeSalutationName, short lastModifiedByEmployeeID)
        {
            var quote = await GetQuoteById(quoteID);

            // maybe add security check later here and autoMapper
            quote.TimelineValue = TimelineValue;
            quote.TimelineUnit = TimelineUnit;
            quote.WordCountPresentationOption = WordCountPresentationOption;
            quote.ShowInterpretingDurationInBreakdown = ShowInterpretingDurationInBreakdown;
            quote.ShowWorkDurationInBreakdown = ShowWorkDurationInBreakdown;
            quote.ShowPagesOrSlidesInBreakdown = ShowPagesOrSlidesInBreakdown;
            quote.ShowNumberOfCharactersInBreakdown = ShowNumberOfCharactersInBreakdown;
            quote.ShowNumberOfDocumentsInBreakdown = ShowNumberOfDocumentsInBreakdown;
            quote.ClientPonumber = ClientPonumber;
            quote.CustomerSpecificField1Value = CustomerSpecificField1Value;
            quote.CustomerSpecificField2Value = CustomerSpecificField2Value;
            quote.CustomerSpecificField3Value = CustomerSpecificField3Value;
            quote.CustomerSpecificField3Value = CustomerSpecificField4Value;
            quote.ShowCustomerSpecificField1Value = ShowCustomerSpecificField1Value;
            quote.ShowCustomerSpecificField2Value = ShowCustomerSpecificField2Value;
            quote.ShowCustomerSpecificField3Value = ShowCustomerSpecificField3Value;
            quote.ShowCustomerSpecificField4Value = ShowCustomerSpecificField4Value;
            quote.AddresseeSalutationName = AddresseeSalutationName;
            quote.LastModifiedByEmployeeId = lastModifiedByEmployeeID;

            quoteRepository.Update(quote);
            await quoteRepository.SaveChangesAsync();

            return quote;
        }

        public async Task<Quote> UpdateQuotePDFDetails(int quoteID, DateTime QuoteDate, string QuoteOrgName, string QuoteAddress1, string QuoteAddress2, string QuoteAddress3, string QuoteAddress4, string QuoteCountyOrState, string QuotePostcodeOrZip,
                                     short QuoteCountryId, string Title, string OpeningSectionText, string ClosingSectionText, short SalesContactEmployeeId, short lastModifiedByEmployeeID)
        {
            var quote = await GetQuoteById(quoteID);

            // maybe add security check later here and autoMapper
            quote.QuoteDate = QuoteDate;
            quote.QuoteOrgName = QuoteOrgName;
            quote.QuoteAddress1 = QuoteAddress1;
            quote.QuoteAddress2 = QuoteAddress2;
            quote.QuoteAddress3 = QuoteAddress3;
            quote.QuoteAddress4 = QuoteAddress4;
            quote.QuoteCountyOrState = QuoteCountyOrState;
            quote.QuotePostcodeOrZip = QuotePostcodeOrZip;
            quote.QuoteCountryId = QuoteCountryId;
            //quote.Title = Title;
            quote.SalesContactEmployeeId = SalesContactEmployeeId;
            quote.LastModifiedByEmployeeId = lastModifiedByEmployeeID;

            if (quote.AddresseeSalutationName != "")
            {
                OpeningSectionText = OpeningSectionText.Replace("{name}", quote.AddresseeSalutationName);
            }
            quote.OpeningSectionText = OpeningSectionText;

            var tunraroundtime = quote.TimelineValue.ToString() + " ";
            switch (quote.LangIanacode)
            {
                case "en":
                    if (quote.TimelineUnit == 2)
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "working week";
                        }
                        else
                        {
                            tunraroundtime += "working weeks";
                        }
                    }
                    else
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "working day";
                        }
                        else
                        {
                            tunraroundtime += "working days";
                        }
                    }
                    break;
                case "de":
                    if (quote.TimelineUnit == 2)
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "Arbeitswoche";
                        }
                        else
                        {
                            tunraroundtime += "Arbeitswochen";
                        }
                    }
                    else
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "Arbeitstag";
                        }
                        else
                        {
                            tunraroundtime += "Arbeitstage";
                        }
                    }
                    break;
                case "sv":
                    if (quote.TimelineUnit == 2)
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "arbetsvecka";
                        }
                        else
                        {
                            tunraroundtime += "arbetsveckor";
                        }
                    }
                    else
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "arbetsdag";
                        }
                        else
                        {
                            tunraroundtime += "arbetsdagar";
                        }
                    }
                    break;
                case "da":
                    if (quote.TimelineUnit == 2)
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "arbejdsuge";
                        }
                        else
                        {
                            tunraroundtime += "arbejdsuger";
                        }
                    }
                    else
                    {
                        if (quote.TimelineValue == 1)
                        {
                            tunraroundtime += "arbejdsdag";
                        }
                        else
                        {
                            tunraroundtime += "arbejdsdage";
                        }
                    }
                    break;
            }

            ClosingSectionText = ClosingSectionText.Replace("{turnaroundtime}", tunraroundtime);
            quote.ClosingSectionText = ClosingSectionText;

            quoteRepository.Update(quote);
            await quoteRepository.SaveChangesAsync();

            return quote;
        }
        public async Task<Quote> UpdateSurchargeID(int Id, int surchargeId, short? lastModifiedByUserId)
        {
            var quote = await GetQuoteById(Id);

            // maybe add security check later here and autoMapper
            if (surchargeId != -1)
            {
                quote.SurchargeId = surchargeId;
            }
            else
            {
                quote.SurchargeId = null;
            }
            if (lastModifiedByUserId != null)
            {
                quote.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
                quote.LastModifiedByEmployeeId = lastModifiedByUserId;
            }
            quoteRepository.Update(quote);
            await quoteRepository.SaveChangesAsync();

            return quote;
        }

        public async Task<Quote> UpdateDiscountID(int Id, int discountId, short? lastModifiedByUserId)
        {
            var quote = await GetQuoteById(Id);

            // maybe add security check later here and autoMapper
            if (discountId != -1)
            {
                quote.DiscountId = discountId;
            }
            else
            {
                quote.DiscountId = null;
            }
            if (lastModifiedByUserId != null)
            {
                quote.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
                quote.LastModifiedByEmployeeId = lastModifiedByUserId;
            }
            quoteRepository.Update(quote);
            await quoteRepository.SaveChangesAsync();

            return quote;
        }

    }
}

