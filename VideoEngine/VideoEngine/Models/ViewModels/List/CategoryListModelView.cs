using Jugnoon.Entity;
using Jugnoon.Framework;
using System.Collections.Generic;

namespace Jugnoon.Models
{

    /// <summary>
    /// Handling Category Listing Model
    /// </summary>
    public class CategoryListViewModel_v2 : ListViewModel
    {
        /// <summary>
        /// Specific list without pagination e.g display maximum of 4 categories 
        /// </summary>
        public bool shortList { get; set; } = false;

        /// <summary>
        /// Toggle on | off displaying records with category listings
        /// </summary>
        public bool display_records { get; set; } = true;

        /// <summary>
        /// Toggle on | off displaying category thumbnail with category listings
        /// </summary>
        public bool display_thumbnail { get; set; } = true;


        public int TotalRecords { get; set; } = 10;

        public List<JGN_Categories> DataList { get; set; }

        public CategoryEntity QueryOptions { set; get; }

        // category directory path
        public string Path { get; set; }
    }

    public class CategoryListModelView
    {

        public string HeadingTitle { get; set; } = "";

        public int Type { get; set; } = 0;

        public int TotalRecords { get; set; } = 20;

        public bool isAll { get; set; } = false;

        public string Path { get; set; } = "";

        public bool isMain { get; set; } = false;

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
