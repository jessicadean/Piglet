using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace jdean_budgeter.Models.Helpers
{
    public static class Extensions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var HouseholdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdId");

            if (HouseholdClaim != null)
                return Int32.Parse(HouseholdClaim.Value);
            else
                return null;

        }

        public static string GetHouseholdName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var HouseholdNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdName");

            if (HouseholdNameClaim != null)
                return (HouseholdNameClaim.Value);

            else
                return null;
        }

        public static string GetFirstName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var FirstNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "FirstName");

                if (FirstNameClaim != null)
                return (FirstNameClaim.Value);

            else
                return null;
        }

        // attempted something from a tutorial, may not work
        //public static string GetFirstName(this ClaimsPrincipal principal)
        //{
        //    var firstName = principal.Claims.FirstOrDefault(c => c.Type == "FirstName");
        //    return firstName?.Value;
        //}

        //public static string GetLastName(this ClaimsPrincipal principal)
        //{
        //    var lastName = principal.Claims.FirstOrDefault(c => c.Type == "LastName");
        //    return lastName?.Value;
        //}
        // end tutorial attempt

        public static bool IsInHousehold(this IIdentity user)
        {
            var cUser = (ClaimsIdentity)user;
            var hHid = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return (hHid != null && !string.IsNullOrWhiteSpace(hHid.Value));
        }
    }
}