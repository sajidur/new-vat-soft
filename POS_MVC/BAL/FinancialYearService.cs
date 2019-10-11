using REX_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.BAL
{
    public class FinancialYearService
    {
        DBService<FinancialYear> service = new DBService<FinancialYear>();
        public List<FinancialYear> GetAll()
        {
            return service.GetAll().ToList();
        }
        public FinancialYear GetById(int? id = 0)
        {
            return service.GetById(id);
        }
        //public FinancialYear GetLatest()
        //{
        //    return service.GetAll().OrderByDescending(a=>a.Id).FirstOrDefault();
        //}

        public FinancialYear Save(FinancialYear cus)
        {
            return service.Save(cus);

        }
        public FinancialYear Update(FinancialYear t, int id)
        {
            return service.Update(t, id);

        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }
    }
}