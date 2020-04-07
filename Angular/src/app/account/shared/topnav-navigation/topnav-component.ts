/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit } from "@angular/core";
import { Router, NavigationEnd, ActivatedRoute } from "@angular/router";
import "rxjs/add/operator/filter";
import "rxjs/add/operator/map";
import "rxjs/add/operator/mergeMap";

import { ConfigDataService } from "../../../configs/services/data.service";
import { TranslateService } from "@ngx-translate/core";

import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
@Component({
  selector: "app-account-topnavigation",
  templateUrl: "./topnav-component.html",
  providers: [ConfigDataService],
})
export class AccountTopNavigationComponent implements OnInit {
  MyAccount_Menu: any = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private activatedRoute: ActivatedRoute,
    private configdata: ConfigDataService,
    public translate: TranslateService
  ) {}

  topMenuIndex = 0;

  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;

  Configs: any = {};

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
        if (event["topmenuIndex"] !== undefined) {
          this.topMenuIndex = event["topmenuIndex"];
        }
      });

    this.configs$.subscribe((configs: any) => {
      this.Configs = configs;
      this.prepareNavList();
    });
  }

  prepareNavList() {
    if (this.Configs.general) {
      const conf = this.Configs.general.features;

      if (conf.enable_videos) {
        this.MyAccount_Menu.push({
          id: 1,
          title: "Videos",
          value: "/my-videos",
          index: 1,
        });
      }

      /*if (conf.enable_audio) {
        this.MyAccount_Menu.push({
          id: 2,
          title: "Audio",
          value: "/my-audio",
          index: 2,
        });
      }

      if (conf.enable_photos) {
        this.MyAccount_Menu.push({
          id: 3,
          title: "Photos",
          value: "/my-photos",
          index: 3,
        });
      }

      if (conf.enable_qa) {
        this.MyAccount_Menu.push({
          id: 4,
          title: "Q&A",
          value: "/my-qa",
          index: 4,
        });
      }

      if (conf.enable_forums) {
        this.MyAccount_Menu.push({
          id: 5,
          title: "Forums",
          value: "/my-topics",
          index: 5,
        });
      }

      if (conf.enable_polls) {
        this.MyAccount_Menu.push({
          id: 6,
          title: "Polls",
          value: "/my-polls",
          index: 6,
        });
      }

      if (conf.enable_classified) {
        this.MyAccount_Menu.push({
          id: 7,
          title: "Listings",
          value: "/my-listings",
          index: 8,
        });
      }

      if (conf.enable_blogs) {
        this.MyAccount_Menu.push({
          id: 8,
          title: "Blogs",
          value: "/my-blogs",
          index: 7,
        });
      } */

      this.MyAccount_Menu.push({
        id: 9,
        title: "Settings",
        value: "",
        index: 0,
      });
    }
  }
}
