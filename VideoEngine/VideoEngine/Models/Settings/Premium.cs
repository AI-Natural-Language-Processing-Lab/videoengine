namespace Jugnoon.Settings
{
    public class Premium
    {
        /// <summary>
        /// Enable payment functionality within your application 
        /// </summary>
        public bool enable_payment { get; set; }

        /// <summary>
        /// Enable premium options from available systems 0: credit based, 1: monthly subscription
        /// </summary>
        public byte premium_option { get; set; }

        /// <summary>
        /// Enable redirection option after signup process unless you customize with your own way. 0: redirect to myaccount, 1: redirect to payment page
        /// </summary>
        public byte premium_redirect_option { get; set; }

        /// <summary>
        /// Enable paypal as payment gateway.
        /// </summary>
        public bool enable_paypal { get; set; }

        /// <summary>
        /// Setup paypal email address for processing transactions
        /// </summary>
        public string paypal_receiver_email { get; set; }

        /// <summary>
        /// Toggle on | off sandbox mode
        /// </summary>
        public bool sandbox_mode { get; set; }
        
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
