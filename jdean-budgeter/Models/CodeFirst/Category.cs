using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jdean_budgeter.Models.CodeFirst
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public int? Hhid { get; set; }
     
        public virtual CategoryType CategoryType { get; set; }
       
        public virtual ICollection <Transaction> Transactions { get; set; }
        public virtual ICollection <Budget> Budgets { get; set; }
    }
}