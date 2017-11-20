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
using System.Threading.Tasks;

namespace jdean_budgeter.Controllers
{
    public class HouseholdsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            return View(db.Households.Where(h =>h.Id == user.HouseholdId).ToList());
        }

        // GET: Households/Details/5
        [AuthorizeHouseholdRequired]
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            if (household.Id == user.HouseholdId)
            {
                return View(household);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,HeadofHholdId")] Household household)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                
                db.Households.Add(household);
                user.HouseholdId = household.Id;

                household.HeadofHholdId = user.FullName;
                db.SaveChanges();
                //return RedirectToAction("Index");
                await HttpContext.RefreshAuthentication(user);
                return RedirectToAction("Index", "Home");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        [AuthorizeHouseholdRequired]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HeadofHholdId")] Household household)
        {
            if (ModelState.IsValid)
            {
                
                InvitationHelper invitationHelper = new InvitationHelper();
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        [AuthorizeHouseholdRequired]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //POST: Households/Join
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JoinHousehold(string userId, int householdId)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            HouseholdUserHelper householdHelper = new HouseholdUserHelper();
            if (householdHelper.IsUserInHousehold(user.Id, householdId) == true)
            {
                return RedirectToAction("Join", "Households");
            }
            var household = db.Households.Find(householdId);
            household.Users.Add(user);
            await HttpContext.RefreshAuthentication(user);
            return View(household);
            
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
