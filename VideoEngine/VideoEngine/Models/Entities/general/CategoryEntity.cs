using System;

namespace Jugnoon.Entity
{
    public class CategoryEntity : ContentEntity
    {
        public int parentid { get; set; } = -1;
        public int type { get; set; } = 0;
        public int priority { get; set; } = 0;
        public string picturename { get; set; } = "";
        public string level { get; set; } = "";
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
