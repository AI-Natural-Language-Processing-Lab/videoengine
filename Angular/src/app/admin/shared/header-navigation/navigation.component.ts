/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component } from "@angular/core";
import { AppState } from "../../../configs/themeSettings";
import { TranslateService } from "@ngx-translate/core";
import { CookieService } from "ngx-cookie-service";
import { Router } from "@angular/router";
import { select, select$ } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
// reducer actions
import { UserService } from "../../../admin/users/services/auth.service";
import { AppConfig } from "../../../configs/app.config";
import { CoreService } from "../../../admin/core/coreService";
@Component({
  selector: "app-admin-header",
  templateUrl: "./navigation.component.html",
  providers: [UserService]
})
export class NavigationComponent {
  LanguageList: any;
  DefaultLanguage = "en";
  DefaultLanguageComponent: any;

  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  constructor(
    private translate: TranslateService,
    private coreService: CoreService,
    public config: AppConfig,
    private cookieService: CookieService,
    private userService: UserService,
    private router: Router
  ) {
    this.LanguageList = AppState.SUPPORTED_LANGS_EXTENDED;
    this.initialize();
  }

  initialize() {
    this.DefaultLanguage = AppState.DEFAULT_LANG;
    for (const lang of this.LanguageList) {
      if (lang.culture === this.DefaultLanguage) {
        this.DefaultLanguageComponent = lang;
      }
    }
  }

  toggleLanguage(lang: any, event) {
    AppState.DEFAULT_LANG = lang.culture;
    this.cookieService.set("_LANG", lang.culture);
    console.log(AppState.DEFAULT_LANG);
    this.translate.setDefaultLang(lang.culture);
    this.initialize();
    event.stopPropagation();
  }

  signout(event: any) {
    this.userService.SignOut();
    this.router.navigate([""]);
    event.stopPropagation();
  }
}
