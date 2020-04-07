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
import { FormService } from "../../../admin/users/services/form.service";
import { DataService } from "../../../admin/users/services/data.service";
import { SettingsService } from "../../../admin/users/services/settings.service";
// reducer actions
import { UserAPIActions } from "../../../reducers/users/actions";
import { CookieService } from "ngx-cookie-service";

@Component({
  templateUrl: "./mypurchases.html",
  encapsulation: ViewEncapsulation.None,
  providers: [
    DataService,
    SettingsService,
    UserAPIActions,
    FormService,
    CookieService
  ]
})
export class MyPurchasesComponent implements OnInit {
  controls: any = [];
  showLoader = false;

  constructor(
  ) {}

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  uploadedFiles = [];
  User: any = {};
  ngOnInit() {
    this.auth$.subscribe(Info => {
      this.User = Info.User;
    });
  }
}
