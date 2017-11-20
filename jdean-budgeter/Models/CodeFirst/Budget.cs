using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jdean_budgeter.Models.CodeFirst
{
    public class Budget
    {
        public Budget()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OpeningAmt { get; set; }
        public decimal Amount { get; set; }
        public bool Reconciled { get; set; }
        public int CategoryId { get; set; }
        public int? HouseholdId { get; set; }
        public int Frequency { get; set; }

        public virtual ICollection<Transaction>Transactions { get; set; }
        public virtual Category Category { get; set; }
        public virtual Household Household { get; set; }
    }
}