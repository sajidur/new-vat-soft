using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using REX_MVC.Models;
using REX_MVC.Util;

namespace REX_MVC.BAL
{
    public class StockInService
    {
        DBService<StockIn> service = new DBService<StockIn>();
        DBService<Inventory> inventory = new DBService<Inventory>();
        ProductService productService = new ProductService();
        public List<StockIn> GetAll()
        {
            return service.GetAll();
        }

        public StockIn GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public StockIn Save(StockIn stockIn, string Notes)
        {
            var goodsType = 1;
            var product = productService.GetById(stockIn.ProductId);
            if (product != null)
            {
                goodsType = product.ProductTypeId ?? 1;
            }
            var existingItem = inventory.GetAll(a => a.ProductId == stockIn.ProductId && a.IsActive == true && a.WarehouseId == stockIn.WarehouseId).ToList();
            if (existingItem.Count > 0)
            {
                foreach (var inv in existingItem)
                {
                    inv.UpdatedDate = DateTime.Now;
                    inv.UpdatedBy = "";
                    inv.BalanceQty = inv.BalanceQty + stockIn.BaleQty??0;
                    inv.ReceiveQty = inv.ReceiveQty ?? 0 + stockIn.BaleQty ?? 0;
                    inv.GoodsType = goodsType.ToString();
                    inventory.Update(inv, inv.Id);
                }

            }
            else
            {
                Inventory result = new Inventory();
                Inventory FinalResult = new Inventory();
                result.ProductId = (int)stockIn.ProductId;
                result.QtyInBale = (int?)stockIn.BaleWeight;
                result.SupplierId = stockIn.SupplierId??0;
                result.WarehouseId = (int)stockIn.WarehouseId;
                result.ReceiveQty = stockIn.BaleQty;
                result.BalanceQty = stockIn.BaleQty??0;
                result.Notes = Notes;
                result.GoodsType =goodsType.ToString();
                result.CreatedBy = CurrentSession.GetCurrentSession().UserName;
                result.CreatedDate = DateTime.Now;
                result.IsActive = true;
                FinalResult = inventory.Save(result);

            }
            return service.Save(stockIn);
        }


        public StockIn Update(StockIn t, int id)
        {
            return service.Update(t, id);

        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }
    }
}