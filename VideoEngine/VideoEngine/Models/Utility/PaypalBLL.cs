using System.Net;
using System.Text;

namespace Jugnoon.Utility
{
    public class PaypalBLL
    {

        /// <summary>
        /// Subscription Enabled (0: Off, 1: On)
        /// </summary>
        public static bool isPaypalSubscription()
        {
            return Settings.Configs.PremiumSettings.enable_paypal;
        }


        /// <summary>
        /// Get Paypal Live Status -> 0: Sandbox, 1: Live
        /// </summary>
        public static int Paypal_Live_Status()
        {
            if (Settings.Configs.PremiumSettings.sandbox_mode)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Get Paypal Receiver Email
        /// </summary>
        public static string Paypal_Receiver_Email()
        {
            return Settings.Configs.PremiumSettings.paypal_receiver_email;
        }

        /// <summary>
        /// Paypal Button Image Url
        /// </summary>
        public static string Paypal_Button_Image_Url()
        {
            if(Jugnoon.Settings.Configs.PremiumSettings.premium_option == 1)
               return "https://www.paypalobjects.com/webstatic/en_US/btn/btn_subscribe_113x26.png"; // subscription
            else
               return "https://www.paypalobjects.com/en_US/i/btn/x-click-but6.gif"; // credit / debit
        }

        /// <summary>
        /// Paypal Unsubscribe Button Image Url
        /// </summary>
        public static string Paypal_Unsubscribe_Button_Image_Url()
        {
            return "https://www.paypalobjects.com/en_US/i/btn/btn_unsubscribe_SM.gif";
        }

        public static string GetPayPalPaymentUrl(string item_name, string item_number, string amount, string quantity, string username)
        {
            string strURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            if (Paypal_Live_Status() == 1)
                strURL = "https://www.paypal.com/cgi-bin/webscr";
            string BaseUrl = Config.GetUrl();
            string cancel_url = BaseUrl + "paypal/cancel";
            string return_url = BaseUrl + "paypal/confirmation";
            string notifyUrl = BaseUrl + "paypal/index";
            //string image_url = HttpUtility.UrlEncode(BaseUrl + "images/logo/classified_header.jpg");
            string image_url = ""; // paypal header url
            string paypal_logo = WebUtility.UrlEncode(BaseUrl + "images/logo.png");
            StringBuilder Url = new StringBuilder();
            string CustomFieldValue = username; //User-defined field which PayPal passes through the system and returns to you in your merchant payment notification email. Subscribers do not see this field.
            Url.Append(strURL + "?cmd=_xclick&upload=1&rm=2&no_shipping=1&no_note=1&currency_code=USD&business=" + PaypalBLL.Paypal_Receiver_Email() + "&item_number=" + item_number + "&item_name=" + item_name + "&amount=" + amount + "&quantity=" + quantity + "&undefined_quantity=0&notify_url=" + notifyUrl + "&return=" + return_url + "&cancel_return=" + cancel_url + "&cpp_header_Image=" + image_url + "&cpp_headerback_color=ECDFDF&cpp_headerborder_color=A02626&cpp_payflow_color=ECDFDF&image_url=" + paypal_logo + "&custom=" + CustomFieldValue + "");
            return Url.ToString();
        }

        public static string GetPayPalSubscriptionURL(string item_name, string item_number, string amount, int months, string username)
        {
            int RecurringPaymentOPtion = 1; // // Recurring payments. Subscription payments recur unless subscribers cancel their subscriptions before the end of the current billing cycle or you limit the number of times that payments recur with the value that you specify for srt.
                                            //Allowable values are:
                                            //0 – subscription payments do not recur
                                            //1 – subscription payments recur
            string CustomFieldValue = username; //User-defined field which PayPal passes through the system and returns to you in your merchant payment notification email. Subscribers do not see this field.
                                                //The default is 0.
                                                // t1
                                                //See description.
                                                //Trial period 1 units of duration. Required if you specify a1.
                                                //Allowable values are:

            //D – for days; allowable range for p2 is 1 to 90
            //W – for weeks; allowable range for p2 is 1 to 52
            //M – for months; allowable range for p2 is 1 to 24
            //Y – for years; allowable range for p2 is 1 to 5
            string strURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            if (Paypal_Live_Status() == 1)
                strURL = "https://www.paypal.com/cgi-bin/webscr";
            string BaseUrl = Config.GetUrl();
            string cancel_url = BaseUrl + "paypal/subscription/cancel.aspx";
            string return_url = BaseUrl + "paypal/subscription/confirmation.aspx";
            string notifyUrl = BaseUrl + "paypal/ipn.aspx";
            //string image_url = HttpUtility.UrlEncode(BaseUrl + "images/logo/classified_header.jpg");
            string image_url = ""; // paypal header url
            string paypal_logo = WebUtility.UrlEncode(BaseUrl + "images/logo.png");
            StringBuilder Url = new StringBuilder();
            Url.Append(strURL + "?cmd=_xclick-subscriptions&business=" + PaypalBLL.Paypal_Receiver_Email() + "&item_name=" + WebUtility.UrlEncode(item_name) + "&a3=" + amount + "&p3=" + months + "&t3=M&currency_code=USD&notify_url=" + notifyUrl + "&return=" + return_url + "&cancel_return=" + cancel_url + "&cpp_header_Image=" + image_url + "&cpp_headerback_color=ECDFDF&cpp_headerborder_color=A02626&cpp_payflow_color=ECDFDF&image_url=" + paypal_logo + "&src=" + RecurringPaymentOPtion + "&custom=" + CustomFieldValue + "");
            return Url.ToString();
        }


        public static string GetPayPalUnsubscriptionURL(string email)
        {
            //The default is 0.
            // t1
            //See description.
            //Trial period 1 units of duration. Required if you specify a1.
            //Allowable values are:

            //D – for days; allowable range for p2 is 1 to 90
            //W – for weeks; allowable range for p2 is 1 to 52
            //M – for months; allowable range for p2 is 1 to 24
            //Y – for years; allowable range for p2 is 1 to 5
            string strURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            if (PaypalBLL.Paypal_Live_Status() == 1)
                strURL = "https://www.paypal.com/cgi-bin/webscr";
            string BaseUrl = Config.GetUrl();
            string cancel_url = BaseUrl + "paypal/unsuscribe/cancel.aspx";
            string return_url = BaseUrl + "paypal/unsuscribe/confirmation.aspx";
            string notifyUrl = BaseUrl + "paypal/ipn.aspx";
            //string image_url = HttpUtility.UrlEncode(BaseUrl + "images/logo/classified_header.jpg");
            string image_url = ""; // paypal header url
            string paypal_logo = WebUtility.UrlEncode(BaseUrl + "images/logo.png");
            StringBuilder Url = new StringBuilder();
            Url.Append(strURL + "?cmd=_subscr-find&alias=" + WebUtility.UrlEncode(email) + "&business=" + PaypalBLL.Paypal_Receiver_Email() + "&notify_url=" + notifyUrl + "&return=" + return_url + "&cancel_return=" + cancel_url + "&cpp_header_Image=" + image_url + "&cpp_headerback_color=ECDFDF&cpp_headerborder_color=A02626&cpp_payflow_color=ECDFDF&image_url=" + paypal_logo + "");
            return Url.ToString();
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
