
using System.Collections.Generic;


namespace Jugnoon.Models
{
    public class BreadViewModel
    {
        public string Header { get; set; }
        public string HeaderTitle { get; set; }
        public List<BreadItem> BreadItems { get; set; }
    }

    public class BreadItem
    {
        public string title { get; set; } = "";
        public string url { get; set; } = "";
        public bool isActive { get; set; } = false;
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
