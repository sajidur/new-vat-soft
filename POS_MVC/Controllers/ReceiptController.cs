using RiceMill_MVC.BAL;
using RiceMill_MVC.BLL;
using RiceMill_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiceMill_MVC.Controllers
{
    public class ReceiptController : Controller
    {
        LedgerPostingService postingService = new LedgerPostingService();
        PartyBalanceService partyBalanceService = new PartyBalanceService();
        CustomerService supplierService = new CustomerService();
        string ChkNo;
        string CSK;
        decimal Amount;
        // GET: Receipt
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Transfer()
        {
            return View();
        }
        public ActionResult LoadTodayReceive(DateTime clientDate)
        {
            var totalReceive = postingService.GetTodayReceipt(clientDate);
            return Json(totalReceive, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Transfer(int ledgerPostingId,int toLedgerId)
        {
            var transferMessage = "";
            var ledgerPost = postingService.GetById(ledgerPostingId);
            if (ledgerPost!= null)
            {
              var partybalance=  partyBalanceService.GetByLedgerPostingId(ledgerPost.LedgerId??0, ledgerPost.Id.ToString(), ledgerPost.VoucherNo,ledgerPost.InvoiceNo);
                if (partybalance!=null)
                {
                    partybalance.extra1+= "Transfer from " + partybalance.LedgerId;
                    ledgerPost.Extra1 += "Transfer from " + partybalance.LedgerId;

                    partybalance.LedgerId = toLedgerId;
                    partyBalanceService.Update(partybalance,partybalance.PartyBalanceId);

                    ledgerPost.LedgerId = toLedgerId;
                    postingService.Update(ledgerPost, ledgerPost.Id);
                    transferMessage = "Sucess";
                }
                else
                {
                    transferMessage = "Transfer Not possible";
                }
            }
            else
            {
                transferMessage = "Transfer Not possible";
            }
            return Json(transferMessage,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReceivePayment(string voucherNo, int supplierId, DateTime voucherDate, string notes, List<LedgerPosting> ledgerPosting, bool isSendSMS)
        {
            ActionResult actionResult;
            decimal num;
            decimal? credit;
            int? ledgerId;
            DateTime now;
            Customer supplierInfo = this.supplierService.GetById(new int?(supplierId));
            if (supplierInfo != null)
            {
                foreach (LedgerPosting item in ledgerPosting)
                {
                    num = new decimal();
                    item.Debit = new decimal?(num);
                    item.PostingDate = new DateTime?(voucherDate);
                    item.VoucherNo = voucherNo;
                    item.VoucherTypeId = new int?(5);
                    item.Extra1 = string.Concat("Voucher:", voucherNo, " ", notes);
                    this.postingService.Save(item);
                    credit = item.Credit;
                    this.Amount = (credit.HasValue ? credit.GetValueOrDefault() : decimal.Zero);
                    this.ChkNo = item.ChequeNo;
                    ledgerId = item.LedgerId;
                    if ((ledgerId.GetValueOrDefault() == 1 ? !ledgerId.HasValue : true))
                    {
                        string[] str = new string[] { "Ref:No- TT-DR", voucherNo, ",Dated:", null, null };
                        now = DateTime.Now;
                        str[3] = now.ToString("dd-MM-yyyy");
                        str[4] = ".";
                        this.ChkNo = string.Concat(str);
                    }
                    else
                    {
                        string[] strArrays = new string[] { "Ref:No- Cash-DR", voucherNo, ",Dated:", null, null };
                        now = DateTime.Now;
                        strArrays[3] = now.ToString("dd-MM-yyyy");
                        strArrays[4] = ".";
                        this.ChkNo = string.Concat(strArrays);
                    }
                }
                string smsmsg = this.ChkNo;
                LedgerPosting posting = new LedgerPosting()
                {
                    ChequeDate = new DateTime?(DateTime.Now),
                    VoucherNo = voucherNo,
                    ChequeNo = "",
                    VoucherTypeId = new int?(5),
                    LedgerId = supplierInfo.LedgerId,
                    PostingDate = new DateTime?(voucherDate)
                };
                LedgerPosting nullable = posting;
                credit = ledgerPosting.Sum<LedgerPosting>((LedgerPosting a) => a.Credit);
                nullable.Debit = new decimal?(credit.Value);
                num = new decimal();
                posting.Credit = new decimal?(num);
                posting.Extra1 = string.Concat("Voucher:", voucherNo, " ", notes);
                this.postingService.Save(posting);
                PartyBalance balance = new PartyBalance()
                {
                    AgainstVoucherTypeId = new int?(5),
                    VoucherNo = voucherNo,
                    PostingDate = new DateTime?(voucherDate)
                };
                PartyBalance partyBalance = balance;
                ledgerId = supplierInfo.LedgerId;
                partyBalance.LedgerId = (ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0);
                num = new decimal();
                balance.Debit = new decimal?(num);
                balance.Credit = posting.Debit;
                balance.VoucherTypeId = 5;
                balance.extra1 = string.Concat("Recipt Invoice: ", voucherNo, " Notes:", notes);
                balance.extra2 = posting.Id.ToString();
                this.partyBalanceService.Save(balance);
                if (isSendSMS)
                {
                    Customer customer = (new CustomerService()).GetById(new int?(supplierId));
                    CustomerService customerService = new CustomerService();
                    ledgerId = customer.LedgerId;
                    rptIndividualLedger_Result due = customerService.GetBalance((ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0));
                    string balanceText = "";
                    credit = due.Balance;
                    num = new decimal();
                    if ((credit.GetValueOrDefault() < num ? !credit.HasValue : true))
                    {
                        credit = due.Balance;
                        balanceText = string.Concat("Your Present Balance With Dada Rice Tk=", string.Format("{0:#,#.}", decimal.Round((credit.HasValue ? credit.GetValueOrDefault() : decimal.Zero)), ""), "/=.");
                    }
                    else
                    {
                        decimal minusOne = decimal.MinusOne;
                        credit = due.Balance;
                        balanceText = string.Concat("Your Present Balance With Dada Rice Tk=", string.Format("{0:#,#.}", minusOne * decimal.Round((credit.HasValue ? credit.GetValueOrDefault() : decimal.Zero)), ""), "/=.");
                    }
                    SMSEmailService sMSEmailService = new SMSEmailService();
                    string phone = supplierInfo.Phone;
                    string[] name = new string[] { "Dear ", customer.Name, " Tk=", null, null, null };
                    credit = balance.Credit;
                    name[3] = string.Format("{0:#,#.}", decimal.Round((credit.HasValue ? credit.GetValueOrDefault() : decimal.Zero)), "");
                    name[4] = "/-Received With Thanks As ";
                    name[5] = smsmsg;
                    sMSEmailService.SendOneToOneSingleSms(phone, string.Concat(string.Concat(name), balanceText));
                }
                actionResult = Json("", 0);
            }
            else
            {
                actionResult = Json("error", 0);
            }
            return actionResult;
        }
        public ActionResult GetInvoiceNumber()
        {
            string invoiceNumber =DateTime.Now.Year +
                new GlobalClass().GetMaxId("Id", "LedgerPosting");
            return Json(invoiceNumber, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllReceipt(string fromDate, string toDate)
        {
            //DateTime from = Convert.ToDateTime(fromDate);
            //DateTime to = Convert.ToDateTime(toDate);

            var partybalance = partyBalanceService.DailyReceiveAndPayment(1,fromDate,toDate);

            // var result = AutoMapper.Mapper.Map<List<PartyBalance>, List<PartyBalanceResponse>>(partybalance);
            return Json(partybalance, JsonRequestBehavior.AllowGet);
        }

    }
}