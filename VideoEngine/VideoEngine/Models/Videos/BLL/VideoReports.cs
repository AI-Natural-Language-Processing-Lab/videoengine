using Jugnoon.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Jugnoon.Entity;
using System;

/// <summary>
/// Reporting API - Business Layer Designed for Videos. 
/// </summary>
/// 
namespace Jugnoon.Videos
{
    public class VideoReports
    {
        public static async Task<GoogleChartEntity> YearlyReport(ApplicationDbContext context, VideoEntity entity)
        {
            var reportData = await context.JGN_Videos
                .Join(context.AspNetusers,
                video => video.userid,
                user => user.Id,
                (video, user) => new VideoQueryEntity
                {
                    video = video,
                    user = user
                })
                     .GroupBy(o => new
                     {
                         year = o.video.created_at.Year
                     })
                     .Select(g => new ReportEntity
                     {
                         Year = g.Key.year,
                         Total = g.Count()
                     })
                     .OrderBy(a => a.Year)
                     .ToListAsync();

            var newObject = new { role = "style" };
            var data = new GoogleChartEntity()
            {
                chartType = entity.chartType,
                dataTable = new List<dynamic[]>
                {
                   new dynamic[] { "Year", "Posted Topics", newObject },
                }
            };

            foreach (var item in reportData)
            {
                data.dataTable.Add(new dynamic[] { item.Year.ToString(), item.Total, "color: #76A7FA" });
            }

            return data;
        }

        public static async Task<GoogleChartEntity> Last12MonthsReport(ApplicationDbContext context, VideoEntity entity)
        {
            var reportData = await context.JGN_Videos
                .Join(context.AspNetusers,
                video => video.userid,
                user => user.Id,
                (video, user) => new VideoQueryEntity
                {
                    video = video,
                    user = user
                })
                     .Where(p => p.video.created_at >= DateTime.Now.AddYears(-1))
                     .GroupBy(o => new
                     {
                         month = o.video.created_at.Month,
                         year = o.video.created_at.Year
                     })
                     .Select(g => new ReportEntity
                     {
                         Year = g.Key.year,
                         Month = g.Key.month,
                         Total = g.Count()
                     })
                     .OrderBy(a => a.Year)
                     .ToListAsync();

            var newObject = new { role = "style" };
            var data = new GoogleChartEntity()
            {
                chartType = entity.chartType,
                dataTable = new List<dynamic[]>
                {
                   new dynamic[] { "Month", "Posted Topics", newObject },
                }
            };

            foreach (var item in reportData)
            {
                data.dataTable.Add(new dynamic[] { item.Year.ToString(), item.Total, "color: #76A7FA" });
            }

            return data;
        }

        public static async Task<GoogleChartEntity> CurrentMonthReport(ApplicationDbContext context, VideoEntity entity)
        {
            try
            {
                var reportData = await context.JGN_Videos
                .Join(context.AspNetusers,
                video => video.userid,
                user => user.Id,
                (video, user) => new VideoQueryEntity
                {
                    video = video,
                    user = user
                })
                    .Where(p => p.video.created_at >= DateTime.Now.AddDays(-31))
                    .GroupBy(x => x.video.created_at.Day)
                    .Select(g => new ReportEntity
                    {
                        Day = g.Key,
                        Total = g.Count()
                    })
                    .OrderBy(a => a.Day)
                    .ToListAsync();

                var newObject = new { role = "style" };
                var data = new GoogleChartEntity()
                {
                    chartType = entity.chartType,
                    dataTable = new List<dynamic[]>
                {
                   new dynamic[] { "Day", "Posted Topics", newObject },
                }
                };

                foreach (var item in reportData)
                {
                    data.dataTable.Add(new dynamic[] { item.Year.ToString(), item.Total, "color: #76A7FA" });
                }

                return data;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }

            return new GoogleChartEntity();
        }


    }

}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
