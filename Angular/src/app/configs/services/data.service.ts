/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { ConfigAPIActions } from "../../reducers/configs/actions";
import { HttpClient } from "@angular/common/http";
import { ConfigSettingsService } from "./settings.service";
import { CoreAPIActions } from "../../reducers/core/actions";

@Injectable()
export class ConfigDataService {
  
  constructor(
    private settings: ConfigSettingsService,
    private http: HttpClient,
    private actions: ConfigAPIActions,
    private coreActions: CoreAPIActions
  ) {
  }

  /* -------------------------------------------------------------------------- */
  /*                           Core load data api call                          */
  /* -------------------------------------------------------------------------- */
  LoadRecords(FilterOptions, apptype) {
    let URL = this.settings.getApiOptions().load;
    if (apptype === 'admin') {
      URL = this.settings.getApiOptions().load_admin;
    }
    this.actions.loadStarted();
    this.http.post(URL, JSON.stringify(FilterOptions)).subscribe(
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
