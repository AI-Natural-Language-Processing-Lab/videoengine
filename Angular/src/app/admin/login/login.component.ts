/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component,  ViewEncapsulation, OnInit } from "@angular/core";
import { Router } from "@angular/router";
// services
import { SettingsService } from "../../admin/users/services/settings.service";
import { DataService } from "../../admin/users/services/data.service";
import { FormService } from "../../admin/users/services/form.service";

// shared services
import { CoreService } from "../../admin/core/coreService";
import { CoreAPIActions } from "../../reducers/core/actions";

// reducer actions
import { UserAPIActions } from "../../reducers/users/actions";
import { fadeInAnimation } from "../../animations/core";
import { CookieService } from "ngx-cookie-service";

@Component({
  selector: "app-login",
  templateUrl: "./login.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" },
  providers: [
    DataService,
    SettingsService,
    UserAPIActions,
    FormService,
    CookieService
  ]
})
export class LoginComponent implements OnInit {
  controls: any = [];
  showLoader = false;
  submitText = "Login";
  submitCss =
    "btn btn-info btn-lg btn-block text-uppercase ";
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

  ngOnInit() {
    this.controls = this.formService.getLoginControls({
      username: "",
      password: "",
      rememberme: false
    });
  }

  SubmitForm(payload) {
    this.showLoader = true;
    console.log(payload);

    this.dataService.Authenticate(payload).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-error"
          });
        } else {
          if (data.user === undefined) {
            this.coreActions.Notify({
              title: "User Object Missing",
              text: "",
              css: "bg-error"
            });
          }

          this.coreActions.Notify({
            title: "Authenticated...",
            text: "",
            css: "bg-success"
          });

          const Auth_Obj = {
            userid: data.user.userid,
            username: data.user.username,
            email: data.user.email,
            picturename: data.user.img_url,
            firstname: data.user.firstname,
            lastname: data.user.lastname
          };

          if (payload.rememberme) {
            // save user id in cookie
            this.cookieService.set("_AUTH", JSON.stringify(Auth_Obj));
          }
          const obj = {
            isAuthenticated: true,
            User: Auth_Obj
          };

          this.actions.Authenticate(obj);
          // redirect
          this.router.navigate(["dashboard"]);
          // document.location.href = '/';
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
