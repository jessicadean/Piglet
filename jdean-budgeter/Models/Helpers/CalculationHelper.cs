using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jdean_budgeter.Models.CodeFirst;
using System.Web.Mvc;
using System.Data.Entity;

namespace jdean_budgeter.Models.Helpers
{
    public class CalculationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void UpdateBalance(decimal Balance, int id)
        {
            var bankAccount = db.BankAccounts.Find(id);
            decimal balance = bankAccount.Balance;
            balance += db.Transactions.Sum(t => t.Amount);
            db.Entry(bankAccount).State = EntityState.Modified;
            db.SaveChanges();
        }

    }
}

//public void UpdateBalance()
//{
//    decimal Balance = 0;
//    Balance += db.Transactions.Sum(t => t.Amount);

//}
