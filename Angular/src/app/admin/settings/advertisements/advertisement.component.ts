/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit } from "@angular/core";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

// reducer actions
import { AdvertisementAPIActions } from "../../../reducers/settings/advertisements/actions";
import { PermissionService } from "../../../admin/users/services/permission.service";

@Component({
  templateUrl: "./advertisement.html"
})
export class AdvertisemntComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    public permission: PermissionService,
    private actions: AdvertisementAPIActions
  ) {}

  @select(["advertisement", "filteroptions"])
  readonly filteroptions$: Observable<any>;

  @select(["advertisement", "categories"])
  readonly categories$: Observable<any>;

  @select(["advertisement", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete

  SearchOptions: any;
  ToolbarOptions: any;
  SelectedItems: any; // selected items in list by check / uncheck options
  isItemsSelected = false; // check the isenabled of items there or not
  FilterOptions: any; // local copy of observable query filters

  ngOnInit() {
    // user authentication & access right management
    // full resource access key and readonly key can be generated via roles management
    this.auth$.subscribe((auth: any) => {
      const FullAccessID = "1521396255768";
      const ReadOnlyAccessID = "1521396280060";
      if (
        this.permission.GrandResourceAccess(
          false,
          FullAccessID,
          ReadOnlyAccessID,
          auth.Role
        )
      ) {
        this.isAccessGranted = true;
        if (this.permission.GrandResourceAction(FullAccessID, auth.Role)) {
          this.isActionGranded = true;
        }
      }
    });
    this.SearchOptions = this.settingService.getSearchOptions();
    this.ToolbarOptions = this.settingService.getToolbarOptions();

    this.filteroptions$.subscribe(options => {
      this.FilterOptions = options;
      if (options.track_filter) {
        this.dataService.LoadRecords(options);
        // reset track filter to false again
        options.track_filter = false;
        this.actions.updateFilterOptions(options);
      }
    });
    this.isloaded$.subscribe((loaded: boolean) => {
      if (!loaded) {
        this.dataService.LoadRecords(this.FilterOptions);
      }
    });
  }

  /* toolbar actions */
  toolbaraction(selection: any) {
    switch (selection.action) {
      case "f_type":
        this.actions.applyFilter({ attr: "type", value: selection.value });
        break;
    }
  }

  /* find records event trigger */
  FindRecords(filters: any) {
    const _filterOptions = filters.filters;
    _filterOptions.pagenumber = 1;
    _filterOptions.track_filter = true; // to force triggering load event via obvervable subscription
    this.actions.updateFilterOptions(_filterOptions);
  }
}
