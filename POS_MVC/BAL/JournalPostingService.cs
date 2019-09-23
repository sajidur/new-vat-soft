using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RiceMill_MVC.Models;
using RiceMill_MVC.Util;

namespace RiceMill_MVC.BAL
{
    public class JournalPostingService
    {
        DBService<JournalMaster> journalMaster = new DBService<JournalMaster>();
        DBService<JournalDetail> journalDetails = new DBService<JournalDetail>();
        FinancialYearService financialYearService = new FinancialYearService();
        EmployeeService employeeService = new EmployeeService();

        public List<JournalMaster> GetAll(DateTime fromDate, DateTime toDate)
        {
            List<JournalMaster> list = this.journalMaster.GetAll((JournalMaster a) => (a.LadgerDate >= (DateTime?)fromDate) && (a.LadgerDate <= (DateTime?)toDate)).ToList<JournalMaster>();
            return list;
        }
        public List<JournalMaster> GetAll()
        {
            return journalMaster.GetAll().ToList();
        }
        public JournalMaster GetById(int? id = 0)
        {
            return journalMaster.GetById(id);
        }

        
        public JournalMaster Save(JournalMaster JMaster)
        {
            //    var ledgerDetails = ledgerService.GetById(ledger.LedgerId);
            //    var isExists = service.GetAll().Where(a => a.InvoiceNo == ledger.InvoiceNo && a.LedgerId == ledger.LedgerId).FirstOrDefault();
            var max = journalMaster.LastRow().OrderByDescending(a => a.Id).FirstOrDefault();
            if (max == null)
            {
                JMaster.Id = 1;
            }
            else
            {
                JMaster.Id = max.Id + 1;

            }
            var financialYear = CurrentSession.GetCurrentSession().FinancialYear;
            JMaster.YearId = CurrentSession.GetCurrentSession().FinancialYear;
            JMaster.VoucherTypeId = 6;
            JMaster.UserId = CurrentSession.GetCurrentSession().UserId;
            
            var result = journalMaster.Save(JMaster);
                return result;

        }
        public JournalDetail Save(JournalDetail JDetails)
        {
            var max = journalDetails.LastRow().OrderByDescending(a => a.Id).FirstOrDefault();
            if (max == null)
            {
                JDetails.Id = 1;
            }
            else
            {
                JDetails.Id = max.Id + 1;

            }
            int value = int.Parse(journalMaster.LastRow().OrderByDescending(p => p.Id).Select(r => r.Id).First().ToString());
            JDetails.JournalMasterId = value;


            var result = journalDetails.Save(JDetails);
            return result;
        }
    }


}