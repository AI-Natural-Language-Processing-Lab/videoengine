/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";

import * as OPTIONS from "../mailtemplates.model";
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
      load: APIURL + "api/mailtemplates/load",
      getinfo: APIURL + "api/mailtemplates/getinfo",
      action: APIURL + "api/mailtemplates/action",
      proc: APIURL + "api/mailtemplates/proc"
    };

    this.configs$.subscribe((configs: any) => {
      if (configs.general !== undefined) {
          console.log('configs defined');
          this.Configs = configs.general.mailtemplates;
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
        title: "Add Template",
        tooltip: "Add new mail template",
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

    const template_types: any = [];
    for (const prop in this.Configs) {
      template_types.push({
         value: this.Configs[prop],
         title: prop
      })
    }
    // supported mailtemplates content types
    for (const type of template_types) {
      this.toolbarOptions.left_options[0].Options.push({
        id: "0",
        title: type.title,
        value: type.value,
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
  getInitObject(): OPTIONS.MailTemplatesEntity {
    return {
      id: 0,
      templatekey: "",
      description: "",
      subjecttags: "",
      subject: "",
      tags: "",
      contents: "",
      type: "0"
    };
  }
}
