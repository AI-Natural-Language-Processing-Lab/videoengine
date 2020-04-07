/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { LanguageAPIActions } from "../../../../reducers/settings/language/actions";
import { HttpClient } from "@angular/common/http";

import { SettingsService } from "./settings.service";
import { CoreAPIActions } from "../../../../reducers/core/actions";

@Injectable()
export class DataService {
  constructor(
    private settings: SettingsService,
    private http: HttpClient,
    private actions: LanguageAPIActions,
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

  /* -------------------------------------------------------------------------- */
  /*                              Get Single Record                             */
  /* -------------------------------------------------------------------------- */
  GetInfo(userid: number) {
    const URL = this.settings.getApiOptions().getinfo;
    return this.http.post(URL, JSON.stringify({ userid }));
  }

  
 /* -------------------------------------------------------------------------- */
  /*               Perform actions (enable, disable, approve) etc               */
  /* -------------------------------------------------------------------------- */
  ProcessActions(SelectedItems, isenabled) {
    // apply changes directory instate
    this.actions.applyChanges({
      SelectedItems,
      isenabled
    });

   
    this.http
      .post(this.settings.getApiOptions().action, JSON.stringify(SelectedItems))
      .subscribe(
        (data: any) => {
          this.coreActions.Notify({
            title: "Operation Performed",
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
