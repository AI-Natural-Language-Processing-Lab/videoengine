
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, ViewEncapsulation, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
import { FormService } from "../../admin/users/services/form.service";
import { DataService } from "../../admin/users/services/data.service";
import { SettingsService } from "../../admin/users/services/settings.service";
// shared services
import { CoreAPIActions } from "../../reducers/core/actions";
import { CoreService } from "../../admin/core/coreService";

// reducer actions
import { UserAPIActions } from "../../reducers/users/actions";
import { CookieService } from "ngx-cookie-service";

@Component({
  selector: "app-profilesetup",
  templateUrl: "./profilesetup.html",
  encapsulation: ViewEncapsulation.None,
  providers: [
    DataService,
    SettingsService,
    UserAPIActions,
    FormService,
    CookieService,
    CoreService
  ]
})

export class ProfileSetupComponent implements OnInit {
  controls: any = [];
  showLoader = true;
  submitText = "Save Changes";
  User: any = {};
  uploadedFiles = [];
  constructor(
    private dataService: DataService,
    private coreActions: CoreAPIActions,
    private formService: FormService,
    private router: Router,
    private coreService: CoreService
  ) {}
  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  ngOnInit() {
    this.auth$.subscribe(Info => {

      // load extended information including dynamic profile data to edi
      this.LoadInfo(Info.User.id);
      /*this.User = Info.User;
      // dynamic attribute processing
      console.log(this.User);
      this.coreService.prepareDynamicControlData(this.User);

      this.initializeControls(Info.User);
      this.showLoader = false;
      */
    });
  }

  LoadInfo(UserId: string) {
    this.showLoader = true;
    this.dataService.GetInfo(UserId).subscribe((data: any) => {
      if (data.status === "error") {
        this.coreActions.Notify({
          title: data.message,
          text: "",
          css: "bg-success"
        });
        // redirect to user page
        this.router.navigate(["/"]);
      } else {
        this.User = data.post;

        // dynamic attribute processing
        this.coreService.prepareDynamicControlData(this.User);

        this.initializeControls(this.User);
      }
      this.showLoader = false;
    });
  }

  initializeControls(data: any) {
    this.controls = this.formService.getControls(data, 2, false);
  }

  SubmitForm(payload) {
    for (const prop of Object.keys(this.User)) {
      for (const payload_prop of Object.keys(payload)) {
        if (prop === payload_prop) {
          this.User[prop] = payload[payload_prop];
        }
      }
    }

    this.User.attr_values = this.coreService.processDynamicControlsData(payload, this.User);
    // skip admin related additional edit options
    this.User.isadmin = false;

    // console.log(this.User);

    this.User.viewType = 2; // edit profile
 
    this.showLoader = true;
    this.dataService.AddRecord(this.User).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-danger"
          });
        } else {
          const message = "Profile Updated";
          this.coreActions.Notify({
            title: message,
            text: "",
            css: "bg-success"
          });

          this.router.navigate(["/"]);
        }
        this.showLoader = false;
      },
      err => {
        this.showLoader = false;
        this.coreActions.Notify({
          title: "Error Occured",
          text: "",
          css: "bg-danger"
        });
      }
    );
  }
}
