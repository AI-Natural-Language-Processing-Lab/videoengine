/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { Router } from "@angular/router";
import { select, select$ } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
/* modal popup */
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

// shared services
import { CoreService } from "../../core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { MailTemplatesAPIActions } from "../../../reducers/settings/mailtemplates/actions";
import { fadeInAnimation } from "../../../animations/core";

import { PermissionService } from "../../../admin/users/services/permission.service";
import { ContentTypes } from "../../../configs/settings";

@Component({
  templateUrl: "./mailtemplates.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class MailTemplatesComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private modalService: NgbModal,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    public permission: PermissionService,
    private actions: MailTemplatesAPIActions,
    private router: Router
  ) {}

  @select(["mailtemplates", "filteroptions"])
  readonly filteroptions$: Observable<any>;

  @select(["mailtemplates", "itemsselected"])
  readonly isItemSelected$: Observable<any>;

  @select(["mailtemplates", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["mailtemplates", "records"])
  readonly records$: Observable<any>;

  @select(["mailtemplates", "pagination"])
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
  MailTypes: any = [];
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
        for (const prop in configs.general.mailtemplates) {
          this.MailTypes.push({
             id: configs.general.mailtemplates[prop],
             title: prop
          })
        }
      }
    });
    this.SearchOptions = this.settingService.getSearchOptions();
    this.SearchOptions.showSearchPanel = true;
    this.SearchOptions.actions = this.settingService.initialize_actions();

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
        for (const prop in this.Configs.general.mailtemplates) {
          if (options.type.toString() === "-1") {
            this.ToolbarOptions.left_options[0].title = "[All]";
          } else if (this.Configs.general.mailtemplates[prop] === options.type) {
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
    switch (selection.action) {
      case "add":
        this.AddRecord();
        return;
      case "f_type":
        this.actions.applyFilter({ attr: "type", value: selection.value });
        break;
      case "m_markas":
        this.ProcessActions(selection.value);
        return;
      case "pagesize":
        this.actions.applyFilter({ attr: "pagesize", value: selection.value });
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

  /* Add Record */
  AddRecord() {
    // this.router.navigate(['/settings/mailtemplates/process/']);
    this.router.navigate(["/settings/mailtemplates/process/"]);
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
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
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
      type: 0, // 0: my , 1: favorites, 2: liked, 3: playlist
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
