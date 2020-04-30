/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                    */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as OPTIONS from "../model";
import { AppConfig } from "../../../configs/app.config";

/* -------------------------------------------------------------------------- */
/*                        Core Photos / Albums Settings                       */
/* -------------------------------------------------------------------------- */
@Injectable()
export class SettingsService {
  // configurations
  private apiOptions: OPTIONS.IAPIOptions;

  constructor(public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/attr/load",
      load_attr: APIURL + "api/attr/load_attr",
      proc_template: APIURL + "api/attr/proc_template",
      proc_section: APIURL + "api/attr/proc_section",
      delete_template: APIURL + "api/attr/delete_template",
      delete_section: APIURL + "api/attr/delete_section",
      add_attr: APIURL + "api/attr/add_attr",
      delete_attr: APIURL + "api/attr/delete_attr",
    };
  }

  getApiOptions() {
    return this.apiOptions;
  }

}
