using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jdean_budgeter.Models.CodeFirst
{
    public class BankAccount
    {
        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Opened { get; set; }
        public int? HouseholdId { get; set; }
        public int Type { get; set; }
        public decimal Balance { get; set; }
        
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual Household Household { get; set; }

    }
}