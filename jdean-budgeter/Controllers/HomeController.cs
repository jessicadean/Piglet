using jdean_budgeter.Models;
using jdean_budgeter.Models.CodeFirst;
using jdean_budgeter.Models.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace jdean_budgeter.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            
            var user = db.Users.Find(User.Identity.GetUserId());
            var myAccounts = db.BankAccounts.Where(b => b.Household.Users.Any(h => h.Id == user.Id));
            decimal expAmt = 0;
            decimal incAmt = 0;

            foreach(var exp in myAccounts.Select(t => t.Transactions).ToList())
            {
                expAmt += exp.Where(e => e.Credit == false && e.PmtDate.Month == DateTime.Today.Month).Sum(e => e.Amount);
            }
            foreach (var inc in myAccounts.Select(t => t.Transactions).ToList())
            {
                incAmt += inc.Where(e => e.Credit == true && e.PmtDate.Month == DateTime.Today.Month).Sum(e => e.Amount);
            }

            ViewBag.Expenses = expAmt;
            ViewBag.Incomes = incAmt;
            //ViewBag.Incomes = db.Transactions.Where(t => t.Credit == true).Sum(t => t.Amount);

            return View(myAccounts.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateJoinHousehold()
        {
            //implementation for creating or joining Hhold
            return View();
        }

        public PartialViewResult _LeaveHouseHold()
        {
            return PartialView();
        }

        //[AuthorizeHouseholdRequired]
        //public async Task<ActionResult> LeaveHousehold(string userId, int? Id)
        //{
        //    var user = db.Users.Find(User.Identity.GetUserId());
        //    user.HouseholdId = null;
        //    await ControllerContext.HttpContext.RefreshAuthentication(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Leavehousehold(string userId, int? Id)
        {var user = db.Users.Find(User.Identity.GetUserId());
            userId = user.Id;
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Household household = user.Household;
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        [AuthorizeHouseholdRequired]
        [HttpPost, ActionName("LeaveHousehold")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LeaveHouseholdConfirm(string userId, int? Id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseholdId = null;
            await ControllerContext.HttpContext.RefreshAuthentication(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        

        public ActionResult TestChart()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var house = db.Households.Find(user.HouseholdId);

            decimal TotalExpenses = db.Expenses.Sum(e => e.Amount);

            List<Expense> expenses = new List<Expense>();
            decimal total = expenses.Sum(item => item.Amount);
            //ViewBag.HouseHoldExpenses = house.Expenses.Sum();
            ViewBag.HouseholdIncomes = 8;
            //ViewBag.ResolvedTk = 7;

            return View();
        }

        //public ActionResult StaticData()
        //{
        //    object[] dataArray = new object[12];
        //    //var month = DateTime.Today.Month;
        //    var randomNum = new Random();
        //    for (var i = 0; i <= 11; i++)
        //    {
        //        dataArray[i] = new { Month = i + 1, Income = randomNum.Next(1000, 10000), Expense = randomNum.Next(1000, 10000) };
        //    }

        //    return Content(JsonConvert.SerializeObject(dataArray), "application/json");
        //}foreach(var account in bankAccounts)
//            {
//                ChartItemList list = new ChartItemList();
//        list.Expense = Convert.ToInt32(account.Transactions.Where(t => t.TransactionTypeId == 2).Sum(t => t.Amount));
//                list.Income = Convert.ToInt32(account.Transactions.Where(t => t.TransactionTypeId == 1).Sum(t => t.Amount));
//                list.Name = account.AccountName;
//                amtList.Add(list);
//            }
//    int counter = 1;
//    object[] dataArray = new object[amtList.Count];
//            foreach (var account in amtList)
//            {
//                dataArray[counter - 1] = new { Month = account.Name, Income = account.Income, Expense = account.Expense
//};
//counter++;
//            };

        public ActionResult StaticData()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var expenses = db.Transactions.Where(t => t.Household.Id == user.HouseholdId);
            List<ChartItem> chartList = new List<ChartItem>();
            foreach (var item in expenses)
            {

                ChartItem list = new ChartItem();
                list.Expense = Convert.ToInt32(db.Transactions.Where(t => t.PmtDate.Month == DateTime.Now.Month));
                chartList.Add(list);
            }

            int counter = 1;

            object[] dataArray = new object[chartList.Count];
            //var month = DateTime.Today.Month;
            // var randomNum = new Random();


            foreach (var item in chartList)
            {
                dataArray[counter -1] = new { Income = item.Expense};
            }


            return Content(JsonConvert.SerializeObject(dataArray), "application/json");
        }

        [HttpGet]
        public ActionResult GetChart()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var house = db.Households.Find(user.HouseholdId);
            var tod = DateTimeOffset.Now;
            decimal totalExpense = 0;
            decimal totalBudget = 0;
            var totalAcc = (from a in house.BankAccounts
                            select a.Balance).DefaultIfEmpty().Sum();

            var donut = (from c in house.Categories
                         where c.CategoryType.Name == "Expense"
                         let aSum = (from t in c.Transactions
                                     where t.PmtDate.Year == tod.Year && t.PmtDate.Month == tod.Month
                                     select t.Amount).DefaultIfEmpty().Sum()
                         select new
                         {
                             label = c.Name,
                             value = aSum
                         }).ToArray();
            var result = new
            {
                totalAcc = totalAcc,
                totalBudget = totalBudget,
                totalExpense = totalExpense,
                donut = donut
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");


        }
    }
}