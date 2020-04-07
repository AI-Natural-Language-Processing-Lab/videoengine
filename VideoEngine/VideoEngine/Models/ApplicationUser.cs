using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jugnoon.Framework;
using Microsoft.AspNetCore.Identity;

namespace Jugnoon.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(80)]
        public string firstname { get; set; }

        [MaxLength(80)]
        public string lastname { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? last_login { get; set; }

        [MaxLength(200)]
        public string picturename { get; set; }

        public byte isenabled { get; set; }

        public string val_key { get; set; }

        public int views { get; set; }

        public short roleid { get; set; }
       
        public byte type { get; set; }

        [NotMapped]
        public string url { get; set; }

        [NotMapped]
        public JGN_User_Stats stats { get; set; }

        [NotMapped]
        public JGN_User_Settings settings { get; set; }

        [NotMapped]
        public JGN_User_Account account { get; set; }

        [NotMapped]
        public List<JGN_Attr_TemplateSections> options { get; set; }

        [NotMapped]
        public List<JGN_Attr_Values> attr_values { get; set; }

        [NotMapped]
        public string img_url { get; set; }
        [NotMapped]
        public string role_name { get; set; }
        [NotMapped]
        public string password { get; set; }
        [NotMapped]
        public string npassword { get; set; }
        [NotMapped]
        public string opassword { get; set; }

        [NotMapped]
        public string customize_register_date { get; set; }

        [NotMapped]
        public string customize_last_login { get; set; }

        [NotMapped]
        public bool isadmin { get; set; }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
