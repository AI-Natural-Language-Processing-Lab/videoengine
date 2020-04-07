/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { UserAPIActions } from "../../../reducers/users/actions";
import { CookieService } from "ngx-cookie-service";
import { HttpClient } from "@angular/common/http";
import { SettingsService } from "./settings.service";
import { CoreAPIActions } from "../../../reducers/core/actions";
import { AppConfig } from "../../../configs/app.config";

// Actions
import { VideoAPIActions } from "../../../reducers/videos/actions";

@Injectable()
export class UserService {
  constructor(
    public config: AppConfig,
    private settings: SettingsService,
    private http: HttpClient,
    private actions: UserAPIActions,
    private cookieService: CookieService,
    private coreActions: CoreAPIActions,
    private videoActions: VideoAPIActions
  ) {}

  SignOut() {
    this.cookieService.delete("_AUTH");
    this.actions.SignOut();
  }
  CheckAuthentication() {
    const _auth = this.cookieService.get("_AUTH");
    if (_auth !== undefined && _auth !== "") {
      // authenticated (authenticate until user full data receiving)
      this.actions.Authenticate({
        isAuthenticated: true,
        User: JSON.parse(_auth)
      });
      this.coreActions.Notify({
        title: "Logged in successfully",
        text: "",
        css: "bg-success"
      });
    }
  }

  AuthorizeUser(userid: string) {
    this.GetInfo(userid).subscribe((data: any) => {
      if (data.status === "error") {
        this.coreActions.ErrorNotify({
          title: data.message,
        });
        this.coreActions.toggleLoader(false);
      } else {
        if (this.config.getGlobalVar("apptype") == "account") {
          // my account
          const obj = {
            isAuthenticated: true,
            User: data.post,
            Token: data.token,
            Role: []
          };
          this.actions.Authenticate(obj);
          // apply user filter
          this.videoActions.updateFilter(obj.User);
        } else {
          // admin account
          this.actions.Authenticate({
            isAuthenticated: true,
            User: data.post,
            Token: data.token,
            Role: this.ExtractAccessIDs(data.role)
          });
        }
        // disable global loader
        this.coreActions.toggleLoader(false);
      }
    });
  }

  GetInfo(userid: string) {
    let URL = this.settings.getApiOptions().getinfo_auth;
    let isadmin = false;
    if (this.config.getGlobalVar("apptype") == "admin") {
       isadmin = true;
    }
    return this.http.post(URL, JSON.stringify({ id: userid, isadmin: isadmin }));
  }

  /* extract and return only array of accessids */
  ExtractAccessIDs(Role: any) {
    if (Role.length === 0) {
      return [];
    }
    if (Role[0].permissions.length === 0) {
      return [];
    }
    const AccessIds: any = [];
    const perms = Role[0].permissions;
    for (const item of perms) {
      AccessIds.push(item.robject.uniqueid);
    }
    return AccessIds;
  }
}
