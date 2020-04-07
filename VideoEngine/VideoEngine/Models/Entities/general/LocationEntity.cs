using System;

namespace Jugnoon.Entity
{
    public class LocationEntity : ContentEntity
    {
        public long parentid { get; set; } = 0;

        public bool skipCountryStates { get; set; } = false;

        public string start_search_key { get; set; } = "";
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
