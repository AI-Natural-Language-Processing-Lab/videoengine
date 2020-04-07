namespace Jugnoon.Entity
{
    public class MessageEntity : ContentEntity
    {
        public string From { get; set; }
        public string To { get; set; }
        public long reply_id { get; set; }
        public bool isSent { get; set; }
        public bool isRead { get; set; }
        public bool isDeleted { get; set; }
        public bool loadUserList { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

