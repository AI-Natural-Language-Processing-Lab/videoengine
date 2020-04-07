using System.Net;
using System.Text.RegularExpressions;
/// <summary>
/// Utility class responsible for processing BBCodes
/// </summary>
namespace Jugnoon.Utility
{
    public class BBCode
    {

        private static RegexOptions m_options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
        private static Regex r_default = new Regex("\\[default\\](?<inner>(.*?))\\[/default\\]", m_options);
        private static Regex r_primary = new Regex("\\[primary\\](?<inner>(.*?))\\[/primary\\]", m_options);
        private static Regex r_danger = new Regex("\\[danger\\](?<inner>(.*?))\\[/danger\\]", m_options);
        private static Regex r_warning = new Regex("\\[warning\\](?<inner>(.*?))\\[/warning\\]", m_options);
        private static Regex r_success = new Regex("\\[success\\](?<inner>(.*?))\\[/success\\]", m_options);
        private static Regex r_info = new Regex("\\[info\\](?<inner>(.*?))\\[/info\\]", m_options);

        private static Regex r_paragraph = new Regex("\\[p\\](?<inner>(.*?))\\[/p\\]", m_options);
        private static Regex r_code1 = new Regex("\\[code\\](?<inner>(.*?))\\[/code\\]", m_options);
        private static Regex r_quote = new Regex("\\[quote\\](?<inner>(.*?))\\[/quote\\]", m_options);
        private static Regex r_size = new Regex("\\[size=(?<size>([1-9]))\\](?<inner>(.*?))\\[/size\\]", m_options);
        private static Regex r_bold = new Regex("\\[B\\](?<inner>(.*?))\\[/B\\]", m_options);
        private static Regex r_strike = new Regex("\\[S\\](?<inner>(.*?))\\[/S\\]", m_options);
        private static Regex r_italic = new Regex("\\[I\\](?<inner>(.*?))\\[/I\\]", m_options);
        private static Regex r_underline = new Regex("\\[U\\](?<inner>(.*?))\\[/U\\]", m_options);
        private static Regex r_email2 = new Regex("\\[email=(?<email>[^\\]]*)\\](?<inner>(.*?))\\[/email\\]", m_options);
        private static Regex r_email1 = new Regex("\\[email[^\\]]*\\](?<inner>(.*?))\\[/email\\]", m_options);
        private static Regex r_url1 = new Regex("\\[url\\](?<http>(http://)|(https://)|(ftp://)|(ftps://))?(?<inner>(.*?))\\[/url\\]", m_options);
        private static Regex r_url2 = new Regex("\\[url=(?<http>(http://)|(https://)|(ftp://)|(ftps://))?(?<url>[^\\]]*)\\](?<inner>(.*?))\\[/url\\]", m_options);

        // Some BBCodes are disabled, you can enable or customize it by adding more according to your requirements.

        /*private static Regex r_font = new Regex("\\[font=(?<font>([-a-z0-9, ]*))\\](?<inner>(.*?))\\[/font\\]", m_options);
        private static Regex r_color = new Regex("\\[color=(?<color>(\\#?[-a-z0-9]*))\\](?<inner>(.*?))\\[/color\\]", m_options);
        private static Regex r_bullet = new Regex("\\[\\*\\]", m_options);
        private static Regex r_list4 = new Regex("\\[list=i\\](?<inner>(.*?))\\[/list\\]", m_options);
        private static Regex r_list3 = new Regex("\\[list=a\\](?<inner>(.*?))\\[/list\\]", m_options);
        private static Regex r_list2 = new Regex("\\[list=1\\](?<inner>(.*?))\\[/list\\]", m_options);
        private static Regex r_list1 = new Regex("\\[list\\](?<inner>(.*?))\\[/list\\]", m_options);
        private static Regex r_center = new Regex("\\[center\\](?<inner>(.*?))\\[/center\\]", m_options);
        private static Regex r_left = new Regex("\\[left\\](?<inner>(.*?))\\[/left\\]", m_options);
        private static Regex r_right = new Regex("\\[right\\](?<inner>(.*?))\\[/right\\]", m_options);
        private static Regex r_quote2 = new Regex("\\[quote=(?<quote>[^\\]]*)\\](?<inner>(.*?))\\[/quote\\]", m_options);
        private static Regex r_quote1 = new Regex("\\[quote\\](?<inner>(.*?))\\[/quote\\]", m_options);
        private static Regex r_topic = new Regex("\\[topic=(?<topic>[^\\]]*)\\](?<inner>(.*?))\\[/topic\\]", m_options);
        private static Regex r_code2 = new Regex("\\[code=(?<language>[^\\]]*)\\](?<inner>(.*?))\\[/code\\]", m_options);
        private static Regex r_emotion = new Regex("\\[Emotion=(?<inner>(.*?))\\]", m_options);
        private static Regex img_src = new Regex("\\<imgs.*srcs*=s*(.*.(gif|jpeg|jpg)).*>", m_options);
        private static Regex r_youtube_2 = new Regex("\\[YOUTUBE\\](?<inner>(.*?))\\[/YOUTUBE\\]", m_options);*/

        private static Regex r_img = new Regex("\\[img\\](?<http>(http://)|(https://)|(ftp://)|(ftps://))?(?<inner>(.*?))\\[/img\\]", m_options);
        private static Regex r_youtube = new Regex("\\[youtube\\](?<inner>((http://)|(https://))?(www\\.)?youtube.com/watch\\?v=(?<id>[0-9A-Za-z-_]{11})[^[]*)\\[/youtube\\]", m_options);
        private static Regex r_vimeo = new Regex("\\[VIMEO\\](?<inner>(.*?))\\[/VIMEO\\]", m_options);

        public static string MakeHtml(string bbcode, bool DoFormatting)
        {
            if (DoFormatting)
            {
                NestedReplace(ref bbcode, r_bold, "<strong>${inner}</strong>");
                NestedReplace(ref bbcode, r_strike, "<s>${inner}</s>");
                NestedReplace(ref bbcode, r_italic, "<i>${inner}</i>");
                NestedReplace(ref bbcode, r_underline, "<u>${inner}</u>");
                NestedReplace(ref bbcode, r_email2, "<a href=\"mailto:${email}\">${inner}</a>", new string[] { "email" });
                NestedReplace(ref bbcode, r_email1, "<a href=\"mailto:${inner}\">${inner}</a>");
                NestedReplace(ref bbcode, r_url2, "<a rel=\"nofollow\" target=\"_blank\" href=\"${http}${url}\">${inner}</a>", new string[] { "url", "http" }, new string[] { "", "http://" });
                NestedReplace(ref bbcode, r_url1, "<a rel=\"nofollow\" target=\"_blank\" href=\"${http}${inner}\">${http}${inner}</a>", new string[] { "http" }, new string[] { "http://" });

                // Some BBCodes are disabled, you can enable or customize it by adding more according to your requirements.

                //If basePage.BoardSettings.BlankLinks Then
                //NestedReplace(bbcode, r_url2, "<a target=""_blank"" rel=""nofollow"" href=""${http}${url}"">${inner}</a>", New String() {"url", "http"}, New String() {"", "http://"})
                // NestedReplace(bbcode, r_url1, "<a target=""_blank"" rel=""nofollow"" href=""${http}${inner}"">${http}${inner}</a>", New String() {"http"}, New String() {"http://"})
                //NestedReplace(ref bbcode, r_url2, "<a rel=\"nofollow\" target=\"_blank\" href=\"${http}${url}\">${inner}</a>", new string[] { "url", "http" }, new string[] { "", "http://" });
                //NestedReplace(bbcode, r_font, "<span style=""font-family:${font}"">${inner}</span>", New String() {"font"})
                // NestedReplace(bbcode, r_color, "<span style=""color:${color}"">${inner}</span>", New String() {"color"})
                // bbcode = r_bullet.Replace(bbcode, "<li>")
                // NestedReplace(bbcode, r_list4, "<ol type=""i"">${inner}</ol>")
                // NestedReplace(bbcode, r_list3, "<ol type=""a"">${inner}</ol>")
                // NestedReplace(bbcode, r_list2, "<ol>${inner}</ol>")
                // NestedReplace(bbcode, r_list2, "<ul>${inner}</ul>")
                // NestedReplace(bbcode, r_center, "<div align=""center"">${inner}</div>")
                // NestedReplace(bbcode, r_left, "<div align=""left"">${inner}</div>")
                // NestedReplace(bbcode, r_right, "<div align=""right"">${inner}</div>")

                // image
                NestedReplace(ref bbcode, r_img, "<br /><img rel=\"nofollow\" src=\"${http}${inner}\"/><br />", new string[] { "http" }, new string[] { "http://" });

                NestedReplace(ref bbcode, r_quote, "<div class=\"well\">${http}${inner}</div>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_paragraph, "<p>${http}${inner}</code></p>", new string[] { "http" }, new string[] { "" });

                NestedReplaceEncode(ref bbcode, r_code1, "<pre><code class=\"language-csharp\">${http}${inner}</code></pre>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_danger, "<span class=\"label label-danger\">${http}${inner}</span>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_warning, "<span class=\"label label-warning\">${http}${inner}</span>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_success, "<span class=\"label label-success\">${http}${inner}</span>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_info, "<span class=\"label label-info\">${http}${inner}</span>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_primary, "<span class=\"label label-primary\">${http}${inner}</span>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_default, "<span class=\"label label-default\">${http}${inner}</span>", new string[] { "http" }, new string[] { "" });

                NestedReplace(ref bbcode, r_youtube, "<div class='embed-responsive embed-responsive-16by9'><iframe class='embed-responsive-item' src=\"https://www.youtube.com/embed/${id}\" frameborder ='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe></div>"
                    , new string[] { "id" });

                NestedReplace(ref bbcode, r_vimeo, "<div class='embed-responsive embed-responsive-16by9'><iframe class='embed-responsive-item' src='${inner}' webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe></div>");

                NestedReplace(ref bbcode, r_youtube, "<div class='embed-responsive embed-responsive-16by9'><iframe class='embed-responsive-item' src='${inner}' frameborder ='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe></div>");
            }

            return bbcode;
        }
        protected static void NestedReplaceEncode(ref string refText, Regex regexMatch, string strReplace, string[] Variables, string[] VarDefaults)
        {
            Match m = regexMatch.Match(refText);
            while (m.Success)
            {
                string tStr = strReplace;
                int i = 0;

                foreach (string tVar in Variables)
                {
                    string tValue = m.Groups[tVar].Value;
                    if (tValue.Length == 0)
                    {
                        // use default instead
                        tValue = VarDefaults[i];
                    }
                    tStr = tStr.Replace("${" + tVar + "}", tValue);
                    i += 1;
                }

                string value = WebUtility.HtmlEncode(m.Groups["inner"].Value);
                // encode new line with coded word to avoid from compression
                value = value.Replace(System.Environment.NewLine, "~~");

                tStr = tStr.Replace("${inner}", value);

                refText = refText.Substring(0, m.Groups[0].Index) + tStr + refText.Substring(m.Groups[0].Index + m.Groups[0].Length);

                m = regexMatch.Match(refText);
            }
        }

        /// <summary>
        /// remove quote, image, youtube urls
        /// </summary>
        /// <param name="bbcode"></param>
        /// <param name="DoFormatting"></param>
        /// <returns></returns>
        public static string RemoveHtml(string bbcode, bool DoFormatting)
        {
            if (DoFormatting)
            {
                // e-mails
                NestedReplace(ref bbcode, r_email2, "<a href=\"mailto:${email}\">${inner}</a>", new string[] { "email" });
                NestedReplace(ref bbcode, r_email1, "<a href=\"mailto:${inner}\">${inner}</a>");

                NestedReplace(ref bbcode, r_url2, "<a rel=\"nofollow\" target=\"_blank\" href=\"${http}${url}\">${inner}</a>", new string[] { "url", "http" }, new string[] { "", "http://" });

                NestedReplace(ref bbcode, r_url1, "<a rel=\"nofollow\" target=\"_blank\" href=\"${http}${inner}\">${http}${inner}</a>", new string[] { "http" }, new string[] { "http://" });

                NestedReplace(ref bbcode, r_img, "", new string[] { "http" }, new string[] { "http://" });

                NestedReplace(ref bbcode, r_quote, "", new string[] { "http" }, new string[] { "" });
                // youtube

                NestedReplace(ref bbcode, r_youtube, "", new string[] { "id" });
            }

            return bbcode;
        }

        protected static void NestedReplace(ref string refText, Regex regexMatch, string strReplace, string[] Variables, string[] VarDefaults)
        {
            Match m = regexMatch.Match(refText);
            while (m.Success)
            {
                string tStr = strReplace;
                int i = 0;

                foreach (string tVar in Variables)
                {
                    string tValue = m.Groups[tVar].Value;
                    if (tValue.Length == 0)
                    {
                        // use default instead
                        tValue = VarDefaults[i];
                    }
                    tStr = tStr.Replace("${" + tVar + "}", tValue);
                    i += 1;
                }

                string value = m.Groups["inner"].Value;
                tStr = tStr.Replace("${inner}", value);

                refText = refText.Substring(0, m.Groups[0].Index) + tStr + refText.Substring(m.Groups[0].Index + m.Groups[0].Length);

                m = regexMatch.Match(refText);
            }
        }

        protected static void NestedReplace(ref string refText, Regex regexMatch, string strReplace, string[] Variables)
        {
            Match m = regexMatch.Match(refText);
            while (m.Success)
            {
                string tStr = strReplace;

                foreach (string tVar in Variables)
                {
                    tStr = tStr.Replace("${" + tVar + "}", m.Groups[tVar].Value);
                }

                tStr = tStr.Replace("${inner}", m.Groups["inner"].Value);

                refText = refText.Substring(0, m.Groups[0].Index) + tStr + refText.Substring(m.Groups[0].Index + m.Groups[0].Length);
                m = regexMatch.Match(refText);
            }
        }

        protected static void NestedReplace(ref string refText, Regex regexMatch, string strReplace)
        {
            Match m = regexMatch.Match(refText);
            while (m.Success)
            {
                string tStr = strReplace.Replace("${inner}", m.Groups["inner"].Value);
                refText = refText.Substring(0, m.Groups[0].Index) + tStr + refText.Substring(m.Groups[0].Index + m.Groups[0].Length);
                m = regexMatch.Match(refText);
            }
        }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
