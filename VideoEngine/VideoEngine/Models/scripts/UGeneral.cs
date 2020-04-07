using Jugnoon.Utility;
using Ganss.XSS;


namespace Jugnoon.Scripts
{
    public class UGeneral
    {
        public static string SanitizeText(string text, bool isCompress = true)
        {
            var _sanitize = new HtmlSanitizer();
            string _text = _sanitize.Sanitize(text);
            if (isCompress)
                _text = UtilityBLL.CompressCodeBreak(_text);

            return _text;
        }

        //****************************************************
        // GENERAL UTILITY FUNCTIONS
        //****************************************************

        // Utility function to prepare description of any record
        public static string Prepare_Description(string description, int length)
        {
            string desc = UtilityBLL.CompressCodeBreak(BBCode.MakeHtml(description, true));

            // again replace coded word (~~ by bbcode to newline)
            desc = desc.Replace("~~", System.Environment.NewLine);

            if (desc == "")
                return desc;

            if (desc.Length > length && length > 0)
                desc = desc.Substring(0, length) + "..";

            return desc;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
