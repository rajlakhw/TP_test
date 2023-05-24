using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.ClientInvoice;

namespace Services
{
    public class TPClientInvoicesLogic : ITPClientInvoicesLogic
    {
        private readonly IRepository<ClientInvoice> clientInvoiceRepository;
        private readonly IRepository<AutoClientInvoicingSetting> autoClientInvoiceRepository;
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<Org> orgRepository;

        public TPClientInvoicesLogic(IRepository<AutoClientInvoicingSetting> autoClientInvoiceRepository,
            IRepository<ClientInvoice> clientInvoiceRepository,
            IRepository<Contact> contactRepo,
            IRepository<Org> orgRepo)
        {
            this.autoClientInvoiceRepository = autoClientInvoiceRepository;
            this.clientInvoiceRepository = clientInvoiceRepository;
            this.contactRepository = contactRepo;
            this.orgRepository = orgRepo;
        }

        public async Task<int> GetAutoClientInvoicingSettingsId(int dataObjectId, int dataTypeId)
            => await autoClientInvoiceRepository.All().Where(x => x.DeletedDate == null && x.DataObjectId == dataObjectId && x.DataObjectTypeId == dataTypeId).Select(x => x.Id).FirstOrDefaultAsync();

        public async Task<ClientInvoice> GetById(int ID)
        {
            var result = await clientInvoiceRepository.All().Where(a => a.Id == ID && a.DeletedDateTime == null).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ClientInvoiceViewModel> GetViewModelById(int id)
        {
            var invoice = await clientInvoiceRepository.All().Where(x => x.Id == id)
                .Select(x => new ClientInvoiceViewModel()
                {
                    Id = x.Id,
                    ContactId = x.ContactId,
                    SecondContactId = x.SecondContactId,
                    OwnerEmployeeId = x.OwnerEmployeeId,
                    InvoiceDate = x.InvoiceDate,
                    IsFinalised = x.IsFinalised,
                    FinalisedDateTime = x.FinalisedDateTime,
                    InvoiceLangIanacode = x.InvoiceLangIanacode,
                    InvoiceCurrencyId = x.InvoiceCurrencyId,
                    ClientPonumber = x.ClientPonumber,
                    InvoiceTitle = x.InvoiceTitle,
                    InvoiceDescription = x.InvoiceDescription,
                    Vatrate = x.Vatrate,
                    InvoiceOrgName = x.InvoiceOrgName,
                    InvoiceAddress1 = x.InvoiceAddress1,
                    InvoiceAddress2 = x.InvoiceAddress2,
                    InvoiceAddress3 = x.InvoiceAddress3,
                    InvoiceAddress4 = x.InvoiceAddress4,
                    InvoiceCountyOrState = x.InvoiceCountyOrState,
                    InvoicePostcodeOrZip = x.InvoicePostcodeOrZip,
                    InvoiceCountryId = x.InvoiceCountryId,
                    ShowOrderPonumbersInBreakdown = x.ShowOrderPonumbersInBreakdown,
                    ShowNotesInBreakdown = x.ShowNotesInBreakdown,
                    ShowSourceLangsInBreakdown = x.ShowSourceLangsInBreakdown,
                    ShowTargetLangsInBreakdown = x.ShowTargetLangsInBreakdown,
                    ShowJobItemsInBreakdown = x.ShowJobItemsInBreakdown,
                    ShowContactNamesInBreakdown = x.ShowContactNamesInBreakdown,
                    WordCountPresentationOption = x.WordCountPresentationOption,
                    ShowInterpretingDurationInBreakdown = x.ShowInterpretingDurationInBreakdown,
                    ShowWorkDurationInBreakdown = x.ShowWorkDurationInBreakdown,
                    ShowPagesOrSlidesInBreakdown = x.ShowPagesOrSlidesInBreakdown,
                    ShowCustomerSpecificField1ValueInBreakdown = x.ShowCustomerSpecificField1ValueInBreakdown,
                    PaymentTermDays = x.PaymentTermDays,
                    AutoChaseWhenOverdue = x.AutoChaseWhenOverdue,
                    PaidDate = x.PaidDate,
                    PaidMethodId = x.PaidMethodId,
                    ExportedToSageDateTime = x.ExportedToSageDateTime,
                    OverallChargeToClient = x.OverallChargeToClient,
                    CreatedDateTime = x.CreatedDateTime,
                    CreatedByEmployeeId = x.CreatedByEmployeeId,
                    LastModifiedDateTime = x.LastModifiedDateTime,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    DeletedDateTime = x.DeletedDateTime,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    DeletedFreeTextReason = x.DeletedFreeTextReason,
                    EarlyPaymentDiscountUsed = x.EarlyPaymentDiscountUsed,
                    EarlyPaymentDiscountDate = x.EarlyPaymentDiscountDate,
                    AnticipatedFinalValueAmountExcludingVat = x.AnticipatedFinalValueAmountExcludingVat,
                    AnticipatedFinalVatamount = x.AnticipatedFinalVatamount,
                    FinalValueAmountExcludingVat = x.FinalValueAmountExcludingVat,
                    FinalVatamount = x.FinalVatamount,
                    FinalValueAmount = x.FinalValueAmount,
                    FinalValueAmountInGbp = x.FinalValueAmountInGbp,
                    CustSpecificPaymentTermDaysEnabled = x.CustSpecificPaymentTermDaysEnabled,
                    EonfilesDeletedDateTime = x.EonfilesDeletedDateTime,
                    ApplyLateFees = x.ApplyLateFees,
                    LatePaymentDaysCharged = x.LatePaymentDaysCharged,
                    ReminderLettersSent = x.ReminderLettersSent,
                    ReminderFeeCharge = x.ReminderFeeCharge
                }).FirstOrDefaultAsync();

            return invoice;
        }

        public async Task<List<ClientInvoice>> GetOverdueClientInvoicesForOrgID(int orgID)
        {
            var result = await clientInvoiceRepository.All()
                         .Where(i => i.DeletedDateTime == null && i.PaidDate == null && i.IsFinalised == true &&
                                (i.InvoiceDate.Value.AddDays(i.PaymentTermDays) < DateTime.Now))
                         .Join(contactRepository.All().Where(c => c.OrgId == orgID && c.DeletedDate == null),
                               ci => ci.ContactId,
                               c => c.Id,
                               (ci, c) => new { invoice = ci, contact = c })
                         .Join(orgRepository.All().Where(o => o.DeletedDate == null),
                               i => i.contact.OrgId,
                               o => o.Id,
                               (i, o) => new { invoice = i.invoice })
                         .Select(i => i.invoice)
                         .OrderBy(o => o.InvoiceDate)
                         .ToListAsync();

            return result;

        }

    }
}
