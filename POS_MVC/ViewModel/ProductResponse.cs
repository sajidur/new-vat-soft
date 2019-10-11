using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using REX_MVC.Models;
using System.Web.Script.Serialization;

namespace REX_MVC.ViewModel
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string ProductNameBangla { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public bool IsActive { get; set; }

        public Nullable<decimal> TaxRate { get; set; }
        public Nullable<decimal> SDRate { get; set; }
        public string ProductCategory { get; set; }
        public string HSCode { get; set; }

        public CategoryResponse Category { get; set; }
        //[ScriptIgnore(ApplyToOverrides = true)]
        //public ICollection<InventoryResponse> Inventories { get; set; }
        //public ICollection<ReceiveDetailResponse> ReceiveDetails { get; set; }
        //public ICollection<SalesDetailResponse> SalesDetails { get; set; }
        //public ICollection<SalesOrderResponse> SalesOrders { get; set; }
        //public ICollection<StockInResponse> StockIns { get; set; }
        //public ICollection<StockOutResponse> StockOuts { get; set; }
    }
}