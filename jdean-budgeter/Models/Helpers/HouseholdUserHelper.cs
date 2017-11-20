using jdean_budgeter.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jdean_budgeter.Models.Helpers
{
    public class HouseholdUserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


        public bool IsUserInHousehold(string userId, int householdId)
        {
            var household = db.Households.Find(householdId);
            var userBool = household.Users.Any(u => u.Id == userId); //checks for if is in THIS household, not ANY a household
            return userBool;
        }
        public void RemoveUserFromHousehold(string userId, int householdId)
        {
            var user = db.Users.Find(userId);
            var household = db.Households.Find(householdId);
            household.Users.Remove(user);
            db.SaveChanges();
        }
        public void AddUserToHousehold(string userId, int householdId)
        {
            var user = db.Users.Find(userId);
            var household = db.Households.Find(householdId);
            household.Users.Add(user);
            db.SaveChanges();
        }

        //public List<Household> ListUserProjects(string userId)
        //{
        //    var user = db.Users.Find(userId);
        //    return user.Projects.ToList();
        //}

        public List<ApplicationUser> ListUsersInHousehold(int householdId)
        {
            var household = db.Households.Find(householdId);
            return household.Users.ToList();
        }

        //public List<ApplicationUser> ListUsersNotOnProjects(int projectId)
        //{
        //    return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        //}
    }
}