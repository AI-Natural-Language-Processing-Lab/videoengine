using Jugnoon.Utility;
/// <summary>
/// Core Query Builder Entity - Grouped togather all shared query builder attributes.
/// </summary>
namespace Jugnoon.Entity
{

    public class ContentEntity
    {
        /// <summary>
        ///  Filter records based on id (if id > 0, default value = 0 (no filter))
        /// </summary>
        public long id { get; set; } = 0;

        /// <summary>
        /// Add filter to avoid getting record in list matched with this id (if id > 0, default value = 0 (no filter))
        /// </summary>
        public long excludedid { get; set; } = 0;

        /// <summary>
        /// Handle order by query parameter for building queries (e.g [created_at desc], default value = "")
        /// </summary>
        public string order { get; set; } = "";

        /// <summary>
        /// Filter records based on selected month (e.g 3, default value = 0 (no filter))
        /// </summary>
        public int month { get; set; } = 0;

        /// <summary>
        /// Filter records based on selected year (e.g 2019, default value = 0 (no filter))
        /// </summary>
        public int year { get; set; } = 0;

        /// <summary>
        /// Filter records based on date flag (0: All, 1: Today, 2: This Week, 3: This Month, default value = 0 (Skip))
        /// </summary>
        public DateFilter datefilter { get; set; } = DateFilter.AllTime;

        /// <summary>
        /// Toggle on | off cache for list processed by query builder (default value = false (Skip))
        /// </summary>
        public bool iscache { get; set; } = false;

        /// <summary>
        /// Handler current page index for loading records (default value = 1 (Load First Page))
        /// </summary>
        public int pagenumber { get; set; } = 1;

        /// <summary>
        /// Handler page size for loading records (default value = 18)
        /// </summary>
        public int pagesize { get; set; } = 18;

        /// <summary>
        /// Handle search term (default value = "", skip search)
        /// </summary>
        public string term { get; set; } = "";

        /// <summary>
        /// Instruct query build to fetch data from specific fields required in listings)
        /// </summary>
        public bool issummary { get; set; } = true;

        /// <summary>
        /// Instruct query build to fetch data from specific fields required for dropdowns or navigational use only)
        /// </summary>
        public bool isdropdown { get; set; } = false;

        /// <summary>
        /// Flag operator used to send targetted action from application e.g enable, disable, delete, approve, featured etc actions on specifc selected records.
        /// </summary>
        public string actionstatus { get; set; } = "";

        /// <summary>
        ///  Filter records based on isenabled flag (0: disabled, 1: enabled, 2: both) (default value = 1 (only enabled videos))
        /// </summary>
        public EnabledTypes isenabled { get; set; } = EnabledTypes.Enabled;

        /// <summary>
        ///  Filter records based on isapproved flag (0: not reviewed, 1: reviewed) (default value = 1 (only reviewed videos))
        /// </summary>
        public ApprovedTypes isapproved { get; set; } = ApprovedTypes.Enabled; // 0: not reviewed, 1: reviewed;

        /// <summary>
        ///  Filter records based on isfeatured flag (0: normal, 1: featured, 2: paid, 3: all) (default value = 3 (all videos))
        /// </summary>
        public FeaturedTypes isfeatured { get; set; } = FeaturedTypes.All;

        /// <summary>
        ///  Filter records based on categoryid (if category id > 0, default value = 0)
        /// </summary>
        public short categoryid { get; set; } = 0;

        /// <summary>
        ///  Filter records based on categoryname (default value = "" (no filter))
        /// </summary>
        public string categoryname { get; set; } = "";

        /// <summary>
        ///  Filter records based on array of categoryids (default value = [] (no filter))
        /// </summary>
        public short[] category_ids { get; set; } = new short[] { };

        /// <summary>
        ///  Filter records based on comma separated list of tags (default value = "" (no filter))
        /// </summary>
        public string tags { get; set; } = "";

        /// <summary>
        ///  Flag used to instruct query builder to skip pagination logic if enabled. (default = false)
        /// </summary>
        public bool loadall { get; set; } = false;

        /// <summary>
        ///  Flag used to instruct query builder to load only public available records (default = true)
        /// </summary>
        public bool ispublic { get; set; } = true;

        /// <summary>
        ///  Flag used to instruct query builder to skip advance filters e.g in case of getting single reocrd based on id, no need to perform search or other filters
        /// </summary>
        public bool nofilter { get; set; } = false;

        /// <summary>
        ///  Optional parameter for front application use that instruct api to load stats or shared data if not already loaded 
        /// </summary>
        public bool loadstats { get; set; } = false;

        /// <summary>
        ///  Filter records based on user_id (author) (if user_id > 0, default value = 0 (no filter))
        /// </summary>
        public string userid { get; set; } = "";

        /// <summary>
        ///  Filter records based on username (author) (if username != "", default value = "" (no filter))
        /// </summary>
        public string username { get; set; } = "";

        /// <summary>
        ///  Filter records based on isadult flag (0: normal, 1: adult, 2: all => default value = 2 (all videos))
        /// </summary>      
        public AdultTypes isadult { get; set; } = AdultTypes.All;

        /// <summary>
        ///  Filter (videos | audio) based on additional group by filter mode (mode > 0,  default 0 (no filter))
        /// </summary>
        public int mode { get; set; } = 0;

        /// <summary>
        ///  Redirect dataacces layer routing to load favorited records instead of normal records
        ///  Usage case: Load favorited user contents (videos, audio files, albums, asked questions etc)
        /// </summary>
        public bool loadfavorites { get; set; } = false;

        /// <summary>
        ///  Redirect dataacces layer routing to load liked records instead of normal records
        ///  Usage case: Load liked user contents (videos, audio files, albums, asked questions etc)
        /// </summary>
        public bool loadliked { get; set; } = false;

        /// <summary>
        ///  Redirect dataacces layer routing to load records have not resolved reports
        ///  Usage case: Load contents with report flag (videos, audio files, albums, asked questions etc)
        /// </summary>
        public bool loadabusereports { get; set; } = false;

        /// <summary>
        ///  Handle type of report to be generated
        ///  Usage case: Basic Reports
        /// </summary>
        public DefaultReportTypes reporttype { get; set; } = DefaultReportTypes.Yearly;

        /// <summary>
        ///  Type of chart when generating report
        /// </summary>
        public ChartTypes chartType { get; set; } = ChartTypes.ColumnChart;

        

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
