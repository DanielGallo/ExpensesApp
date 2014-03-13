//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Expenses
{
    using System;
    using System.Collections.Generic;
    
    public partial class Expense
    {
        public int ID { get; set; }
        public int ReportID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public string Merchant { get; set; }
        public System.DateTime Date { get; set; }
        public string Receipt { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Report Report { get; set; }
    }
}