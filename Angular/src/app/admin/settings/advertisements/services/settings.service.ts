/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";

import * as OPTIONS from "../advertisement.model";
import { iUploadOptions } from "../../../core/core.model";
import { AppConfig } from "../../../../configs/app.config";
import {
  ThemeCSS
} from "../../../../configs/themeSettings";
import { CoreService } from "../../../core/coreService";
@Injectable()
export class SettingsService {
  // configurations
  private apiOptions: OPTIONS.IAPIOptions;
  private uploadOptions: iUploadOptions;
  private toolbarOptions: any;
  private searchOptions: any;

  constructor(private coreService: CoreService, public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/ads/load",
      getinfo: APIURL + "api/ads/getinfo",
      action: APIURL + "api/ads/action",
      proc: APIURL + "api/ads/proc"
    };

    this.init_toolbar_options();
    this.init_search_options();
  }

  init_toolbar_options() {
    this.toolbarOptions = {
      showtoolbar: true,
      showsecondarytoolbar: false,
      showcheckAll: true,
      navbarcss: ThemeCSS.NAVBAR_CSS,
      left_options: [],
      left_caption: "Filter:",
      right_caption: "",
      right_options: [],
      actions: []
    };

    this.toolbarOptions.left_options.push(
      {
        title: "Adult",
        ismultiple: false,
        icon: "", // icon-sort-amount-desc position-left
        value: 1,
        clickevent: "f_type",
        tooltip: "Load Adult Ads",
        isclick: true
      },
      {
        title: "Non Adult",
        ismultiple: false,
        icon: "", // icon-sort-amount-desc position-left
        value: 0,
        clickevent: "f_type",
        tooltip: "Load Adult Ads",
        isclick: true
      }
    );

    this.toolbarOptions.right_options.push({
      title: "Order",
      ismultiple: true,
      position: "right",
      icon: "icon-sort-by-order-alt position-left",
      Options: [
        {
          id: "0",
          title: "ID",
          value: "id",
          isclick: true,
          clickevent: "orderby",
          tooltip: "Order by id"
        },
        {
          id: "1",
          title: "Type",
          value: "type",
          isclick: true,
          clickevent: "orderby",
          tooltip: "Order by type"
        }
      ]
    });
  }

  init_search_options() {
    this.searchOptions = {
      showpanel: true, // show, hide whole panel
      showSearchPanel: true,
      showAdvanceSearchLink: false,
      term: "",
      topselectioncheck: true,
      filters: [],
      dropdownFilters: [],
      checkFilters: [],
      categories: [],
      selectedcategory: "",
      singleaction: false,
      navList: this.coreService.getSettingsNavList(),
      actions: []
    };
  }

  getApiOptions() {
    return this.apiOptions;
  }

  getUploadOptions() {
    return this.uploadOptions;
  }

  getToolbarOptions() {
    return this.toolbarOptions;
  }

  getSearchOptions() {
    return this.searchOptions;
  }
}
