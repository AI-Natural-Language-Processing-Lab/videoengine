
namespace Jugnoon.Settings
{
    public class Location
    {
        /// <summary>
        /// Toggle on | off location broad to specific or all countries
        /// </summary>
        public bool enable_country { get; set; }

        /// <summary>
        /// Toggle on | off location broad to specific or all states within country
        /// </summary>
        public bool enable_state { get; set; }

        /// <summary>
        /// Comma separated list of countries supported within your application
        /// </summary>
        public string selected_countries { get; set; }

        /// <summary>
        /// Comma separated list of states supported within your application
        /// </summary>
        public string selected_states { get; set; }

        /// <summary>
        /// Comma separated list of cities supported within your application
        /// </summary>
        public string selected_cities { get; set; }
        
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
