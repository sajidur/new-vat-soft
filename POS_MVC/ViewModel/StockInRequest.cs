using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.ViewModel
{
    public class StockInRequest
    {
        public int Qty { get; set; }
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public string WarehouseName { get; set; }
        public int SupplierId { get; set; }
    }
}