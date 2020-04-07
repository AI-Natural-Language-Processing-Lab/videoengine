using Jugnoon.BLL;
namespace Jugnoon.Entity
{
    public class TagEntity : ContentEntity
    {
        public string title { get; set; } = "";
        public TagsBLL.TagType tag_type { get; set; } = TagsBLL.TagType.Normal;
        public TagsBLL.TagLevel tag_level { get; set; } = TagsBLL.TagLevel.All;
        public int priority { get; set; } = 0;
        public TagsBLL.Types type { get; set; } = TagsBLL.Types.Videos;
        public int records { get; set; } = 0;
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
