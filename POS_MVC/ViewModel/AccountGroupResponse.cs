using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.ViewModel
{
    public class AccountGroupResponse
    {

        public int Id { get; set; }
        public string AccountGroupName { get; set; }
        public Nullable<int> GroupUnder { get; set; }
        public string Under { get; set; }
        public string Narration { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public string Nature { get; set; }
        public string AffectGrossProfit { get; set; }
        public Nullable<System.DateTime> ExtraDate { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
    }
}