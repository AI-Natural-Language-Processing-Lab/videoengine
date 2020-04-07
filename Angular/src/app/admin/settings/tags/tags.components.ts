/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

// shared services
import { CoreService } from "../../core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { TagsAPIActions } from "../../../reducers/settings/tags/actions";
import { fadeInAnimation } from "../../../animations/core";

import { PermissionService } from "../../../admin/users/services/permission.service";


@Component({
  templateUrl: "./tags.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class TagsComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    public permission: PermissionService,
    private actions: TagsAPIActions
  ) {}

  @select(["tags", "filteroptions"])
  readonly filteroptions$: Observable<any>;

  @select(["tags", "itemsselected"])
  readonly isItemSelected$: Observable<any>;

  @select(["tags", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["tags", "records"])
  readonly records$: Observable<any>;

  @select(["tags", "pagination"])
  readonly pagination$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete

  SearchOptions: any;
  ToolbarOptions: any;

  SelectedItems: any; // selected items in list by check / uncheck options
  isItemsSelected = false; // check the isenabled of items there or not
  FilterOptions: any; // local copy of observable query filters
  Records = 0;
  Pagination: any = {};
  Configs: any = {};
  TagTypes: any = [];
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

    this.configs$.subscribe((configs: any) => {
      this.Configs = configs;
      if (configs.general !== undefined) {
        for (const prop in configs.general.tag) {
          this.TagTypes.push({
             id: configs.general.tag[prop],
             title: prop
          })
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

      if (this.Configs.general !== undefined) {
        for (const prop in this.Configs.general.tag) {
          if (this.Configs.general.tag[prop] === options.type) {
            this.ToolbarOptions.left_options[1].title = "[" + prop + "]";
          }
        }
      }
    });

    this.records$.subscribe(records => {
      this.Records = records;
    });

    this.pagination$.subscribe(pagination => {
      this.Pagination = pagination;
    });

    this.isloaded$.subscribe((loaded: boolean) => {
      if (!loaded) {
        this.dataService.LoadRecords(this.FilterOptions);
      } else {
        // loaded data from reducer store (cache)
        // update pagination (records & pagesize on load)
        this.refreshStats();
      }
    });
    this.isItemSelected$.subscribe((selectedItems: boolean) => {
      this.isItemsSelected = selectedItems;
    });
  }

  /* toolbar actions */
  toolbaraction(selection: any) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    switch (selection.action) {
      case "m_markas":
        this.ProcessActions(selection.value);
        return;
      case "f_type":
        this.actions.applyFilter({ attr: "type", value: selection.value });
        break;
      case "f_ttype":
        this.actions.applyFilter({ attr: "tag_type", value: selection.value });
        break;
      case "f_tlevel":
        this.actions.applyFilter({ attr: "tag_level", value: selection.value });
        break;
      case "f_status":
        this.actions.applyFilter({ attr: "isenabled", value: selection.value });
        break;
      case "pagesize":
        this.actions.applyFilter({ attr: "pagesize", value: selection.value });
    }
  }

  /* find records event trigger */
  FindRecords(filters: any) {
    const _filterOptions = filters.filters;
    _filterOptions.pagenumber = 1;
    _filterOptions.track_filter = true; // to force triggering load event via obvervable subscription
    this.actions.updateFilterOptions(_filterOptions);
  }

  getSelectedItems(arr: any) {
    this.SelectedItems = arr;
    if (this.SelectedItems.length > 0) {
      this.isItemsSelected = true;
    } else {
      this.isItemsSelected = false;
    }
  }

  ProcessActions(selection: any) {
    console.log("process action triggered");
    console.log(selection);

    if (this.SelectedItems.length > 0) {
      for (const item of this.SelectedItems) {
        item.actionstatus = selection.actionstatus;
        if (selection.value === "delete" || selection.value === "deleteall") {
          item.isdeleted = true;
        } else {
          // update item
          // _updateItem(item, isenabled);
        }
      }
      //if (isenabled === 'deleteall') {
      /*$scope.Data = [];
            selectedItems.push({
                id: 1, // dummy id
                actionstatus: isenabled
            });*/
      //}
      this.dataService.ProcessActions(this.SelectedItems, selection);
    }
  }

  refreshStats() {
    this.actions.refresh_pagination({
      type: 0,
      totalrecords: this.Records,
      pagesize: this.FilterOptions.pagesize
    });
    // refresh list states
    this.coreActions.refreshListStats({
      totalrecords: this.Records,
      pagesize: this.FilterOptions.pagesize,
      pagenumber: this.Pagination.currentPage
    });
  }
}
