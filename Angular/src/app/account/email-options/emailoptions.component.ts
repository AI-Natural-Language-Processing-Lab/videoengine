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
import { CoreService } from "../../admin/core/coreService";
import { CoreAPIActions } from "../../reducers/core/actions";

// reducer actions
import { UserAPIActions } from "../../reducers/users/actions";
import { CookieService } from "ngx-cookie-service";

@Component({
  selector: "app-emailoptions",
  templateUrl: "./email.options.html",
  encapsulation: ViewEncapsulation.None,
  providers: [
    DataService,
    SettingsService,
    UserAPIActions,
    FormService,
    CookieService
  ]
})
export class EmailOptionsComponent implements OnInit {
  controls: any = [];
  showLoader = true;
  submitText = "Save Changes";
  User: any = {};

  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: UserAPIActions,
    private formService: FormService,
    private router: Router,
    private cookieService: CookieService
  ) {}
  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  ngOnInit() {
    this.auth$.subscribe(Info => {
      this.User = Info.User;
      this.initializeControls(Info.User);
      this.showLoader = false;
    });
  }

  initializeControls(data: any) {
    this.controls = this.formService.getControls(data, 3,false);
  }

  SubmitForm(payload) {
    this.User.password = payload.password;
    this.User.email = payload.email;
    this.User.viewType = 3; // change email
    this.showLoader = true;
    // skip admin related additional edit options
    this.User.isadmin = false;

    this.dataService.AddRecord(this.User).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-danger"
          });
        } else {
          const message = "Email change request submitted successfully";
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
