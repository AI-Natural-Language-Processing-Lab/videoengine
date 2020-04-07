/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component } from "@angular/core";

@Component({
  templateUrl: "./settings.html"
})
export class UserProfileSettingsComponent {
  
  Attr_Type = 3; // 0: Ads, 1: Agency / Company, 2: Artists, 3: User Profile
  Skip_Template = true; // no need to manage multiple templates
}
