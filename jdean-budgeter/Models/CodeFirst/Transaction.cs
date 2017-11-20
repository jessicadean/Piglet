using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jdean_budgeter.Models.CodeFirst
{
    public class Transaction
    {
     

        public int Id { get; set; }
        public string Name { get; set; }
        public int? FrequencyId { get; set; }
        public bool Credit { get; set; }
        public decimal Amount { get; set; }
        public decimal? RecAmt { get; set; }
        public bool RecStatus { get; set; }
        public DateTime PmtDate { get; set; } 
        public int? CategoryId { get; set; }
        public int BankAccountId { get; set; }
        

        public virtual BankAccount BankAccount { get; set; }
        public virtual Household Household { get; set; }
      
        public virtual Category Category { get; set; }

       

    }
}