using System.Text;
using Jugnoon.Utility;
using Jugnoon.Entity;
using Jugnoon.Framework;
using System.Threading.Tasks;
/// <summary>
/// Business Layer: For processing ATOM / RSS Feeds
/// </summary>
namespace Jugnoon.BLL
{
    public class XMLBLL
    {                        
        // Category Sitemaps
        public static async Task<string> Google_CategorySitemap(ApplicationDbContext context, CategoryEntity Entity)
        {
            var str = new StringBuilder();
            str.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            str.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");
            str.AppendLine(" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\"");
            str.AppendLine(" xmlns:video=\"http://www.google.com/schemas/sitemap-video/1.1\">");

            string path = "videos/";
           
            var _lst = await CategoryBLL.LoadItems(context,Entity);
            foreach (var Item in _lst)
            {
                str.AppendLine("<url>");
                str.AppendLine("<loc>" + CategoryUrlConfig.PrepareUrl(Item, path) + "</loc>");
                str.AppendLine("</url>");
            }
            str.AppendLine("</urlset>");

            return str.ToString();
        }

        public static async Task<string> Bing_CategorySitemap(ApplicationDbContext context, CategoryEntity Entity)
        {
            var str = new StringBuilder();
            str.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            str.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            var _lst = await CategoryBLL.LoadItems(context,Entity);
            string path = "videos/";
           
            foreach (var Item in _lst)
            {
                str.AppendLine("<url>");
                str.AppendLine("<loc>" + CategoryUrlConfig.PrepareUrl(Item, path) + "</loc>");
                str.Append("</url>");
            }
            str.AppendLine("</urlset>");

            return str.ToString();
        }

        // Tags Sitemaps
        public static string Google_TagySitemap(ApplicationDbContext context, TagEntity Entity)
        {
            var str = new StringBuilder();
            str.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            str.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");
            str.AppendLine(" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\"");
            str.AppendLine(" xmlns:video=\"http://www.google.com/schemas/sitemap-video/1.1\">");
            string path = "videos/";
           
            var _lst = TagsBLL.LoadItems(context,Entity).Result;
            foreach (var Item in _lst)
            {
                str.AppendLine("<url>");
                str.AppendLine("<loc>" + TagUrlConfig.PrepareUrl(Item, path) + "</loc>");
                str.AppendLine("</url>");
            }
            str.AppendLine("</urlset>");

            return str.ToString();
        }

        public static string Bing_TagSitemap(ApplicationDbContext context, TagEntity Entity)
        {
            var str = new StringBuilder();
            str.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            str.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            var _lst = TagsBLL.LoadItems(context,Entity).Result;
            string path = "videos/";
           
            foreach (var Item in _lst)
            {
                str.AppendLine("<url>");
                str.AppendLine("<loc>" + TagUrlConfig.PrepareUrl(Item, path) + "</loc>");
                str.Append("</url>");
            }
            str.AppendLine("</urlset>");

            return str.ToString();
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
