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
    
    public partial class Tax
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public string TypeOfDiscount { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
