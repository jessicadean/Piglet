﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using static jdean_budgeter.EmailService;

namespace jdean_budgeter.Models.CodeFirst
{
    public class Notification
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsNew { get; set; }
        public DateTimeOffset Sent { get; set; }
        public string RecipientId { get; set; }

        public virtual ApplicationUser Recipient { get; set; }
    }

    public class NotificationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Notification Notification = new Notification();



        public void Notify(string userId, string subject, string message, bool sendEmail)
        {
            var notification = new Notification
            {
                Subject = subject,
                Message = message,
                IsNew = true,
                RecipientId = userId,
                Sent = DateTimeOffset.UtcNow,
            };

            db.Notifications.Add(notification);
            db.SaveChanges();

            if (sendEmail)
            {
                try
                {
                    var user = db.Users.Find(userId);

                    var from = "PigletApp<PigletAppDONOTREPLY@email>";
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

        public void Notify(int BankAccount, string userId, string subject, string message, bool sendEmail)
        {
            var notification = new Notification
            {
                Subject = subject,
                Message = message,
                IsNew = true,
                RecipientId = userId,
                Sent = DateTimeOffset.UtcNow,
            };

            db.Notifications.Add(notification);
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