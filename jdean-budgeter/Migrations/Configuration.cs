namespace jdean_budgeter.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using jdean_budgeter.Models.CodeFirst;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<jdean_budgeter.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(jdean_budgeter.Models.ApplicationDbContext context)
        {
            //TransactionTypes
           if(!context.TransactionTypes.Any(t => t.Name =="Debit"))
            {
                var type = new TransactionType();
                type.Name = "Debit";
                context.TransactionTypes.Add(type);
            }

           if(!context.TransactionTypes.Any(t => t.Name =="Credit"))
            {
                var type = new TransactionType();
                type.Name = "Credit";
                context.TransactionTypes.Add(type);
            }

           //Frequency
           if(!context.Frequencies.Any(f => f.Name =="Weekly"))
            {
                var frequency = new Frequency();
                frequency.Name = "Weekly";
                context.Frequencies.Add(frequency);
            }

            if (!context.Frequencies.Any(f => f.Name == "Bi-Weekly"))
            {
                var frequency = new Frequency();
                frequency.Name = "Bi-Weekly";
                context.Frequencies.Add(frequency);
            }

            if (!context.Frequencies.Any(f => f.Name == "Monthly"))
            {
                var frequency = new Frequency();
                frequency.Name = "Monthly";
                context.Frequencies.Add(frequency);
            }

            if (!context.Frequencies.Any(f => f.Name == "Annual"))
            {
                var frequency = new Frequency();
                frequency.Name = "Annual";
                context.Frequencies.Add(frequency);
            }

            //CategoryTypes
            if (!context.CategoryTypes.Any(c => c.Name == "Expense"))
            {
                var type = new CategoryType();
                type.Name = "Expense";
                context.CategoryTypes.Add(type);
            }
            if (!context.CategoryTypes.Any(c => c.Name == "Income"))
            {
                var type = new CategoryType();
                type.Name = "Income";
                context.CategoryTypes.Add(type);
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
