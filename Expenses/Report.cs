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
    
    public partial class Report
    {
        public Report()
        {
            this.Expenses = new HashSet<Expense>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public int CurrencyID { get; set; }
        public System.DateTime DateCreated { get; set; }
        public bool Submitted { get; set; }
        public Nullable<System.DateTime> DateSubmitted { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> DateApproved { get; set; }
        public bool Reimbursed { get; set; }
        public Nullable<System.DateTime> DateReimbursed { get; set; }
        public int StatusID { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual User User { get; set; }
        public virtual Status Status { get; set; }
    }
}
