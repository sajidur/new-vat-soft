//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RiceMill_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SalesDelivery
    {
        public int Id { get; set; }
        public string SalesInvoice { get; set; }
        public Nullable<int> SalesMasterId { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNo { get; set; }
        public int CustomerID { get; set; }
        public Nullable<decimal> AdditionalCost { get; set; }
        public string TransportType { get; set; }
        public string TransportNo { get; set; }
        public string ExpenseBy { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual SalesMaster SalesMaster { get; set; }
    }
}
