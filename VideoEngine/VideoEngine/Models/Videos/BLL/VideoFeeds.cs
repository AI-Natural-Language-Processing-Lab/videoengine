using Jugnoon.Framework;
using Jugnoon.Utility;
using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jugnoon.Videos
{
    public class VideoFeeds
    {
        #region Sitemaps

        public static async Task<string> generateGoogleSitemap(ApplicationDbContext context, VideoEntity Entity)
        {
            var str = new StringBuilder();
            str.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            str.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");
            str.AppendLine(" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\"");
            str.AppendLine(" xmlns:video=\"http://www.google.com/schemas/sitemap-video/1.1\">");
            var _lst = await VideoBLL.LoadItems(context, Entity);
            foreach (var Item in _lst)
            {
                string mediaUrl = "";
                if (Item.type == 1)
                    mediaUrl = VideoUrlConfig.Return_MP3_Audio_Url(Item.pub_url, Item.userid) + "/" + Item.videofilename;
                else
                    mediaUrl = VideoUrlConfig.Return_FLV_Video_Url(Item.pub_url, Item.userid) + "/" + Item.videofilename;


                string thumburl = VideoUrlConfig.Return_Video_Thumb_Url(Item.thumb_url, Item.userid) + "/" + Item.thumbfilename;

                str.AppendLine("<url>");
                str.AppendLine("<loc>" + VideoUrlConfig.PrepareUrl(Item) + "</loc>");
                str.AppendLine("<image:image>");
                str.AppendLine("<image:loc>" + thumburl + "</image:loc>");
                str.AppendLine("<image:caption>" + Item.title + "</image:caption>");
                str.AppendLine("</image:image>");
                str.AppendLine("<video:video>");
                str.AppendLine("<video:content_loc>");
                str.AppendLine(mediaUrl);
                str.AppendLine("</video:content_loc>");

                str.AppendLine("<video:thumbnail_loc>");
                str.AppendLine(thumburl);
                str.AppendLine("</video:thumbnail_loc>");
                str.AppendLine("<video:title>" + Item.title + "</video:title>");

                if (Item.description != "")
                {
                    str.AppendLine("<video:description>");
                    str.AppendLine(Item.description);
                    str.AppendLine("</video:description>");
                }
                str.AppendLine("</video:video>");
                str.AppendLine("</url>");
            }
            str.AppendLine("</urlset>");

            return str.ToString();
        }

        public static async Task<string> GenerateBingSitemap(ApplicationDbContext context, VideoEntity Entity)
        {
            var str = new StringBuilder();
            str.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            str.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            var _lst = await VideoBLL.LoadItems(context, Entity);
            foreach (var Item in _lst)
            {
                str.AppendLine("<url>");
                str.AppendLine("<loc>" + VideoUrlConfig.PrepareUrl(Item) + "</loc>");
                str.Append("</url>");
            }
            str.AppendLine("</urlset>");

            return str.ToString();
        }
        #endregion

        #region Feeds

        public static async Task<string> generateRSS(ApplicationDbContext context, VideoEntity Entity)
        {
            var str = new StringBuilder();
            str.AppendLine("<rss version=\"2.0\">");
            str.AppendLine("<channel>");
            str.AppendLine("<title>" + Jugnoon.Settings.Configs.GeneralSettings.website_title + "</title>");
            str.AppendLine("<description>" + Jugnoon.Settings.Configs.GeneralSettings.website_description + "</description>");
            str.AppendLine("<link>" + Config.GetUrl() + "</link>");
            str.AppendLine("<guid>" + Config.GetUrl() + "videos/" + "</guid>");
            var _lst = await VideoBLL.LoadItems(context, Entity);
            foreach (var Item in _lst)
            {
                string title_url = VideoUrlConfig.PrepareUrl(Item);
                string body = WebUtility.HtmlEncode(UtilityBLL.StripHTML_v2(Item.description));
                str.AppendLine("<item>");
                str.AppendLine("<title>" + UtilityBLL.CleanBlogHTML(UtilityBLL.StripHTML(Item.title)) + "</title>");
                str.AppendLine("<link>" + title_url + "</link>");
                str.AppendLine("<guid>" + title_url + "</guid>");
                str.AppendLine("<pubDate>" + String.Format("{0:R}", Item.created_at) + "</pubDate>");
                str.AppendLine("<description>" + body + "</description>");
                str.AppendLine("</item>");
            }
            str.AppendLine("</channel>");
            str.AppendLine("</rss>");

            return str.ToString();
        }

        public static async Task<string> generateATOM(ApplicationDbContext context, VideoEntity Entity, string url)
        {
            var str = new StringBuilder();

            str.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            str.AppendLine("<feed xmlns=\"http://www.w3.org/2005/Atom\">\n");
            str.AppendLine("<title type=\"text\">" + Jugnoon.Settings.Configs.GeneralSettings.website_title + "</title>\n");
            str.AppendLine("<subtitle type=\"html\">" + Jugnoon.Settings.Configs.GeneralSettings.website_description + "</subtitle>");
            str.AppendLine("<id>tag:" + Config.GetUrl() + "," + DateTime.Now.Year + ":3</id>");
            str.AppendLine("<link rel=\"alternate\" type=\"text/html\" hreflang=\"en\" href=\"" + Config.GetUrl("videos/?type=atom") + "\"/>");
            str.AppendLine("<link rel=\"self\" type=\"application/atom+xml\" href=\"" + url + "\"/>");
            str.AppendLine("<rights>" + Jugnoon.Settings.Configs.GeneralSettings.website_title + "</rights>");
            str.AppendLine("<generator uri=\"" + Config.GetUrl("videos/") + "\" version=\"1.0\">");
            str.AppendLine(Jugnoon.Settings.Configs.GeneralSettings.website_title + " (" + Assembly.GetEntryAssembly().GetName().Version + ")");
            str.AppendLine("</generator>");
            var _lst = await VideoBLL.LoadItems(context, Entity);
            foreach (var Item in _lst)
            {
                string title_url = VideoUrlConfig.PrepareUrl(Item);
                string body = WebUtility.HtmlEncode(UtilityBLL.StripHTML_v2(Item.description));

                str.AppendLine("<entry>");
                str.AppendLine("<title type=\"text\">" + Item.title + "</title>");
                str.AppendLine("<link rel=\"alternate\" type=\"text/html\" href=\"" + title_url + "\"/>");
                str.AppendLine("<id>tag:" + Config.GetUrl() + "," + Item.created_at.Year + ":3." + Item.id + "</id>\n");
                str.AppendLine("<updated>" + String.Format("{0:R}", Item.created_at) + "</updated>\n");
                str.AppendLine("<published>" + String.Format("{0:R}", Item.created_at) + "</published>\n");
                str.AppendLine("<author>\n");
                str.AppendLine("<name>" + Item.userid + "</name>\n");
                str.AppendLine("<uri>" + Config.GetUrl("videos/") + "</uri>\n");
                str.AppendLine("</author>\n");
                str.AppendLine("<content type=\"html\">" + body + "</content>\n");
                str.AppendLine("</entry>\n");
            }
            str.AppendLine("</feed>");

            return str.ToString();
        }
        #endregion
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
