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
  selector: "app-manageaccount",
  templateUrl: "./manageaccount.html",
  encapsulation: ViewEncapsulation.None,
  providers: [
    DataService,
    SettingsService,
    UserAPIActions,
    FormService,
    CookieService
  ]
})
export class ManageAccountComponent implements OnInit {
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
    this.controls = this.formService.getControls(data, 4, false);
  }

  deleteAccount() {
    const r = confirm("Are you sure you want to delete your account?");
    if (r === true) {
      this.dataService.DeleteAccount(this.User).subscribe(
        (data: any) => {
          if (data.status === "error") {
            this.coreActions.Notify({
              title: data.message,
              text: "",
              css: "bg-danger"
            });
          } else {
            const message = "Account Deleted";
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

  SubmitForm(payload) {
   
    this.User.cpassword = payload.cpassword;
    this.User.opassword = payload.opassword;
    this.User.password = payload.password;
    this.User.viewType = 4; // change password
    if (this.User.password !== this.User.cpassword) {
      this.coreActions.Notify({
        title: "Confirm Password Not Matched",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    // skip admin related additional edit options
    this.User.isadmin = false;
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
          const message = "Password changed successfully";
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
