using System;
using System.Collections.Generic;
using Jugnoon.Settings;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Jugnoon.Models;
/// <summary>
/// Business Layer :  For processing user membership / subscription
/// </summary>
namespace Jugnoon.BLL
{
    public class UserPackagesBLL
    {
        public static bool Check_Package_Feature()
        {
            if (Configs.PremiumSettings.enable_payment)
                return true; // no packages on by administrator, lets user allowed freely to upload contents.
            else
                return false;
        }

        public static bool Check_membership(ApplicationDbContext context, string username)
        {
            bool flag = false;
            /*var _lst = Fetch_membership_Expiry_Information(context, username).Result;
            if (_lst.Count > 0)
            {
                if (_lst[0].islifetimerenewal == 1)
                {
                    flag = true;
                }
                else
                {
                    if (_lst[0].membership_expiry < DateTime.Now)
                    {
                        // UserBLLhip expired
                        int type = UserBLL.Get_MemberType(context, username);
                        if (type == 2)
                            UserBLL.Update_Field_V3(context, username, "type", (byte)0);
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }*/
            return flag;
        }
        // load user payment history
        public static Task<List<JGN_User_Payments>> Load_User_Payment_History(ApplicationDbContext context, string userid, bool isall, int records)
        {
            var Query = context.JGN_User_Payments
            .Join(context.JGN_Packages,
             payment => payment.package_id,
             package => package.id,
             (payment, package) => new { payment, package });

            Query = Query.Where(p => p.payment.userid == userid && p.payment.isenabled == 1)
            .OrderByDescending(p => p.payment.created_at);

            if (!isall)
                Query = Query.Take(records);

            return Query.Select(p => new JGN_User_Payments()
            {
                id = p.payment.id,
                package_id = p.payment.package_id,
                price = p.payment.price,
                credits = (int)p.payment.credits,
                created_at = (DateTime)p.payment.created_at,
                packages = new JGN_Packages()
                {
                    name = p.package.name,
                    currency = p.package.currency,
                    months = p.package.months
                }
             })
            .ToListAsync();
        }

        public static Task<List<ApplicationUser>> Fetch_membership_Expiry_Information(ApplicationDbContext context, string username)
        {  
            return context.AspNetusers
                 .Where(p => p.UserName == username)
                 .Select(p => new ApplicationUser()
                 {
                     //membership_expiry = (DateTime)p.membership_expiry,
                     //islifetimerenewal = (byte)p.islifetimerenewal,
                     type = p.type
                 }).ToListAsync();
        }
        // Status
        // 0: Pending
        // 1: Completed

        // Add User Payment History
        /* public static long Process(ApplicationDbContext context, UserPaymentEntity entity)
         {
             long PaymentId = 0;
             // var context = SiteConfig.dbContext;


                 var _entity = new user_payments()
                 {
                     username = entity.username,
                     id = (byte)entity.packageid,
                     price = (float)entity.price,
                     credits = entity.credits,
                     created_at = DateTime.Now,
                     months = entity.months,
                     status = (byte)entity.status,
                     transactionid = entity.transactionid,
                     payer_email = entity.payer_email,
                     item_name = entity.item_name,
                     payment_status = entity.payment_status,
                     pending_reason = entity.pending_reason,
                     payment_fee = entity.payment_fee,
                     payment_gross = entity.payment_gross,
                     txn_type = entity.txn_type,
                     first_name = entity.first_name,
                     last_name = entity.last_name,
                     address_street = entity.address_street,
                     address_city = entity.address_city,
                     address_state = entity.address_state,
                     address_zip = entity.address_zip,
                     address_country = entity.address_country,
                     address_status = entity.status.ToString(),
                     payer_id = entity.payer_id,
                     payment_type = entity.payment_type
                 };

                 context.Entry(_entity).State = EntityState.Added;

                context.SaveChanges();

                 PaymentId = _entity.paymentid;

             return PaymentId;
         }

        // Update User Payment Transaction
        public static void Update(ApplicationDbContext context,UserPaymentEntity entity)
        {
                var item = context.user_payments
                     .Where(p => p.paymentid == entity.paymentid)
                     .FirstOrDefault<user_payments>();

                if(item != null)
                {
                    item.username = entity.username;
                    item.id = (byte)entity.packageid;
                    item.price = (float)entity.price;
                    item.credits = entity.credits;
                    item.created_at = DateTime.Now;
                    item.months = entity.months;
                    item.status = (byte)entity.status;
                    item.transactionid = entity.transactionid;
                    item.payer_email = entity.payer_email;
                    item.item_name = entity.item_name;
                    item.payment_status = entity.payment_status;
                    item.pending_reason = entity.pending_reason;
                    item.payment_fee = entity.payment_fee;
                    item.payment_gross = entity.payment_gross;
                    item.txn_type = entity.txn_type;
                    item.first_name = entity.first_name;
                    item.last_name = entity.last_name;
                    item.address_street = entity.address_street;
                    item.address_city = entity.address_city;
                    item.address_state = entity.address_state;
                    item.address_zip = entity.address_zip;
                    item.address_country = entity.address_country;
                    item.address_status = entity.status.ToString();
                    item.payer_id = entity.payer_id;
                    item.payment_type = entity.payment_type;

                    context.SaveChanges();
                }

        }

        // Check for user pending transaction - if exist don't create new, just update information according to newly slected package
        public static bool Check_Pending_Transaction(ApplicationDbContext context, string username)
        {
            bool flag = false;
            // var context = SiteConfig.dbContext;
           
            
                if (context.user_payments.Where(p => p.username == username && p.status == 0).Count() > 0)
                    flag = true;
            

            return flag;
        }

        // Update pending transaction info with newly selected package
        public static long Update_Pending_Transaction_Info(ApplicationDbContext context, UserPaymentEntity payment)
        {
            long PaymentID = 0;
            var item = context.user_payments
                    .Where(p => p.username == payment.username && p.status==0)
                    .FirstOrDefault<user_payments>();

            if(item != null)
            {
                PaymentID = item.paymentid;
                item.id = (byte)payment.packageid;
                item.price = (float)payment.price;
                item.credits = payment.credits;
                item.created_at = DateTime.Now;
                item.months = payment.months;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
            return PaymentID;
        }

        // Check whether transaction id already exist
        public static bool Check_Transaction_ID(ApplicationDbContext context, string Txn_id)
        {
            bool flag = false;
            // var context = SiteConfig.dbContext;
           
            
                if (context.user_payments.Where(p => p.transactionid == Txn_id).Count() > 0)
                    flag = true;
            

            return flag;
        }

        public static void Update_Paypal_Email(ApplicationDbContext context, string UserName, string payeeemail)
        {
            var item = context.AspNetusers
                    .Where(p => p.UserName == UserName)
                    .FirstOrDefault<ApplicationUser>();

            if(item != null)
            {
                item.paypal_email = UtilityBLL.processNull(payeeemail, 0);

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
                
            
        }

        // fetch user email
        public static List<ApplicationUser> Fetch_User_Email(ApplicationDbContext context, string UserName)
        {
           var _items = new List<ApplicationUser>();
            // var context = SiteConfig.dbContext;
           
            
                _items = context.AspNetusers
                     .Where(p => p.UserName == UserName)
                     .Select(p => new ApplicationUser()
                     {
                         paypal_email = p.paypal_email,
                         Email = p.Email
                     })
                     .ToList();
            
            return _items;
        }

        public static void Update_Status(ApplicationDbContext context, string Txn_id, int status)
        {
           
            var item = context.user_payments
                    .Where(p => p.transactionid == Txn_id)
                    .FirstOrDefault<user_payments>();

            item.status = (byte)status;

            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
            
        }

        public static void Update_Status(ApplicationDbContext context, long PaymentID, int status)
        {
            // var context = SiteConfig.dbContext;
           
            
                var item = context.user_payments
                     .Where(p => p.paymentid == PaymentID)
                     .FirstOrDefault<user_payments>();

                if(item != null)
                {
                    item.status = (byte)status;

                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
                
            
        }

        public static void Update_Subscription_ExpiryDate(ApplicationDbContext context, string username, DateTime ExpiryDate)
        {
            var item = context.AspNetusers
                    .Where(p => p.UserName == username)
                    .FirstOrDefault<ApplicationUser>();

            if(item != null)
            {
                item.last_purchased = DateTime.Now;
                item.membership_expiry = ExpiryDate;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
 
        }

        public static void Update_User_Paypal_Subscription(ApplicationDbContext context, string username, int value)
        {
           
            var item = context.AspNetusers
                    .Where(p => p.UserName == username)
                    .FirstOrDefault<ApplicationUser>();
                
            if(item != null)
            {
                item.paypal_subscriber = (byte)value;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
               
            
        }

        public static bool Check_User_Paypal_Subscription(ApplicationDbContext context, string username)
        {
            bool flag = false;
            var item = context.AspNetusers
                .Where(p => p.UserName == username)
                .FirstOrDefault<ApplicationUser>();

            if(item != null)
            {
                if (item.paypal_subscriber == 1)
                    flag = true;
            }
            return flag;
        }

        public static long Return_Payment_ID(ApplicationDbContext context, string username)
        {
            long PaymentID = 0;
            var item = context.user_payments
                .Where(p => p.username == username).Take(1).OrderByDescending(p => p.paymentid).ToList();
            PaymentID = item[0].paymentid;     
            
           return PaymentID;
        }

        // return months info based on user latest payment option
        public static int Return_Months(ApplicationDbContext context, string username)
        {
            int Month = 0;
            // var context = SiteConfig.dbContext;
           
            
                var item = context.user_payments
                    .Where(p => p.username == username).Take(1).OrderByDescending(p => p.paymentid).ToList();
                Month = item[0].months;
            
            return Month;
        }

        // load transaction status information if transaction already exist
        public static List<UserPaymentEntity> Load_Transaction_Status_Information(ApplicationDbContext context, string Txn_id)
        {
           var _items = new List<UserPaymentEntity>();
            _items = context.user_payments
                    .Where(p => p.transactionid == Txn_id)
                    .Select(p => new UserPaymentEntity()
                    {
                        payer_email = p.payer_email,
                        payment_status = p.payment_status,
                        pending_reason = p.pending_reason,
                        txn_type = p.txn_type,
                        payer_status = p.payer_status,
                        payment_type = p.payment_type,
                        status = (byte)p.status
                    }).ToList();
            return _items;
        }
        // load user transaction information
        public static List<UserPaymentEntity> Load_User_Pending_Transaction_Information(ApplicationDbContext context, long paymentid)
        {
            var _items = new List<UserPaymentEntity>();
            _items = context.user_payments
                    .Where(p => p.paymentid == paymentid)
                    .Select(p => new UserPaymentEntity()
                    {
                        username = p.username,
                        packageid = p.id,
                        price = p.price,
                        credits = (int)p.credits,
                        months = p.months,
                        status = (byte)p.status
                    }).ToList();

            return _items;
        }
        

        // load all user usernames
        public static List<ApplicationUser> Fetch_User_Account_Usage(ApplicationDbContext context, string username)
        {

           var _items = new List<ApplicationUser>();
            // var context = SiteConfig.dbContext;
           
            
                _items = context.AspNetusers
                     .Where(p => p.UserName == username)
                     .Select(p => new ApplicationUser()
                     {
                         credits = p.credits,
                         last_purchased = (DateTime)p.last_purchased,
                         membership_expiry = (DateTime)p.membership_expiry,
                         islifetimerenewal = (byte)p.islifetimerenewal,
                         type = p.type
                     }).ToList();

            
            return _items;
        }
        */
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
