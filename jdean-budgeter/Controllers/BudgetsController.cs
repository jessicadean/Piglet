using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jdean_budgeter.Models;
using Microsoft.AspNet.Identity;

namespace jdean_budgeter.Models.CodeFirst
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            DateTime dateTime = DateTime.Now;
            //Compare only date parts
            
            db.Transactions.FirstOrDefault(r =>
                            DbFunctions.TruncateTime(r.PmtDate) == DbFunctions.TruncateTime(dateTime));
            //need to restrict list of transactions to only those in this hhold
            var user = db.Users.Find(User.Identity.GetUserId());
            var myBudgets = db.Budgets.Where(b => b.Household.Users.Any(h => h.Id == user.Id));
            decimal mySpend = 0;

            foreach (var exp in myBudgets.Select(t => t.Transactions).ToList())
            {
                mySpend += exp.Where(e => e.Credit == false && e.PmtDate.Month == DateTime.Today.Month).Sum(e => e.Amount);
            }

          //  var monthlyBudget = db.Transactions.Where(t => t.PmtDate.Month == DateTime.Today.Month).ToList();
            
            //foreach (var trans in monthlyBudget)
            //{
            //    budgetSpent += trans.Amount;
            //}

            ViewBag.Spent = mySpend;

            return View(myBudgets);
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id, Transaction transaction)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }

            var spentVar = budget.Amount / budget.OpeningAmt;
            var percentSpent = Math.Round(spentVar);
            ViewBag.PercentSpent = percentSpent;

            var budgetSpent = budget.OpeningAmt - budget.Amount;
            ViewBag.BudgetSpent = budgetSpent;
            List<Transaction> transactions = new List<Transaction>();
           var transList = db.Transactions.Where(t => t.CategoryId == budget.CategoryId).ToList();

            var budgetTransList = transList.Where(e => e.Credit == false && e.PmtDate.Month == DateTime.Today.Month).ToList();
           
            if (budget.HouseholdId == user.HouseholdId)
            { 
            return View(budget);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,OpeningAmt,Amount,HouseholdId,CategoryId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                budget.HouseholdId = user.HouseholdId;
                budget.Amount = budget.OpeningAmt;
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            if (budget.HouseholdId == user.HouseholdId)
            { 
            return View(budget);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,OpeningAmt,Amount,Reconciled,CategoryId,HouseholdId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
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
