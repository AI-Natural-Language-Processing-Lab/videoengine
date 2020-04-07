/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, EventEmitter, Input, Output } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { select } from "@angular-redux/store";
import { AppConfig } from "../../../configs/app.config";
import { DataService } from "../../../admin/users/services/data.service";

@Component({
  selector: "app-account-dashboard",
  templateUrl: "./account-dashboard.html",
  providers: [DataService]
})
export class AccountDashboardComponent implements OnInit {

  constructor(public config: AppConfig,
    private dataService: DataService) {}

  // Authenticated User Data
  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // Application Configuration Data
  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;

  @Output() View = new EventEmitter<any>();
  @Output() SelectedItems = new EventEmitter<any>();
  
  User: any = {};
  ngOnInit() {
    this.auth$.subscribe((auth: any) => {
       this.User = auth.User;
    });
  }

  onImageUploaded(info: any) {
    console.log('cropper hit');
    this.dataService.UpdateAvator(info);
  }

}
