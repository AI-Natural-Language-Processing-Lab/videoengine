/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { AdvertisementAPIActions } from "../../../../reducers/settings/advertisements/actions";
import { HttpClient } from "@angular/common/http";

import { SettingsService } from "./settings.service";
import { CoreAPIActions } from "../../../../reducers/core/actions";

@Injectable()
export class DataService {
  constructor(
    private settings: SettingsService,
    private http: HttpClient,
    private actions: AdvertisementAPIActions,
    private coreActions: CoreAPIActions
  ) {}

  /* -------------------------------------------------------------------------- */
  /*                           Core load data api call                          */
  /* -------------------------------------------------------------------------- */
  LoadRecords(FilterOptions) {
    const URL = this.settings.getApiOptions().load;
    this.actions.loadStarted();
    this.http.post(URL, JSON.stringify(FilterOptions)).subscribe(
      (data: any) => {
        // update core data
        this.actions.loadSucceeded(data);
        // update list stats
        this.coreActions.refreshListStats({
          totalrecords: data.records,
          pagesize: FilterOptions.pagesize,
          pagenumber: FilterOptions.pagenumber
        });
      },
      err => {
        this.actions.loadFailed(err);
      }
    );
  }

  UpdateRecord(obj) {
    // update record in state
    this.actions.updateRecord(obj);

    this.http
      .post(this.settings.getApiOptions().proc, JSON.stringify(obj))
      .subscribe(
        (data: any) => {
          this.coreActions.Notify({
            title: "Record Updated Successfully",
            text: "",
            css: "bg-success"
          });
        },
        err => {
          this.coreActions.Notify({
            title: "Error Occured",
            text: "",
            css: "bg-danger"
          });
        }
      );
  }
}
