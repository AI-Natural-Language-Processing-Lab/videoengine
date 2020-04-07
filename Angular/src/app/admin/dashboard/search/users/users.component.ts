/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit } from "@angular/core";
// redux
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
// services
import { SettingsService } from "../../../../admin/users/services/settings.service";
import { DataService } from "../../../../admin/users/services/data.service";
import { IFilterOption } from "../../../../reducers/users/model";
import { AppConfig } from "../../../../configs/app.config";

@Component({
  selector: "app-users-search",
  templateUrl: "./users.html",
  providers: [SettingsService, DataService]
})
export class UsersSearchComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    public config: AppConfig
  ) {}

  // artists have not associated with categories in this version
  /*@select(["artists", "categories"])
  readonly categories$: Observable<any>;*/

  PublicView = true;
  NoRecordText = "No Search Result!";
  SearchOptions: any;
  TopSearchOptions: any;
  FilterOptions: any = IFilterOption;
  ToolbarOptions: any;

  ngOnInit() {
    // Left Search
    // this.SearchOptions = this.settingService.getSearchSettings();
    // Top Search
    this.TopSearchOptions = this.settingService.getTopSearchSettings();
    // User entered search term
    this.FilterOptions.term = this.config.getGlobalVar("searchparams").term;
    this.FilterOptions.ispublic = true;
    // toolbar options
    this.ToolbarOptions = this.settingService.getToolbarOptions();
    this.ToolbarOptions.showtoolbar = false; // hide top navigation (mostly needed with left side navigation for additional order / filter options)
    this.ToolbarOptions.showcheckAll = false; // remove check all checkbox from search results. (needed in account listings)

    /*this.categories$.subscribe(categories => {
      for (const category of categories) {
        // load categories on left side navigation if navigation exist
        /*
        this.SearchOptions.categories.push({
          key: category.id,
          value: category.title
        });
        */

        // load categories on top filter options if required
      /*  for (let option of this.TopSearchOptions.NavList) {
          if (option.title === "Categories") {
            option.options.push({
              value: category.id,
              title: category.title
            });
          }
        }
      }
    });*/

    this.loadRecords(this.FilterOptions);
  }

  Search(filterOption: any) {
    this.loadRecords(filterOption);
  }

  loadRecords(options: any) {
    this.dataService.LoadRecords(options);
  }

  /* toolbar actions */
  toolbaraction(selection: any) {
    switch (selection.action) {
      case "order":
        this.FilterOptions.order = selection.value;
        break;
      case "paginate":
        console.log("paginate " + selection.value);
        this.FilterOptions.pagenumber = selection.value;
        break;
    }
    this.loadRecords(this.FilterOptions);
  }
}
