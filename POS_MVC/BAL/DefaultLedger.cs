using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.BAL
{
    public enum DefaultLedger
    {
        Cash=1,
        ProfitAndLoss=2,
        AdvancePayment=3,
        Salary=4,
        ServiceAccount=5,
        PDCPayable=6,
        PDCReceivable=7,
        DiscountAllowed=8,
        DiscountReceived=9,
        SalesAccount=10,
        PurchaseAccount=11,
        ForexGainLoss=12
    }

    public enum VoucherType
    {
        OpeningBalance = 1,
        OpeningStock = 2,
        ContraVoucher = 3,
        PaymentVoucher = 4,
        ReceiptVoucher = 5,
        JournalVoucher = 6,
        PDCPayable = 7,
        PDCReceivable = 8,
        PDCClearance = 9,
        PurchaseOrder = 10,
        MaterialReceipt = 11,
        RejectionOut = 12,
        PurchaseInvoice = 13,
        PurchaseReturn = 14,
        SalesQuotation = 15,
        SalesOrder = 16,
        DeliveryNote = 17,
        RejectionIn = 18,
        SalesInvoice = 19,
        SalesReturn = 20,
        ServiceVoucher = 21,
        CreditNote = 22,
        DebitNote = 23,
        StockJournal = 24,
        PhysicalStock = 25,
        DailySalaryVoucher = 26,
        MonthlySalaryVoucher = 27,
        AdvancePayment = 28,
        PaymentVoucherDelete = 29,
        ReceiptVoucherDelete = 30
    }
}