/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit } from "@angular/core";
import { AppNavigation } from "../../../configs/settings";
import { Router, NavigationEnd, ActivatedRoute } from "@angular/router";
import "rxjs/add/operator/filter";
import "rxjs/add/operator/map";
import "rxjs/add/operator/mergeMap";
import { ConfigDataService } from "../../../configs/services/data.service";
import { TranslateService } from "@ngx-translate/core";
@Component({
  selector: "app-account-leftnavigation",
  templateUrl: "./left-navigation.html",
  providers: [ConfigDataService]
})
export class AccountLeftNavigationComponent implements OnInit {
  MyAccount_Menu: any = AppNavigation.MYACCOUNT_SETTINGS;
  leftMenuIndex = 0;
  topMenuIndex = 0;
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public translate: TranslateService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .map(() => this.activatedRoute)
      .map(route => {
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route;
      })
      .filter(route => route.outlet === "primary")
      .mergeMap(route => route.data)
      .subscribe(event => {
        if (event["leftmenuIndex"] !== undefined) {
          this.leftMenuIndex = event["leftmenuIndex"];
        }
        if (event["topmenuIndex"] !== undefined) {
          this.topMenuIndex = event["topmenuIndex"];
        }
        this.initMenus();
      });
  }

  initMenus() {
    switch (this.topMenuIndex) {
      // settings
      case 0:
        this.MyAccount_Menu = AppNavigation.MYACCOUNT_SETTINGS;
        break;
      case 1:
        this.MyAccount_Menu = AppNavigation.MYACCOUNT_VIDEOS;
        break;
    }
  }
}
