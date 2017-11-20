using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jdean_budgeter.Models.CodeFirst
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int? Frequency { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int HouseholdId { get; set; }
        public int? TransactionId { get; set; }
        public int BudgetId { get; set; }
        

        public virtual Budget Budget { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual Household Household { get; set; }
    }
}