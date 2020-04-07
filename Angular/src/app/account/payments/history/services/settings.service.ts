/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";

import * as OPTIONS from "../model";
import { AppConfig } from "../../../../configs/app.config";

@Injectable()
export class SettingsService {
  private apiOptions: OPTIONS.IAPIOptions;

  constructor(public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/payments/history"
    };
  }

  getApiOptions() {
    return this.apiOptions;
  }
}
