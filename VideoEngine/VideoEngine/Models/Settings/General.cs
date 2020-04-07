namespace Jugnoon.Settings
{
    public class General
    {
        /// <summary>
        /// Choose theme name from list of available themes unless you used your own custom theme / template
        /// </summary>
        public string site_theme { get; set; }

        /// <summary>
        /// Website title used within whole web application
        /// </summary>
        public string website_title { get; set; }

        /// <summary>
        /// General website description used in various parts of application.
        /// </summary>
        public string website_description { get; set; }

        /// <summary>
        /// Append title with each page title on dynamic pages e.g [Page Title] | [Page Caption]
        /// </summary>
        public string page_caption { get; set; }

        /// <summary>
        /// Phone number used within website for support purpose unless you customize it manually.
        /// </summary>
        public string website_phone { get; set; }

        /// <summary>
        /// Admin support email used for sending mails and other support purposes within website.
        /// </summary>
        public string admin_mail { get; set; }

        /// <summary>
        /// Email name used as sender name when sending email to users from website.
        /// </summary>
        public string admin_mail_name { get; set; }

        /// <summary>
        /// Maxiumum pagination links to be used, 0 for unlimited
        /// </summary>
        public short pagination_links { get; set; }

        /// <summary>
        /// Set type of pagination to be rendered, (0: Normal, 1: Advance 3: Simple)
        /// </summary>
        public byte pagination_type { get; set; }

        /// <summary>
        /// Use type of content screening approach to scan and screen words matched with black listed dictionary words (0: screen, 1: screen & replace 2: None)
        /// </summary>
        public byte screen_content { get; set; }

        /// <summary>
        /// Set option to review user posted contents 1: Automatical approval, 0: Need moderator review before approval
        /// </summary>
        public byte content_approval { get; set; }

        /// <summary>
        /// Maximum number of reports count before forcing content to block until review.
        /// </summary>
        public short spam_count { get; set; }
                

        /// <summary>
        /// Set duration in seconds to cache content if enabled cache for. 0 for disabled cache.
        /// </summary>
        public short cache_duration { get; set; }

        /// <summary>
        /// Maximum no of pages to be cached for each type of listing within pagination. 0 for unlimited
        /// </summary>
        public byte max_cache_pages { get; set; }
        
        /// <summary>
        /// Store user types searches for creating auto search labels, auto suggestions and other internal purposes
        /// </summary>
        public bool store_searches { get; set; }

        /// <summary>
        /// Store ip addresses for security and internal use only
        /// </summary>
        public bool store_ipaddress { get; set; }

        /// <summary>
        /// Maximum characters to generate dynamic urls from content titles.
        /// </summary>
        public short maximum_dynamic_link_length { get; set; }

        /// <summary>
        /// Set default culture for your website if language functionality enabled or available
        /// </summary>
        public string default_culture { get; set; }

        /// <summary>
        /// Set no of items display in each page size
        /// </summary>
        public byte pagesize { get; set; }

        /// <summary>
        /// Setup rating option for contents 0: five star, 1: like / dislike, 2: disable rating
        /// </summary>
        public byte rating_option { get; set; }

       
        /// <summary>
        /// If false the system will initialize installation process, please don't remove or change. it will be automatically true after installation process. don't leave it as false in production.
        /// </summary>
        public bool init_wiz { get; set; }

        /// <summary>
        /// Setup / Configure JWT Private Key
        /// </summary>
        public string jwt_private_key { get; set; }


    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
