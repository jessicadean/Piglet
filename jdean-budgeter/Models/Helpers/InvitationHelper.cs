using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using jdean_budgeter.Models.CodeFirst;
using static jdean_budgeter.EmailService;
//using System.ComponentModel.DataAnnotations;

namespace jdean_budgeter.Models.Helpers
{
    public class Invitation
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsNew { get; set; }
        public DateTimeOffset Sent { get; set; }
        public string RecipientId { get; set; }
        public int HouseholdId { get; set; } 

        public virtual ApplicationUser Recipient { get; set; }
    }

    


    public class InvitationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Invitation invitation = new Invitation();



        public void Invite(string userId, string subject, string message, bool sendEmail)
        {
            var inviter = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var invitation = new Invitation
            {
                Subject = subject,
                Message = message,
                IsNew = true,
                RecipientId = userId,
                Sent = DateTimeOffset.UtcNow,
                //HouseholdId = inviter.HouseholdId
            };

            db.Invitations.Add(invitation);
            db.SaveChanges();

            if (sendEmail)
            {
                try
                {
                    var user = db.Users.Find(userId);

                    var from = "PigletApp<PigletAppDONOTREPLY@email.com>";
                    var email = new MailMessage(from, user.Email)
                    {
                        Subject = subject,
                        Body = message,
                    };

                    var svc = new PersonalEmail();
                    svc.Send(email);
                }
                catch (Exception ex)
                {
                }
            }
        }

       
            
        
    }
}