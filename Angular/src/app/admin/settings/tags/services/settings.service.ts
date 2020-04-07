/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";

import * as OPTIONS from "../tags.model";
import { iUploadOptions } from "../../../core/core.model";
import { AppConfig } from "../../../../configs/app.config";
import { CoreService } from "../../../core/coreService";
import {
  ButtonCSS,
  ICONCSS,
  ThemeCSS
} from "../../../../configs/themeSettings";
import { Observable } from "rxjs/Observable";
import { select } from "@angular-redux/store";
@Injectable()
export class SettingsService {

  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;

  // configurations
  private apiOptions: OPTIONS.IAPIOptions;
  private uploadOptions: iUploadOptions;
  private toolbarOptions: any;
  private searchOptions: any;
  Configs: any = {};
  constructor(private coreService: CoreService, public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/tags/load",
      getinfo: APIURL + "api/tags/getinfo",
      action: APIURL + "api/tags/action",
      proc: APIURL + "api/tags/proc"
    };

    this.configs$.subscribe((configs: any) => {
        if (configs.general !== undefined) {
            this.Configs = configs.general.tag;
            this.init_toolbar_options();
            this.init_search_options();
        }
    });
  
  }

  init_search_options() {
    this.searchOptions = {
      showpanel: true, // show, hide whole panel
      showSearchPanel: true,
      showAdvanceSearchLink: false,
      term: "",
      topselectioncheck: true,
      navList: this.coreService.getSettingsNavList(),
      filters: [
        {
          id: 1,
          title: "Inactive Tags",
          value: 0,
          default_value: 2,
          selected: false,
          attr: "isenabled"
        }
      ],
      dropdownFilters: [],
      checkFilters: [],
      categories: [],
      selectedcategory: "",
      singleaction: false,
      actions: []
    };
  }

  init_toolbar_options() {
    this.toolbarOptions = {
      showtoolbar: true,
      showsecondarytoolbar: true,
      showcheckAll: false,
      navbarcss: ThemeCSS.NAVBAR_CSS,
      left_options: [],
      left_caption: "Filter:",
      right_caption: "",
      right_options: [],
      actions: []
    };
    this.toolbarOptions.left_options.push({
      title: "Status",
      ismultiple: true,
      icon: "", // icon-sort-amount-desc position-left
      Options: [
        {
          id: "1",
          title: "Show All",
          value: 0,
          isclick: true,
          clickevent: "f_reset",
          tooltip: "Show all items"
        },
        { id: "2", separator: true },
        {
          id: "5",
          title: "Active",
          value: 1,
          isclick: true,
          clickevent: "f_status",
          tooltip: "Load active tags"
        },
        {
          id: "6",
          title: "Inactive",
          value: 0,
          isclick: true,
          clickevent: "f_status",
          tooltip: "Load inactive tags"
        }
      ]
    });

    this.toolbarOptions.left_options.push({
      title: "Type",
      ismultiple: true,
      icon: "", // icon-sort-amount-desc position-left
      Options: []
    });
   
    const tags: any = [];
    for (const prop in this.Configs) {
      tags.push({
         value: this.Configs[prop],
         title: prop
      })
    }
    for (const type of tags) {
      this.toolbarOptions.left_options[1].Options.push({
        id: "0",
        title: type.title,
        value: type.value,
        isclick: true,
        clickevent: "f_type",
        tooltip: "Load " + type.title + " tags"
      });
    }

    this.toolbarOptions.left_options.push({
      title: "Tag Types",
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "1",
          title: "Show All",
          value: 0,
          isclick: true,
          clickevent: "f_reset",
          tooltip: "Show all items"
        },
        { id: "2", separator: true },
        {
          id: "5",
          title: "Normal Tags",
          value: 0,
          isclick: true,
          clickevent: "f_ttype",
          tooltip: "Load normal tags"
        },
        {
          id: "6",
          title: "User Searches",
          value: 1,
          isclick: true,
          clickevent: "f_ttype",
          tooltip: "Load user searches"
        }
      ]
    });

    this.toolbarOptions.left_options.push({
      title: "Tag Level",
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "1",
          title: "Show All",
          value: 0,
          isclick: true,
          clickevent: "f_reset",
          tooltip: "Show all items"
        },
        { id: "2", separator: true },
        {
          id: "5",
          title: "High",
          value: 0,
          isclick: true,
          clickevent: "f_tlevel",
          tooltip: "Load high priority tags"
        },
        {
          id: "6",
          title: "Medium",
          value: 1,
          isclick: true,
          clickevent: "f_tlevel",
          tooltip: "Load medium priority tags"
        },
        {
          id: "6",
          title: "Low",
          value: 2,
          isclick: true,
          clickevent: "f_tlevel",
          tooltip: "Load low priority tags"
        }
      ]
    });
    this.toolbarOptions.right_options.push(
      this.coreService.getPaginationSettings()
    );

    this.toolbarOptions.actions.push({
      title: "Mark As",
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "1",
          title: "Activate",
          value: 1,
          actionstatus: "enable",
          attr: "isenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.DARK_BUTTON,
          tooltip: "Enable selected records"
        },
        {
          id: "2",
          title: "Deactivate",
          value: 0,
          actionstatus: "disable",
          attr: "isenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.DANGER_BUTTON,
          tooltip: "Disable selected records"
        },
        {
          id: "0",
          title: "Top Priority",
          value: 0,
          actionstatus: "high",
          attr: "tag_level",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.DARK_BUTTON,
          tooltip: "Mark as top priority"
        },
        {
          id: "1",
          title: "Medium",
          value: 1,
          actionstatus: "medium",
          attr: "tag_level",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.DARK_BUTTON,
          tooltip: "Mark as medium"
        },
        {
          id: "3",
          title: "Low Priority",
          value: 1,
          actionstatus: "low",
          isclick: true,
          attr: "tag_level",
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.DARK_BUTTON,
          tooltip: "Mark as low priority"
        },
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
}
