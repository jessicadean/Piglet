using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jdean_budgeter.Models.CodeFirst
{
    public class Household
    {
        public Household()
        {
            Users = new HashSet<ApplicationUser>();
            BankAccounts = new HashSet<BankAccount>();
            Expenses = new HashSet<Expense>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string HeadofHholdId { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}