/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { HistoryAPIActions } from "../../../../reducers/account/history/actions";
import { HttpClient } from "@angular/common/http";

import { SettingsService } from "./settings.service";

@Injectable()
export class DataService {
  constructor(
    private settings: SettingsService,
    private http: HttpClient,
    private actions: HistoryAPIActions
  ) {}

  LoadRecords(obj: any) {
    const URL = this.settings.getApiOptions().load;
    this.actions.loadStarted();
    this.http.post(URL, JSON.stringify(obj)).subscribe(
      (data: any) => {
        // update core data
        this.actions.loadSucceeded(data);
      },
      err => {
        this.actions.loadFailed(err);
      }
    );
  }
}
