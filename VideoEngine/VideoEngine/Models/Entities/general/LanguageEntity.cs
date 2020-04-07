namespace Jugnoon.Entity
{
    public class LanguageEntity : ContentEntity
    {
        /// <summary>
        /// 0: Not Selected, 1: Selected, 2: All
        /// </summary>
        public byte isdefault { get; set; } = 2;

        /// <summary>
        /// 0: Not Selected, 1: Selected, 2: All
        /// </summary>
        public byte isselected { get; set; } = 2;

    }
}
/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
