using Jugnoon.Utility;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "UserName must be inbetween 5 - 15 chars.")]
        [RegularExpression(@"^[a-z0-9_-]{5,15}$", ErrorMessage = "Invalid UserName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^.*(?=.{6,})(?=.*[\d])(?=.*[\W]).*$", ErrorMessage = "Password should be atleast 6 character, 1 digit, 1 special character")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "I agree to the Terms and Conditions?")]
        public bool Agreement { get; set; }

        public string Message { get; set; }
        public AlertTypes AlertType { get; set; }

        public string ReturnUrl { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
