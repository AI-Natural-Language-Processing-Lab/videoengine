
namespace Jugnoon.Settings
{
    public class Registration
    {
        /// <summary>
        /// Toggle on | off registeration process within application
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// Toggle on | off rechapcha for verification
        /// </summary>
        public bool enableChapcha { get; set; }

        /// <summary>
        /// Choose your verification option for authentication, 0: Both UserName, Email, 1: Email Address
        /// </summary>
        public byte uniqueFieldOption { get; set; }

        /// <summary>
        /// Toggle on | off first name / last name field unless you customize registeration process manually
        /// </summary>
        public bool enableNameField { get; set; }

        /// <summary>
        /// Toggle on | off showing enable privacy check option unless you customize registeration process manually
        /// </summary>
        public bool enablePrivacyCheck { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
