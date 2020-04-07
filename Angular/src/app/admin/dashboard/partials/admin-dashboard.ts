/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { select } from "@angular-redux/store";
import { AppConfig } from "../../../configs/app.config";

@Component({
  selector: "app-admin-dashboard",
  templateUrl: "./admin-dashboard.html"
})
export class AdminDashboardComponent implements OnInit {
  constructor(public config: AppConfig) {}

  // Authenticated User Data
  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // Application Configuration Data
  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;

  User: any = {};
  isAdmin = true;
  ngOnInit() {
    this.auth$.subscribe((auth: any) => {
      this.User = auth.User;
    });
  }
}
