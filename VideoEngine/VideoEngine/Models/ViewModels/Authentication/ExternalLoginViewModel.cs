using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Models
{
    public class ExternalLoginProviderViewModel
    {
        public string ReturnUrl { get; set; } = string.Empty;
    }
    public class ExternalLoginViewModel
    {
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "UserName must be inbetween 5 - 15 chars.")]
        [RegularExpression(@"^[a-z0-9_-]{5,15}$", ErrorMessage = "Invalid UserName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
