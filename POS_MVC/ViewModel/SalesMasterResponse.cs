using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using REX_MVC.Models;


namespace REX_MVC.ViewModel
{
    public class SalesMasterResponse
    {
        public SalesMasterResponse()
        {
            CreatedDate = DateTime.Now;
            PricingDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string SalesInvoice { get; set; }
        public int SalesOrderId { get; set; }
        public DateTime SalesDate { get; set; }
        public string PostDate
        {
            get
            {
                return this.SalesDate.ToString("dd-MMM-yyyy");
            }
            set
            {
                this.SalesDate.ToString("dd-MMM-yyyy");
            }
        }
        public string SalesBy { get; set; }
        public int CustomerID { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GrandTotal { get; set; }

        public string TransportType { get; set; }
        public string TransportNo { get; set; }
        public DateTime PricingDate { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public string DriverName { get; set; }
        public Nullable<decimal> RentAmount { get; set; }


        public ICollection<SalesDetailResponse> SalesDetails { get; set; }

        //[ScriptIgnore(ApplyToOverrides = true)]
        public CustomerResponse Customer { get; set; }

    }
}