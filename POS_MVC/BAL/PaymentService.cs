using RiceMill_MVC.Models;
using RiceMill_MVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.BAL
{
    public class PaymentService
    {

        DBService<LedgerPosting> service = new DBService<LedgerPosting>();
        
        public List<LedgerPosting> GetAll()
        {
            return service.GetAll().ToList();
        }
        public LedgerPosting GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public LedgerPosting Save(LedgerPosting cus)
        {
            var isExists = service.GetAll().Where(a => a.LedgerId == cus.LedgerId && a.YearId == cus.YearId).FirstOrDefault();
            var max = service.LastRow().OrderByDescending(a => a.Id).FirstOrDefault().Id;
            cus.Id = max + 1;
            if (isExists != null)
            {
                return null;
            }
            cus.YearId = CurrentSession.GetCurrentSession().FinancialYear;
            service.Save(cus);
            return cus;

        }
        public LedgerPosting Update(LedgerPosting t, int id)
        {
            return service.Update(t, id);
        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }
    }
}