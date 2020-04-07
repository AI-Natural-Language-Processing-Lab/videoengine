using Jugnoon.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Jugnoon.Entity
{
    public class ReportEntity
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public string MonthName
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.Month);
            }
        }
        public int Total { get; set; }
    }

    public class GoogleChartEntity
    {
        public ChartTypes chartType { get; set; } = ChartTypes.ColumnChart;

        public List<dynamic[]> dataTable { get; set; }
    }

}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

