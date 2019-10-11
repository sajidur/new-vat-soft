using REX_MVC.Models;
using REX_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.BAL
{
    public class AccountGroupService
    {
        DBService<AccountGroup> service = new DBService<AccountGroup>();
        public List<AccountGroup> GetAll()
        {
            return service.GetAll().ToList();
        }
        public List<AccountGroup> GetAll(string nature)
        {
          
            if (nature == "Dr")
            {
                return service.GetAll(a => a.Nature == "Assets" || a.Nature == "Expenses").ToList();
            }
            else {
                return service.GetAll(a => a.Nature == "Liabilities" || a.Nature == "Income").ToList();
            }
           
        }
        public List<AccountGroup> GetAll(int groupId)
        {
            return service.GetAll(a=>a.GroupUnder==groupId).ToList();
        }
        public AccountGroup GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public AccountGroup Save(AccountGroup cus)
        {
            var isExists = service.GetAll().Where(a => a.AccountGroupName == cus.AccountGroupName).FirstOrDefault();
            var max = service.LastRow().OrderByDescending(a => a.Id).FirstOrDefault().Id;
            cus.Id = max + 1;
            if (isExists != null)
            {
                return null;
            }
            service.Save(cus);
            return cus;

        }
        public AccountGroup Update(AccountGroup t, int id)
        {
            return service.Update(t, id);
        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }
    }
}