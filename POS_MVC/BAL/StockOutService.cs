using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RiceMill_MVC.Models;
using RiceMill_MVC.Util;
using System.Data.SqlClient;

namespace RiceMill_MVC.BAL
{
    public class StockOutService
    {
        DBService<StockOut> service = new DBService<StockOut>();
        DBService<Inventory> inventory = new DBService<Inventory>();

        public List<StockOut> GetAll()
        {
            return service.GetAll();
        }
        public List<string> GetAllStockChallan()
        {
            DateTime fromDate = DateTime.Now.AddMonths(-1);
            return service.GetAll(a=>a.ProductionDate>fromDate).OrderBy(a=>a.ProductionDate).Select(a=>a.InvoiceNo).Distinct().ToList();
            //List<SqlParameter> parameters = new List<SqlParameter>();
            //SqlParameter param1 = new SqlParameter("@fromDate", DateTime.Now.AddMonths(-1));
            //parameters.Add(param1);
            //SqlParameter param2 = new SqlParameter("@toDate", DateTime.Now);
            //parameters.Add(param2);
            //var get = service.ExecuteProcedure("Exec StockOutChallanList @fromDate,@toDate", parameters);
            //return get;
        }


        public StockOut GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public StockOut Save(StockOut stockOut)
        {
            var existingItem = inventory.GetAll(a => a.ProductId == stockOut.ProductId &&  a.QtyInBale==stockOut.BaleWeight && a.IsActive == true && a.WarehouseId == stockOut.WarehouseId && a.SupplierId == stockOut.SupplierId).ToList();
            if (existingItem.Count > 0)
            {
                foreach (var inv in existingItem)
                {
                    inv.UpdatedDate = DateTime.Now;
                    inv.UpdatedBy = CurrentSession.GetCurrentSession().UserName;
                    inv.ProductionQty = stockOut.BaleQty;
                    inv.BalanceQty = inv.BalanceQty - stockOut.BaleQty??0;
                    inventory.Update(inv, inv.Id);
                }

            }
            return service.Save(stockOut);
        }

        public StockOut Update(StockOut t, int id)
        {
            return service.Update(t, id);

        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }
    }
}