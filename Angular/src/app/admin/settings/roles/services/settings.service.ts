/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as OPTIONS from "../roles.model";
import { iUploadOptions } from "../../../core/core.model";
import { AppConfig } from "../../../../configs/app.config";
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
      load_roles: APIURL + "api/role/load",
      load_objects: APIURL + "api/roleobject/load",
      getinfo: APIURL + "api/role/getinfo",
      add_role: APIURL + "api/role/proc",
      add_object: APIURL + "api/roleobject/proc",
      delete_role: APIURL + "api/role/action",
      delete_object: APIURL + "api/roleobject/action",
      update_permission: APIURL + "api/rolepermission/proc"
    };

    this.init_search_options();

    // this.init_toolbar_options();
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
  getInitRoleObject(): OPTIONS.RoleEntity {
    return {
      id: 0,
      rolename: ""
    };
  }

  getInitRoleObjectObject(): OPTIONS.RoleObjectEntity {
    return {
      id: 0,
      objectname: "",
      description: "",
      uniqueid: ""
    };
  }
}
