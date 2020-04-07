using Jugnoon.Utility;
using Jugnoon.Framework;
using Jugnoon.Models;

namespace Jugnoon.Videos.Models
{
    public class VideoViewModel
    {
        public JGN_Videos Data { get; set; }

        public VideoJsModelView Player { get; set; }

        public ActionViewModel Action { get; set; }

        public CommentViewModel Comments { get; set; }

        public InfoViewModel Info { get; set; }

        public string PreviewUrl { get; set; }

        public string DetailMessage { get; set; }

        public bool isAllowed { get; set; }

        public string Message { get; set; }

        public AlertTypes AlertType { get; set; }
    }

    public class VideoFrameViewModel
    {
        public JGN_Videos Data { get; set; }

        public VideoJsModelView Player { get; set; }

        public bool isAllowed { get; set; }

        public string DetailMessage { get; set; }

        public string Message { get; set; }

        public AlertTypes AlertType { get; set; }
    }

    public class VideoPreviewViewModel
    {
        public bool isExist { get; set; }
        public string previewUrl { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
