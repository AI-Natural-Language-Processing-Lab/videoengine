/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as OPTIONS from "../config.model";
import { CoreService } from "../../admin/core/coreService";
import { AppConfig } from "../app.config";

@Injectable()
export class ConfigSettingsService {
  // configurations
  private apiOptions: OPTIONS.IAPIOptions;

  constructor(private coreService: CoreService, public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/configuration/load_settings",
      load_admin: APIURL + "api/configuration/load_settings_admin"
    };
  }

  getApiOptions() {
    return this.apiOptions;
  }
}
