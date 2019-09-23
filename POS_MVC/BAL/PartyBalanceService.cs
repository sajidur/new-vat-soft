using RiceMill_MVC.Models;
using RiceMill_MVC.Util;
using RiceMill_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.BAL
{
    public class PartyBalanceService
    {
        DBService<PartyBalance> service = new DBService<PartyBalance>();
        FinancialYearService year = new FinancialYearService();
        DBService<Supplier> supplierService = new DBService<Supplier>();
        DBService<PartyPaymentResponse> pService = new DBService<PartyPaymentResponse>();
        DBService<DueSummaryResponse> dueSummaryService = new DBService<DueSummaryResponse>();
        private DBService<rptCustomerLedger_Result> ledgerService = new DBService<rptCustomerLedger_Result>();

        int yearId = 0;
        public PartyBalanceService()
        {
            var session=CurrentSession.GetCurrentSession();
            if (session!=null)
            {
                yearId = session.FinancialYear;
            }
        }
        public List<PartyBalance> GetAll(DateTime fromDate,DateTime todate)
        {
            var supplierLedgerIds = supplierService.GetAll(c => c.SupplierType == "Rice Supplier").Select(b => b.LedgerId).ToList();
            var list= service.GetAll(a=>a.VoucherTypeId==(int)VoucherType.PurchaseInvoice && supplierLedgerIds.Contains(a.LedgerId) && a.FinancialYearId== yearId).ToList();
            //foreach (var item in list)
            //{
            //    var alreadyPaid = service.GetAll(a => a.AgainstInvoiceNo== item.InvoiceNo && a.LedgerId==item.LedgerId).Select(a=>a.Debit).Sum();
            //    item.Debit = alreadyPaid;
            //}
            return list;
        }
        public List<PartyBalance> GetAll()
        {
            return service.GetAll(a=>a.FinancialYearId==yearId).ToList();
        }

        public List<PartyBalance> GetAll(int ledgerId)
        {
            return service.GetAll(a=>a.LedgerId==ledgerId && a.FinancialYearId==yearId).ToList();
        }
        public List<PartyPaymentResponse> GetAllInvoice(int ledgerId,int customerOrSupplier)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@LedgerId", ledgerId);
            parameters.Add(param);
            var get = pService.ExecuteProcedure("Exec DailyTransaction '" + ledgerId+"','"+ customerOrSupplier + "','"+yearId+"'");
            return get;
        }
        public List<PartyPaymentResponse> DailyReceiveAndPayment(int customerOrSupplier,string fromDate,string toDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
           // var date = fromDate.Date.Date.ToString("yyyy-MM-dd");
            var get = pService.ExecuteProcedure("Exec DailyReceiveAndPayment '" + customerOrSupplier + "','"+fromDate+"','"+toDate+"'");
            return get;
        }

        public List<DueSummaryResponse> GetDueSummary(int reportType)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            var get = dueSummaryService.ExecuteProcedure("Exec DueSummary '" + CurrentSession.GetCurrentSession().FinancialYear + "','" + reportType + "',"+yearId+"");
            return get;

        }
        public List<rptCustomerLedger_Result> individualLedger(int type, int ledgerId,string fromDate,string toDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            return this.ledgerService.ExecuteProcedure(string.Concat(new object[] { "Exec rptCustomerLedger '", ledgerId, "','", type, "',"+yearId+",'"+fromDate+"','"+toDate+"'" }));
        }
        public PartyBalance GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public int GetId()
        {
            var lastrow= service.LastRow().OrderByDescending(a => a.PartyBalanceId).FirstOrDefault();
            if (lastrow==null)
            {
                return 1;
            }
            else
            {
                return lastrow.PartyBalanceId + 1;
            }
        }

        public PartyBalance Save(PartyBalance partyBalance)
        {
            var isExists = service.GetAll().Where(a=>a.LedgerId == partyBalance.LedgerId).OrderByDescending(a=>a.PartyBalanceId).FirstOrDefault();
            var max = service.LastRow().OrderByDescending(a => a.PartyBalanceId).FirstOrDefault();
            if (max==null)
            {
                partyBalance.PartyBalanceId = 1;
            }
            else
            {
                partyBalance.PartyBalanceId = max.PartyBalanceId + 1;

            }

            if (isExists != null)
            {
                if (partyBalance.Credit > 0)
                {
                    var newbalance = isExists.Balance + partyBalance.Credit;
                    partyBalance.Balance = newbalance;
                }
                if (partyBalance.Debit > 0)
                {
                    var newbalance = isExists.Balance - partyBalance.Debit;
                    partyBalance.Balance = newbalance;
                }

            }
            else
            {
                if (partyBalance.Credit > 0)
                {
                    partyBalance.Balance =  partyBalance.Credit;
                }
                if (partyBalance.Debit > 0)
                {
                    partyBalance.Balance = (-1)*partyBalance.Debit;
                }
            }
            partyBalance.FinancialYearId = CurrentSession.GetCurrentSession().FinancialYear;
            service.Save(partyBalance);
            //if (partyBalance != null)
            //{
            //    SqlParameter param = new SqlParameter("@LedgerId", partyBalance.LedgerId);
            //    List<SqlParameter> paramss = new List<SqlParameter>();

            //    paramss.Add(param);
            //    service.ExecuteProcedure("Exec BalanceReconcilation " + partyBalance.LedgerId + "",paramss,true);
            //}
            return partyBalance;

        }
        public PartyBalance GetByVoucher(int ledgerId, string voucherNo)
        {
            PartyBalance partyBalance = this.service.GetAll((PartyBalance a) => a.LedgerId == ledgerId && a.VoucherNo == voucherNo &&a.FinancialYearId==yearId).FirstOrDefault<PartyBalance>();
            return partyBalance;
        }
        public PartyBalance GetByLedgerPostingId(int ledgerId, string ledgerPostingId,string voucherNo,string invoiceNo)
        {
            PartyBalance partyBalance = this.service.GetAll((PartyBalance a) => a.LedgerId == ledgerId && a.extra2 == ledgerPostingId &&a.FinancialYearId==yearId).FirstOrDefault<PartyBalance>();
            if (partyBalance==null)
            {
                partyBalance= this.service.GetAll((PartyBalance a) => a.LedgerId == ledgerId && (a.VoucherNo == voucherNo) && a.FinancialYearId == yearId).FirstOrDefault<PartyBalance>();
            }
            return partyBalance;
        }

        public List<PartyBalance> GetPartyPayments(int ledgerId)
        {
            List<PartyBalance> partyBalances = this.service.GetAll((PartyBalance a) => a.LedgerId == ledgerId &&a.FinancialYearId==yearId).ToList<PartyBalance>();
            return partyBalances;
        }
        public PartyBalance Update(PartyBalance t, int id)
        {
            var res= service.Update(t, id);
            if (t != null)
            {
                service.ExecuteNonQuery("Exec BalanceReconcilation " + t.LedgerId + ","+yearId+"");
            }
            return res;
        }
        public int Delete(int id)
        {
            var balance = service.GetById(id);
            var res = service.Delete(id);
            if (balance != null)
            {
                service.ExecuteNonQuery("Exec BalanceReconcilation " + balance.LedgerId + "," + yearId + "");
            }
            return res;
        }
    }
}