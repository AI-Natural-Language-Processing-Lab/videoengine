/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, ViewEncapsulation, OnInit } from "@angular/core";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

import { DataService } from "../../../account/payments/history/services/data.service";

@Component({
  templateUrl: "./history.html",
  encapsulation: ViewEncapsulation.None,
  providers: []
})
export class HistoryComponent implements OnInit {
  controls: any = [];
  showLoader = false;

  constructor(
    private dataService: DataService
  ) {}

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  @select(["accounthistory", "posts"])
  readonly posts$: Observable<any>;

  @select(["accounthistory", "loading"])
  readonly loading$: Observable<boolean>;

  @select(["accounthistory", "isloaded"])
  readonly isloaded$: Observable<any>;

  User: any = {};
  ngOnInit() {
    this.auth$.subscribe(Info => {
      this.User = Info.User;
      this.loadHistory();
    });
  }

  loadHistory() {
    // check if records not loaded, call loadRecord to fetch data from database
    this.isloaded$.subscribe((loaded: boolean) => {
      if (!loaded) {
        this.dataService.LoadRecords(this.User);
      }
    });
  }
}
