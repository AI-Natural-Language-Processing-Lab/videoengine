/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */


import { Component } from "@angular/core";
import { AppConfig } from "../../configs/app.config";
@Component({
  templateUrl: "./index.html"
})
export class SetupComponent {

  SetupType = 0; // 0: database setup, 1: // user & configuration setup
    
  constructor(public config: AppConfig
  ) {
     this.SetupType = parseInt(this.config.getGlobalVar('setuptype'), 10);
  }

}