using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using Jugnoon.Utility.Helper;
using System.Globalization;
using System.Linq;


namespace Jugnoon.Utility
{
    public class UtilityBLL
    {
        public static string ReturnThemePath()
        {
            var selectedTheme = Cookie.ReadCookie("VSKTheme");
            if (selectedTheme == null)
                selectedTheme = Jugnoon.Settings.Configs.GeneralSettings.site_theme;

            return selectedTheme;
        }

        /// <summary>
        /// Validate data for null or for max value
        /// </summary>
        public static string processNull(string value, int length)
        {
            string processedvalue = value;
            if (value == null)
                processedvalue = "";
            else if (length > 0)
            {
                if (value.Length > length)
                    processedvalue = value.Substring(0, length);
            }
            return processedvalue;
        }

        /// <summary>
        /// Validate long character words
        /// </summary>
        public static bool isLongWordExist(string text)
        {
            bool flag = false;
            if (text == null)
                return flag;

            if (text.Contains(" "))
            {
                var arr = text.ToString().Split(char.Parse(" "));
                for (var i = 0; i <= arr.Length - 1; i++)
                {
                    if (arr[i].Length > 30)
                        flag = true;
                }
            }
            else
            {
                if (text.Length > 30)
                    flag = true;
                else
                    flag = false;
            }
            return flag;
        }

        public static string ReplaceSpaceWithUnderscore(string input)
        {
            var str = "";
            if (input == null)
                return str;

            str = Regex.Replace(input, "\\s", "_");
            return Regex.Replace(str, "[^0-9a-zA-Z_]+", "");
        }

        public static string ReplaceSpaceWithHyphin(string input)
        {
            var str = "";
            if (input == null)
                return str;

            str = Regex.Replace(input, "\\s", "-");
            str = Regex.Replace(str, "[\\-]+", "-");
            return Regex.Replace(str, "[^0-9a-zA-Z-_]+", "");
        }

        /// <summary>
        /// Strip special chars and replace space with hyphin (also allow (.) dot)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceSpaceWithHyphin_v2(string input)
        {
            var str = "";
            if (input == null)
                return str;
            // replace all spaces with hypin
            str = Regex.Replace(input, " ", "-");
            str = Regex.Replace(str, @"[\-]+", "-");
            // remove special characters
            // str = Regex.Replace(str, @"[\[\]\\\^\$\|\/\?\*\+\(\)\{\}%,:'""`«»“”‘’;><!@#&\+]?", "");
            Regex r = new Regex("(?:[^a-z0-9-._]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(str, String.Empty);
        }

        public static string ReplaceHyphinWithSpace(string input)
        {
            if (input == null)
                return "";

            return Regex.Replace(input, @"[\-]", " ");
        }

        public static string Add_NoFollow_Tag(string input)
        {
            if (input == null)
                return "";

            return Regex.Replace(input, "<a", "<a rel='nofollow'");
        }

        // replace [p] with page number value
        public static string Add_pagenumber(string input, string value)
        {
            if (input == null)
                return "";
            if (value == null)
                value = "1";

            return Regex.Replace(input, "\\[p\\]", value);
        }

        /// <summary>
        /// Encode text e.g apple -> a***e
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Restrict_Word(string word)
        {
            if (word == null)
                return "";

            int length = word.Length;
            int count = 0;
            var str = new StringBuilder();
            foreach (char c in word)
            {
                if (count > 0 && count < length - 1)
                {
                    str.Append("*");
                }
                else
                {
                    str.Append(c);
                }

                count += 1;
            }
            return str.ToString();
        }

        /// <summary>
        /// Remove all images from text / html
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveImages(string input)
        {
            if (input == null)
                return "";

            string pattern = "</?(?i:img)(.|\\n)*?>";
            return Regex.Replace(input, pattern, string.Empty);
        }

        /// <summary>
        /// Strip html tags or attributes from text
        /// </summary>
        /// <param name="input"></param>
        /// <param name="striplinks"></param>
        /// <returns></returns>
        public static string StripHTML(string input, bool striplinks = false)
        {
            if (input == null)
                return "";

            string str = Regex.Replace(input, @"<script[^>]*?>.*?</script>", "");  // Strip out javascript 
            str = Regex.Replace(str, @"<[\/\!]*?[^<>]*?>", "");  //  Strip out HTML tags 
            str = Regex.Replace(str, @"<style[^>]*?>.*?</style>", "");  // Strip style tags properly 
            str = Regex.Replace(str, @"<![\s\S]*?--[ \t\n\r]*>", "");  // Strip multi-line comments including CDATA 
            str = Regex.Replace(str, @"\[(\w)+\](.+)\[/(\w)+\]", ""); // remove bbcode e.g [abc]...[/abc]
            return str;
        }

        /// <summary>
        /// Strip html tags or attributes from text except p (paragraph)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHTML_v2(string input)
        {
            if (input == null)
                return "";

            var pattern = "</?(?i:script|font|span|frameset|frame|iframe|meta|link|style)(.|\\n)*?>";
            return Regex.Replace(input, pattern, "");
        }

        /// <summary>
        /// Strip html tags or attributes from text except a (hyperlink)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHTML_v3(string input)
        {
            if (input == null)
                return "";

            var pattern = "<(?!\\/?a(?=>|\\s.*>))\\/?.*?>";
            return Regex.Replace(input, pattern, "");
        }

        /// <summary>
        /// Strip html tags or attributes from text except a (hyperlink) and p (paragraph)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHTML_v4(string input)
        {
            if (input == null)
                return "";

            var pattern = "<(?!\\/?(a|p)(?=>|\\s.*>))\\/?.*?>";
            return Regex.Replace(input, pattern, "");
        }

       /// <summary>
       /// Generate hyperlink from url within text
       /// </summary>
       /// <param name="html"></param>
       /// <returns></returns>
        public static string Prepare_Urls(string html)
        {
            if (html == null)
                return "";

            string tStr = html;
            string http_url = "";
            string short_url = "";
            string http_pattern = "(\\[)(?<url>(http(s)?):(([a-zA-Z0-9._\\\\\\.\\/\\?\\+\\%#&\\+-=])+)?)(\\])";
            // match url [http:'www.abc.com] , [] is used to protect urls used within src or href urls
            Match VDMatch = Regex.Match(html, http_pattern);
            while (VDMatch.Success)
            {
                http_url = VDMatch.Groups["url"].Value;
                //http_url = VDMatch.Value;
                if (http_url.Length > 50)
                {
                    short_url = http_url.Substring(0, 50) + "...";
                }
                else
                {
                    short_url = http_url;
                }

                tStr = tStr.Replace(http_url, (Convert.ToString((Convert.ToString("<a href=\"") + http_url) + "\" target=\"_blank\" rel=\"nofollow\">") + short_url) + "</a>");
                tStr = tStr.Replace("[", "");
                tStr = tStr.Replace("]", "");
                VDMatch = VDMatch.NextMatch();
            }
            return tStr;
        }

        /// <summary>
        /// Extract all jpeg images from html or text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ArrayList Extract_JPEGS(string input)
        {
            if (input == null)
                return new ArrayList();

            var m_options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
            var regexMatch = new Regex("<img[^>]+>|<a[^>]+>", m_options);
            // Regex regexMatch = new Regex(@"https?:'(?:[a-z 0-9\-]+\.)+[a-z]{2,6}(?:/[^/#?]+)+\.(?:jpg|jpeg)", m_options);
            ArrayList arr = new ArrayList();
            Match m = regexMatch.Match(input);
            while (m.Success)
            {
                arr.Add(m.Value);
                input = input.Replace(m.Value, "done");
                m = regexMatch.Match(input);
            }
            return arr;
        }

        /// <summary>
        /// Clean-up html or text
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string CleanBlogHTML(string html)
        {
            if (html == null)
                return html;

            html = Regex.Replace(html, "<(.|\\n)*?>", "");
            // remove <.... >
            html = Regex.Replace(html, "(\\[(\\w)+\\](.+)\\[/(\\w)+\\])", "");
            // remove [abc]...[/abc]
            return html;
        }

        /// <summary>
        /// Cleanup search text
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string CleanSearchTerm(string html)
        {
            if (html == null)
                return "";

            html = Regex.Replace(html, "<(.|\\n)*?>", "");
            return Regex.Replace(html, "\\[(\\w)+\\](.+)\\[/(\\w)+\\]", "");
        }


        /// <summary>
        /// Customize Date
        /// </summary>
        public static string CustomizeDate(DateTime startdate, DateTime date, bool ago = true)
        {
            string time = "";
            string isago = "";
            if (ago)
                isago = " ago";

            TimeSpan diffdate = date.Subtract(startdate);
            int days = diffdate.Days;
            if (days >= 365)
            {
                double yr = Convert.ToDouble(Convert.ToInt32(days) / 365);
                int years = Convert.ToInt32(Convert.ToDouble(Math.Ceiling(yr)));
                if (years > 1)
                {
                    time = years + " years" + isago;
                }
                else
                {
                    time = years + " year" + isago;
                }
            }
            else if (days >= 31 && days < 365)
            {
                double mn = Convert.ToDouble(Convert.ToInt32(days) / 31);
                int months = Convert.ToInt32(Convert.ToDouble(Math.Ceiling(mn)));
                if (months > 1)
                {
                    time = months + " months" + isago;
                }
                else
                {
                    time = months + " month" + isago;
                }
            }
            else if (days >= 7 && days < 31)
            {
                double wk = Convert.ToDouble(Convert.ToInt32(days) / 7);
                int week = Convert.ToInt32(Convert.ToDouble(Math.Ceiling(wk)));
                if (week > 1)
                {
                    time = week + " weeks" + isago;
                }
                else
                {
                    time = week + " week" + isago;
                }
            }
            else if (days < 7 && days > 0)
            {
                if (days > 1)
                {
                    time = days + " days" + isago;
                }
                else
                {
                    time = days + " day" + isago;
                }
            }
            else if (days == 0)
            {
                int hours = diffdate.Hours;
                if (hours == 0)
                {
                    int minutes = diffdate.Minutes;
                    if (minutes > 1)
                    {
                        time = minutes + " mins" + isago;
                    }
                    else
                    {
                        time = minutes + " min" + isago;
                    }
                }
                else
                {
                    if (hours > 1)
                    {
                        time = hours + " hours" + isago;
                    }
                    else
                    {
                        time = hours + " hour" + isago;
                    }

                }
            }
            return time;
        }

        /// <summary>
        /// Customize Duration
        /// </summary>
        public static string Customize_Duration(string Duration)
        {
            if (Duration == null)
                return "";

            try
            {
                TimeSpan _span = TimeSpan.Parse(Duration);
                int seconds = _span.Seconds;
                int minutes = _span.Minutes;
                int hours = _span.Hours;
                string str = _span.Minutes + ":" + _span.Seconds;
                if (hours > 0)
                {
                    str = Convert.ToString(_span.Hours + ":") + str;
                }

                return str;
            }
            catch (Exception ex)
            {
                return Duration;
            }

        }

        /// <summary>
        /// Calculate two date difference in days
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="lastdate"></param>
        /// <returns></returns>
        public static int GetDateDiff(DateTime startdate, DateTime lastdate)
        {
            TimeSpan diffdate = lastdate.Subtract(startdate);
            return diffdate.Days;
        }

        /// <summary>
        /// Tune text
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FixCode(string html)
        {
            if (html == null)
                return "";

            html = html.Replace("  ", "&nbsp; ");
            html = html.Replace("  ", " &nbsp;");
            html = html.Replace("\t", "&nbsp;&nbsp;&nbsp;");
            html = html.Replace("[", "&#91;");
            html = html.Replace("]", "&#93;");
            html = html.Replace("<", "&lt;");
            html = html.Replace(">", "&gt;");
            html = html.Replace("\n\n\n\n", "<br /><br />");
            html = html.Replace("\n\n\n", "<br /><br />");
            html = html.Replace("\n\n", "<br /><br />");
            html = html.Replace("\n", "<br />");

            // href
            html = Prepare_Urls(html);

            return html;
        }

        /// <summary>
        /// Compress code
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string CompressCode(string html)
        {
            if (html == null)
                return "";

            return html.Replace(Environment.NewLine, "");
        }

        public static string CompressCodeBreak(string html)
        {
            if (html == null)
                return "";

            return html.Replace(Environment.NewLine, "<br />");

        }

        public static string DeCompressCodeBreak(string html)
        {
            if (html == null)
                return "";

            html = html.Replace("<br />", "\n");
            html = html.Replace("<br>", "\n");
            html = html.Replace("&nbsp;", "  ");

            return html;
        }

        /*/// <summary>
        /// Fetch gallery id from string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static ArrayList Fetch_Gallery_ID(string html)
        {
            if (html == null)
                return new ArrayList();

            var m_options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
            var r_gid = new Regex("\\[GID\\](?<inner>(.*?))\\[/GID\\]", m_options);
            var VDMatch = r_gid.Match(html);
            var _arr = new ArrayList();
            while (VDMatch.Success)
            {
                _arr.Add(VDMatch.Groups["inner"].Value);
                VDMatch = VDMatch.NextMatch();
            }
            return _arr;
        }*/

        /// <summary>
        /// Fetch youtube urls from html or text
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static ArrayList Fetch_Youtube_Url(string html)
        {
            if (html == null)
                return new ArrayList();

            var m_options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
            var r_gid = new Regex("\\[YOUTUBE\\](?<inner>(.*?))\\[/YOUTUBE\\]", m_options);
            var VDMatch = r_gid.Match(html);
            var _arr = new ArrayList();
            while (VDMatch.Success)
            {
                _arr.Add(VDMatch.Groups["inner"].Value);
                VDMatch = VDMatch.NextMatch();
            }
            return _arr;
        }

        /// <summary>
        /// Fetch vimeo urls from html or text
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static ArrayList Fetch_Vimeo_URL(string html)
        {
            if (html == null)
                return new ArrayList();

            var m_options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
            var r_gid = new Regex("\\[VIMEO\\](?<inner>(.*?))\\[/VIMEO\\]", m_options);
            var VDMatch = r_gid.Match(html);
            var _arr = new ArrayList();
            while (VDMatch.Success)
            {
                _arr.Add(VDMatch.Groups["inner"].Value);
                VDMatch = VDMatch.NextMatch();
            }
            return _arr;
        }



        public static string UppercaseFirst(string s, bool isallcharacters = true)
        {
            if (s == null)
                return "";

            var str = new StringBuilder();
            if (isallcharacters)
            {
                if (s.Contains(" "))
                {
                    string[] arr = s.ToString().Split(char.Parse(" "));
                    int i = 0;
                    for (i = 0; i <= arr.Length - 1; i++)
                    {
                        if (arr[i].ToString().Length > 0)
                        {
                            str.Append(UppercaseFirst(arr[i].ToString()) + Convert.ToString(" "));
                        }
                    }
                }
                else
                {
                    str.Append(UppercaseFirst(s));
                }
            }
            else
            {
                str.Append(UppercaseFirst(s));
            }
            return str.ToString();
        }

        public static string UppercaseFirst(string s)
        {
            if (s == null)
                return "";
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static void RenameFile(string oldpath, string newpath)
        {
            if (File.Exists(oldpath))
            {
                File.Move(oldpath, newpath);
                if (File.Exists(newpath))
                {
                    File.Delete(oldpath);
                }
            }
        }

        /// <summary>
        /// Generate Date in Different Formats
        /// </summary>
        public static string Generate_Date(DateTime _date, int FormatID)
        {
            string _date_output = "";
            //switch (Blog_Settings.PostDateTemplate)
            switch (FormatID)
            {
                case 0:
                    // 0:  21 May, 2011
                    _date_output = _date.ToString("dd MMMM, yyyy");
                    break; // TODO: might not be correct. Was : Exit Select
                case 1:
                    // 1: May 30th, 2011
                    _date_output = _date.ToString("MMMM dd, yyyy");
                    break; // TODO: might not be correct. Was : Exit Select
                case 2:
                    // 2: May 11 2011
                    _date_output = _date.ToString("MMMM dd, yyyy");
                    break; // TODO: might not be correct. Was : Exit Select
                case 3:
                    // 3: 2 days ago 
                    _date_output = CustomizeDate(_date, DateTime.Now);
                    break; // TODO: might not be correct. Was : Exit Select
                case 4:
                    // Today 10:54 PM
                    string suffix = "AM";
                    int hours = 0;
                    if (_date.Hour > 12)
                    {
                        suffix = "PM";
                        hours = _date.Hour - 12;
                    }
                    TimeSpan diffdate = DateTime.Now.Subtract(_date);
                    int days = diffdate.Days;
                    string date_suffix = "";
                    if (days == 0)
                    {
                        date_suffix = "Today";
                    }
                    else if (days == 1)
                    {
                        date_suffix = "Yesterday";
                    }
                    else
                    {
                        date_suffix = _date.Month + " " + _date.Day + ", " + _date.Year;
                    }
                    _date_output = date_suffix + " " + hours + ":" + _date.Minute + " " + suffix;
                    break; // TODO: might not be correct. Was : Exit Select
            }
            return _date_output;
        }

        public static string Process_Content_Text(string html)
        {
            if (html == null)
                return "";

            // compress code :-> replace \n - <br />
            html = CompressCodeBreak(html);
            // process bbcode
            html = BBCode.MakeHtml(html, true);
            // prepare urls
            html = GenerateLink(html, true);
            return html;
        }

        // remove html
        // compress code breaks
        // generate links
        // no bbcode processing
        // used in comments etc processing
        public static string Process_Content(string html)
        {
            if (html == null)
                return "";

            var _cmt = StripHTML(html).Trim();
            // remove all html
            if (string.IsNullOrEmpty(_cmt))
            {
                return "";
            }
            // generate urls
            // fix html code
            _cmt = CompressCodeBreak(_cmt);
            _cmt = GenerateLink(_cmt, true);

            return _cmt;
        }

        /// <summary>
        /// Validate url
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static bool ValidateUrl(string URL)
        {
            if (URL == null)
                return false;

            var VDMatch = Regex.Match(URL, "(http|https):\\/\\/(\\w+:{0,1}\\w*@)?(\\S+)(:[0-9]+)?(\\/|\\/([\\w#!:.?+=&%@!\\-\\/]))?");
            if (VDMatch.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Utility Function To Replace String With Hyperlink
        /// </summary>
        /// <param name="text"></param>
        /// <param name="nofollow"></param>
        /// <returns></returns>
        public static string GenerateLink(string text, bool nofollow)
        {
            if (text == null)
                return "";

            var pattern = "(?<hrefurl><(a|iframe)[^>]*>.*?</(a|iframe)>)|(?<imgurl><img.*?/>)|(?<httpurl>(http|ftp|https):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?)";
            if (nofollow)
                return Regex.Replace(text, pattern, new MatchEvaluator(ComputeReplacementNoFollow));
            else
                return Regex.Replace(text, pattern, new MatchEvaluator(ComputeReplacement));
        }

        public static String ComputeReplacement(Match m)
        {
            if (m.Groups["imgurl"].Success)
            {
                return m.Groups["imgurl"].Value;
            }
            else if (m.Groups["httpurl"].Success)
            {
                return "<a href=\"" + m.Groups["httpurl"].Value + "\">" + m.Groups["httpurl"].Value + "</a>";
            }
            else
            {
                return m.Groups["hrefurl"].Value;
            }
        }

        // Computer Replacement With No Follow Text
        public static String ComputeReplacementNoFollow(Match m)
        {
            if (m.Groups["imgurl"].Success)
                return m.Groups["imgurl"].Value;
            else if (m.Groups["httpurl"].Success)
                return "<a href=\"" + m.Groups["httpurl"].Value + "\" rel=\"nofollow\">" + m.Groups["httpurl"].Value + "</a>";
            else
                return m.Groups["hrefurl"].Value;
        }


        public static int ReturnMonth(string MonthName)
        {
            return DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(MonthName) + 1;
        }

        public static string ReturnMonthName(int Month)
        {
            var mfi = new DateTimeFormatInfo();
            return mfi.GetAbbreviatedMonthName(Month);
        }

        public static string ParseUsername(string input, string username)
        {
            if (input == null)
                return "";

            return Regex.Replace(input, "\\[USERNAME\\]", username);
        }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
