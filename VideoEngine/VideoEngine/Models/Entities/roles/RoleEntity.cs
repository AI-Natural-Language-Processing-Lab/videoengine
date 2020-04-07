

namespace Jugnoon.Entity
{

    // query related
    public class RoleEntity : ContentEntity
    {
        public string rolename { get; set; } = "";

    }

    public class RoleObject : ContentEntity
    {
        public string objectname { get; set; } = "";
        public string uniqueid { get; set; } = "";

    }

    public class RoleDPermissionEntity : ContentEntity
    {
        public int roleid { get; set; } = 0;
        public int objectid { get; set; } = 0;
        
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
