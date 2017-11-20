using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jdean_budgeter.Models;
using jdean_budgeter.Models.CodeFirst;
using jdean_budgeter.Models.Helpers;
using Microsoft.AspNet.Identity;

namespace jdean_budgeter.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            return View(db.Transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }
        //GET: Transactions/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.BankAccountId = new SelectList(db.BankAccounts.Where(b => b.HouseholdId == user.HouseholdId), "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ExpenseId,Amount,RecAmt,RecStatus,Credit,PmtDate,CategoryId,BankAccountId")]
        Transaction transaction)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            BankAccount bankAccount = db.BankAccounts.Find(transaction.BankAccountId);
            var budgets = db.Budgets.Where(b => b.HouseholdId == user.HouseholdId);

            if (ModelState.IsValid)

            {
                // transaction.BankAccountId = 
                db.Transactions.Add(transaction);
                db.SaveChanges();

                //var transBudget = db.Budgets.Where(b => b.CategoryId == transaction.CategoryId);
                //if (transaction.CategoryId != null)

                var tBudget = db.Budgets.Where(b => b.Category.Id == transaction.Category.Id);



                //CalculationHelper calcHelper = new CalculationHelper();
                if (transaction.Credit == false)
                {

                    decimal transAmt = transaction.Amount * -1;
                    bankAccount.Balance = bankAccount.Balance + transAmt;
                    //transaction.BudgetId = db.Budgets.Where(b => b.CategoryId == transaction.CategoryId);


                    //transaction.Budget.Amount = transaction.Budget.Amount + transAmt;
                    //budget.Amount = budget.Amount + transAmt;
                    foreach (var item in budgets.Where(b => b.CategoryId == transaction.CategoryId))
                    {
                        item.Amount = item.Amount + transAmt;
                    }
                }
                else
                {
                    bankAccount.Balance = bankAccount.Balance + transaction.Amount;
                }


                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();

                NotificationHelper helper = new NotificationHelper();
                if (bankAccount.Balance < 100)
                {
                    helper.Notify(bankAccount.Household.HeadofHholdId, "PigletApp Notification", "Low balance alert on your account. "
                            + bankAccount.Name, true);
                }

                ViewBag.BankAccountId = new SelectList(db.BankAccounts.Where(b => b.HouseholdId == user.HouseholdId));
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
                return RedirectToAction("Details", "BankAccounts", new { id = bankAccount.Id });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            return RedirectToAction("Details", "BankAccounts", new { id = bankAccount.Id });
        }

        // GET: Transactions/Create
        // public ActionResult Create()
        //{
        //  return View();
        //}

        // POST: Transactions/QuickCreate: categorize later 
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuickCreate([Bind(Include = "Id,Name,BudgetId,ExpenseId,Amount,RecAmt,RecStatus,Credit,PmtDate,CategoryId,BankAccountId")]
        Transaction transaction)
        {
            BankAccount bankAccount = db.BankAccounts.Find(transaction.BankAccountId);
            
            //match the transaction's category ID to the category ID of the budget
            //find the id of the budget that is connected to the transaction by querying the category's id
            //budget has a categoryid, so does the transaction
            //find the proper budget by querying the category id that they have in common
            //transaction.BudgetId = transaction.Category.Id;

            //var budget = transaction.Budget.Id;

            if (ModelState.IsValid)

            {

                db.Transactions.Add(transaction);
                db.SaveChanges();

                //  var transBudget = db.Budgets.Where(b => b.CategoryId == transaction.CategoryId);
                //if (transaction.CategoryId != null)

                //CalculationHelper calcHelper = new CalculationHelper();
                if (transaction.Credit == false)
                {
                    decimal transAmt = transaction.Amount * -1;
                    bankAccount.Balance = bankAccount.Balance + transAmt;

                }
                else
                {
                    bankAccount.Balance = bankAccount.Balance + transaction.Amount;
                }

                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "BankAccounts", new { id = bankAccount.Id });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            return RedirectToAction("Details", "BankAccounts", new { id = bankAccount.Id });
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ExpenseId,Amount,RecAmt,RecStatus,BankAccountId,PmtDate,CategoryId")] Transaction transaction)
        {

            BankAccount bankAccount = db.BankAccounts.Find(transaction.BankAccountId);
            // Budget budget = db.Budgets.Find(transaction.BudgetId);
            var user = db.Users.Find(User.Identity.GetUserId());

            var budgets = db.Budgets.Where(b => b.HouseholdId == user.HouseholdId);

            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
                var oldTrans = db.Transactions.AsNoTracking().First(t => t.Id == transaction.Id);



                if (oldTrans.Amount != transaction.Amount)
                {
                    if (transaction.Credit == false)
                    {

                        decimal transAmt = transaction.Amount * -1;
                        bankAccount.Balance = bankAccount.Balance + transAmt;
                        //transaction.BudgetId = db.Budgets.Where(b => b.CategoryId == transaction.CategoryId);


                        //transaction.Budget.Amount = transaction.Budget.Amount + transAmt;
                        //budget.Amount = budget.Amount + transAmt;
                        foreach (var item in budgets.Where(b => b.CategoryId == transaction.CategoryId))
                        {
                            item.Amount = item.Amount + transAmt;
                        }
                    }
                    else
                    {
                        bankAccount.Balance = bankAccount.Balance + transaction.Amount;
                    }


                    db.Entry(bankAccount).State = EntityState.Modified;
                    db.SaveChanges();

                    NotificationHelper helper = new NotificationHelper();
                    if (bankAccount.Balance < 100)
                    {
                        helper.Notify(bankAccount.Household.HeadofHholdId, "PigletApp Notification", "Low balance alert on your account. "
                                + bankAccount.Name, true);
                    }

                    return RedirectToAction("Details", "BankAccounts", new { id = bankAccount.Id });
                }
                return View(transaction);
            }
            return RedirectToAction("Details", "BankAccounts", new { id = bankAccount.Id });
        }

            // GET: Transactions/Delete/5
            public ActionResult Delete(int? id)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Transaction transaction = db.Transactions.Find(id);
                if (transaction == null)
                {
                    return HttpNotFound();
                }
                if (transaction.Household.Id == user.HouseholdId)
                {
                    return View(transaction);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Transaction transaction = db.Transactions.Find(id);
            BankAccount bankAccount = db.BankAccounts.Find(transaction.BankAccountId);
            var budgets = db.Budgets.Where(b => b.HouseholdId == user.HouseholdId);

            db.Transactions.Remove(transaction);
            db.SaveChanges();
            if (transaction.Credit == false)
            {

                decimal transAmt = transaction.Amount * -1;
                bankAccount.Balance = bankAccount.Balance + transAmt;
                //transaction.BudgetId = db.Budgets.Where(b => b.CategoryId == transaction.CategoryId);


                //transaction.Budget.Amount = transaction.Budget.Amount + transAmt;
                //budget.Amount = budget.Amount + transAmt;
                foreach (var item in budgets.Where(b => b.CategoryId == transaction.CategoryId))
                {
                    item.Amount = item.Amount + transAmt;
                }
            }
            else
            {
                bankAccount.Balance = bankAccount.Balance + transaction.Amount;
            }


            db.Entry(bankAccount).State = EntityState.Modified;
            db.SaveChanges();

            NotificationHelper helper = new NotificationHelper();
            if (bankAccount.Balance < 100)
            {
                helper.Notify(bankAccount.Household.HeadofHholdId, "PigletApp Notification", "Low balance alert on your account. "
                        + bankAccount.Name, true);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
