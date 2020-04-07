/// <summary>
/// List of general application types used throughout application.
/// </summary>
namespace Jugnoon.Utility
{
    public enum ContentTypes
    {
        Videos = 0,
        Photos = 1,
        Blogs = 2,
        Audio = 3,
        General = 4,
        Albums = 5,
        Forums = 6,
        QA = 7,
        Classified = 8,
        Polls = 9,
        All = 100
    };

    public enum DateFilter
    {
        Today = 1,
        ThisWeek = 2,
        ThisMonth = 3,
        ThisYear = 4,
        AllTime = 0
    };

    public enum FeaturedTypes
    {
        Normal = 0,
        Featured = 1,
        Premium = 2,
        All = 3
    };

    public enum EnabledTypes
    {
        Disabled = 0,
        Enabled = 1,
        All = 2
    };

    public enum ApprovedTypes
    {
        Disabled = 0,
        Enabled = 1,
        All = 2
    };

    public enum AdultTypes
    {
        NonAdult = 0,
        Adult = 1,
        All = 2
    };
    
    public enum RatingActionTypes
    {
        EnableRating = 1,
        DisableRating = 0,
        All = 2
    };
    public enum CommentActionTypes
    {
        EnableComments = 1,
        DisableComments = 0,
        All = 2
    };

    public enum PrivacyActionTypes
    {
        Public = 0,
        Private = 1,
        Unlisted = 2,
        All = 3
    };

    public enum DefaultReportTypes
    {
        Yearly = 0,
        Last12Months = 1,
        CurrentMonth = 2
    };

    public enum ChartTypes
    {
        ColumnChart,
        BarChart,
        LineChart,
        PieChart,
    };
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
