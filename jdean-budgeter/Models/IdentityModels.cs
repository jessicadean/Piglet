using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using jdean_budgeter.Models.CodeFirst;
using jdean_budgeter.Models.Helpers;

namespace jdean_budgeter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int? HouseholdId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }

        public virtual Household Household { get; set; }
      
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }

        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            
            userIdentity.AddClaim(new Claim("HouseholdId", HouseholdId.ToString()));

            if (Household != null)
            {
                userIdentity.AddClaim(new Claim("HouseholdName", Household.Name));
            }

            userIdentity.AddClaim(new Claim("FirstName", FirstName));

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<jdean_budgeter.Models.CodeFirst.Expense> Expenses { get; set; }
    }
}