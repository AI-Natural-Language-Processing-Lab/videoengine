
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import { Injectable } from "@angular/core";
import * as OPTIONS from "../categories.model";
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
  // configurations
  private apiOptions: OPTIONS.IAPIOptions;
  private uploadOptions: iUploadOptions;
  private toolbarOptions: any;
  private searchOptions: any;

  // Application Configuration Data
  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;

  Configs: any = {};
  constructor(private coreService: CoreService, public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/categories/load",
      load_dropdown: APIURL + "api/categories/load_dropdown",
      getinfo: APIURL + "api/categories/getinfo",
      action: APIURL + "api/categories/action",
      proc: APIURL + "api/categories/proc"
    };

    this.configs$.subscribe((configs: any) => {
        if (configs.general !== undefined) {
            console.log('configs defined');
            this.Configs = configs.general.category;
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
      filters: [],
      dropdownFilters: [],
      checkFilters: [],
      categories: [],
      selectedcategory: "",
      singleaction: true,
      actions: this.initialize_actions()
    };
  }
  initialize_actions() {
    return [
      {
        id: 1,
        title: "Add Category",
        tooltip: "Add new category",
        row: 1,
        icon: "icon-file-plus",
        options: {},
        css: "btn m-b-5 btn-block btn-success",
        event: "add"
      }
    ];
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
      title: "Type",
      ismultiple: true,
      icon: "", // icon-sort-amount-desc position-left
      Options: [
        {
          id: "1",
          title: "Show All",
          value: -1,
          isclick: true,
          clickevent: "f_reset",
          tooltip: "Show all items"
        },
        { id: "2", separator: true }
      ]
    });

  
    const categories: any = [];
    for (const prop in this.Configs) {
      categories.push({
         id: this.Configs[prop],
         title: prop
      })
    }

    for (const type of categories) {
      this.toolbarOptions.left_options[0].Options.push({
        id: "0",
        title: type.title,
        value: type.id,
        isclick: true,
        clickevent: "f_type",
        tooltip: "Load " + type.title + " templates"
      });
    }

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
          title: "Enable",
          value: 1,
          actionstatus: "enable",
          attr: "isenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Enable selected records"
        },
        {
          id: "2",
          title: "Disable",
          value: 0,
          actionstatus: "disable",
          attr: "isenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Disable selected records"
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
  getInitObject(): OPTIONS.CategoriesEntity {
    return {
      id: 0,
      title: "",
      term: "",
      description: "",
      parentid: 0,
      priority: 0,
      mode: 0,
      isenabled: 1,
      type: 0,
      picturename: "",
      icon: "",
      files: [],
      img_url: ""
    };
  }
}
