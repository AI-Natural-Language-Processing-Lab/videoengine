namespace Jugnoon.Settings
{
    public class Features
    {
        /// <summary>
        /// Enable video functionality in application if module available.
        /// </summary>
        public bool enable_videos { get; set; }
      
        /// <summary>
        /// Toggle on | off categorizing contents and listing
        /// </summary>
        public bool enable_categories { get; set; }

        /// <summary>
        /// Toggle on | off labeling or tagging contents and listing
        /// </summary>
        public bool enable_tags { get; set; }


        /// <summary>
        /// Display (total number of public records) counter with category or tag links
        /// </summary>
        public bool showLabelCounter { get; set; }

        /// <summary>
        /// Toggle on | off archiving contents and listing
        /// </summary>
        public bool enable_archives { get; set; }


        /// <summary>
        /// Toggle on | off group by contents based on today / this week / this month / all time date filters
        /// </summary>
        public bool enable_date_filter { get; set; }

        /// <summary>
        /// Toggle on | off advertisement within application
        /// </summary>
        public bool enable_advertisement { get; set; }

        /// <summary>
        /// Toggle on | off adult content verification warning
        /// </summary>
        public bool enable_adult_veritifcation { get; set; }

        /// <summary>
        /// Toggle on | off enabling multiple language support within application
        /// </summary>
        public bool enable_languages { get; set; }

    }
}


/*
    * This file is subject to the terms and conditions defined in
    * file 'LICENSE.md', which is part of this source code package.
    * Copyright 2007 - 2020 MediaSoftPro
    * For more information email at support@mediasoftpro.com
 */
