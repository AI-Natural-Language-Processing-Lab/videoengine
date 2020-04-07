
using Jugnoon.Utility;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Models
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "Sender Name Required")]
        [Display(Name = "Full Name")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [Display(Name = "Email Address")]
        [StringLength(80, ErrorMessage = "Email Address Max Length 80.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Message Required")]
        [Display(Name = "Message")]
        public string Body { get; set; }

        public string Message { get; set; }

        public AlertTypes AlertType { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

