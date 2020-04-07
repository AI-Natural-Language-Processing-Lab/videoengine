
namespace Jugnoon.Settings
{
    public class Contact
    {
        /// <summary>
        /// Setup address information to be used in contact information
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// Setup telephone information to be used in contact information
        /// </summary>
        public string tel1 { get; set; }

        /// <summary>
        /// Setup telephone information (alternative) to be used in contact information
        /// </summary>
        public string tel2 { get; set; }

        /// <summary>
        /// Setup fax information to be used in contact information
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        /// Setup email information to be used in contact information
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Setup detail information for contact section unless you customize this part manually
        /// </summary>
        public string detail_info { get; set; }

        /// <summary>
        /// Toggle on | off contact form
        /// </summary>
        public bool enable_contact_form { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
