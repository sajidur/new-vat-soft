﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.ViewModel
{
    public class SalesOrderResponse
    {
        public SalesOrderResponse()
        {
            this.Amount = this.BaleQty * this.Rate;
            this.Customer = new CustomerResponse();
            this.Product = new ProductResponse();
        }

        public int Id { get; set; }
        public string SalesOrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string OrderRecieveBy { get; set; }
        public int CustomerID { get; set; }
        public int ProductId { get; set; }
        public int BaleQty { get; set; }
        public decimal BaleWeight { get; set; }
        public decimal TotalQtyInKG { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get ; set; }
        public System.DateTime DeliveryDate { get; set; }
        public string PostDate { get { return DeliveryDate.ToString("dd-MM-yyyy"); } set { DeliveryDate.ToString("dd-MM-yyyy"); } }

        public Nullable<decimal> DeliveryQty { get; set; }
        public string TransportType { get; set; }
        public string TransportNo { get; set; }
        public Nullable<System.DateTime> PricingDate { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public bool IsActive { get; set; }


        public CustomerResponse Customer { get; set; }
        public ProductResponse Product { get; set; }
    }
}