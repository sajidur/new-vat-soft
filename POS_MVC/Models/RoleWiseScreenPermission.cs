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
    
    public partial class RoleWiseScreenPermission
    {
        public string RoleId { get; set; }
        public string ScreenId { get; set; }
        public int CompanyId { get; set; }
        public Nullable<System.DateTime> SetDate { get; set; }
        public string UserName { get; set; }
    
        public virtual Screen Screen { get; set; }
    }
}
