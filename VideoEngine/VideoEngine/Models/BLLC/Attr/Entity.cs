using Jugnoon.Entity;

/// <summary>
/// Dynamic Attributes Processing Business Layer
/// </summary>
namespace Jugnoon.Attributes
{
    public class AttrTemplateEntity : ContentEntity
    {
        /// <summary>
        /// Load dynamic attributes for specific type of content e.g ads, company, user profile, artist etc
        /// </summary>
        public Attr_Type attr_type { get; set; } = Attr_Type.Ad;

        /// <summary>
        /// For Application Allows App to load Templates + Sections or Direct Sections
        /// </summary>
        public bool skip_template { get; set; } = false;
    }

    public class AttrTemplateSectionEntity : ContentEntity
    {
        public short templateid { get; set; } = 0;

        public Attr_Type attr_type { get; set; } = Attr_Type.Ad;
    }

    public class AttrAttributeEntity : ContentEntity
    {
        public int type { get; set; } = 0;
        public short sectionid { get; set; } = 0;
        /// <summary>
        /// Dynamic Values (0: Ads, 1: Agencies)
        /// </summary>
        public byte attr_type { get; set; } = 0;
    }

    public class AttrValueEntity : ContentEntity
    {
        /// <summary>
        /// Unique id of either Ads or Agency based on Attr_Type
        /// </summary>
        public long contentid { get; set; } = 0;

        public byte type { get; set; } = 0;

        /// <summary>
        /// Dynamic Values (0: Ads, 1: Agencies)
        /// </summary>
        public Attr_Type attr_type { get; set; } = Attr_Type.Ad;

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
