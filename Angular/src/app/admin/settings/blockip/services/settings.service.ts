/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as OPTIONS from "../blockip.model";
import { iUploadOptions } from "../../../core/core.model";
import { AppConfig } from "../../../../configs/app.config";
import { CoreService } from "../../../core/coreService";
import {
  ButtonCSS,
  ICONCSS,
  ThemeCSS
} from "../../../../configs/themeSettings";

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
      load: APIURL + "api/ip/load",
      getinfo: APIURL + "api/ip/getinfo",
      action: APIURL + "api/ip/action",
      proc: APIURL + "api/ip/proc"
    };

    this.init_toolbar_options();

    this.init_search_options();
  }

  init_search_options() {
    this.searchOptions = {
      showpanel: true, // show, hide whole panel
      showSearchPanel: true,
      showAdvanceSearchLink: false,
      term: "",
      topselectioncheck: true,
      navList: this.coreService.getSettingsNavList(),
      filters: [],
      dropdownFilters: [],
      checkFilters: [],
      categories: [],
      selectedcategory: "",
      singleaction: true,
      actions: [
        {
          id: 1,
          title: "Add",
          tooltip: "Block new ip adress",
          row: 1,
          icon: "icon-file-plus",
          options: {},
          css: "btn m-b-5 btn-block btn-success",
          event: "add"
        }
      ]
    };
  }

  init_toolbar_options() {
    this.toolbarOptions = {
      showtoolbar: true,
      showsecondarytoolbar: true,
      showcheckAll: false,
      navbarcss: ThemeCSS.NAVBAR_CSS,
      left_options: [],
      left_caption: "",
      right_caption: "",
      right_options: [],
      actions: []
    };

    this.toolbarOptions.right_options.push(
      this.coreService.getPaginationSettings()
    );

    this.toolbarOptions.actions.push({
      title: "Mark As",
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "2",
          title: "Delete",
          value: 0,
          actionstatus: "delete",
          css: ButtonCSS.DANGER_BUTTON,
          attr: "",
          isclick: true,
          clickevent: "m_markas",
          icon: ICONCSS.DELETE_ICON,
          tooltip: "Delete selected records"
        }
      ]
    });
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
  /* Initialize object for adding new record */
  getInitObject(): OPTIONS.IBlockEntity {
    return {
      id: 0,
      ipaddress: ""
    };
  }
}
