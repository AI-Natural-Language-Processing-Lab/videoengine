/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, Input, OnInit } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { Router, NavigationEnd, ActivatedRoute } from "@angular/router";
import "rxjs/add/operator/filter";
import "rxjs/add/operator/map";
import "rxjs/add/operator/mergeMap";
import { TranslateService } from "@ngx-translate/core";
@Component({
  selector: "app-account-breadcrumb",
  templateUrl: "./breadcrumb.component.html"
})
export class AccountBreadcrumbComponent implements OnInit {
  @Input() layout;
  pageInfo;
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
    public translate: TranslateService
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
        this.titleService.setTitle(event["title"]);
        this.pageInfo = event;
      });
  }
}
