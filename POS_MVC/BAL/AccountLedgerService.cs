using REX_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.BAL
{
    public class AccountLedgerService
    {
        DBService<AccountLedger> service = new DBService<AccountLedger>();
        PartyBalanceService partyBalance = new PartyBalanceService();
        AccountGroupService groupService = new AccountGroupService();
        List<AccountLedger> ledgerList = new List<AccountLedger>();
        public List<AccountLedger> GetAll()
        {
            return service.GetAll().ToList();
        }
        public List<AccountLedger> GetAllExpense()
        {
            List<AccountLedger> list = this.service.GetAll((AccountLedger a) => a.AccountGroupId == (int?)15 || a.AccountGroupId == (int?)13 || a.AccountGroupId == (int?)11).ToList<AccountLedger>();
            return list;
        }
        public List<AccountLedger> GetAllIncome()
        {
            List<AccountLedger> list = this.service.GetAll((AccountLedger a) => a.AccountGroupId == (int?)10 || a.AccountGroupId == (int?)12 || a.AccountGroupId == (int?)14).ToList<AccountLedger>();
            return list;
        }
        public List<AccountLedger> GetAll(int groupId)
        {
            var group = groupService.GetAll(groupId);
            var groupDetails = groupService.GetById(groupId);
            foreach (var item in groupDetails.AccountLedgers)
            {
                ledgerList.Add(item);
            }
            foreach (var item in group)
            {
                foreach (var l in item.AccountLedgers)
                {
                    ledgerList.Add(l);
                }
                GetAll(item.Id);
            }
            return ledgerList;
        }
        public List<AccountLedger> GetAll(string CrDr) {
            if (CrDr == "Dr")
            {
                return service.GetAll(a => a.CrOrDr == "Dr").ToList();
            }
            else {
                return service.GetAll(a => a.CrOrDr == "Cr").ToList();
            }
           
        }
        public AccountLedger GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public AccountLedger Save(AccountLedger cus)
        {
            var isExists = service.GetAll().Where(a => a.AccountGroupId == cus.AccountGroupId && a.LedgerName == cus.LedgerName).FirstOrDefault();
            var max = service.LastRow().OrderByDescending(a=>a.Id).FirstOrDefault().Id;
            cus.Id = max + 1;
            cus.OrderNo = cus.Id + 1;
            if (isExists != null)
            {
                return isExists;
            }
            service.Save(cus);
            return cus;

        }
        public AccountLedger Update(AccountLedger t, int id)
        {
            return service.Update(t, id);
        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }

        public string CheckDrOrCr(string nature)
        {
            if (nature == "Assets" || nature == "Expenses")
            {
                return "Dr";
            }
            else if (nature == "Liabilities" || nature == "Income")
            {
                return "Cr";
            }
            else
                return "";
        }

        public string CheckDrOrCr(int accountGroup)
        {
            var group = groupService.GetById(accountGroup);
            if (group.Nature == "Assets" || group.Nature == "Expenses")
            {
                return "Dr";
            }
            else if (group.Nature == "Liabilities" || group.Nature == "Income")
            {
                return "Cr";
            }
            else
                return "";
        }
    }
}