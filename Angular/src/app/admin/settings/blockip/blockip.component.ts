/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, ViewEncapsulation, OnInit } from "@angular/core";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
/* modal popup */
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";

// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

// modal popup
import { ViewComponent } from "./partials/modal.component";

// shared services
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { BlockIPAPIActions } from "../../../reducers/settings/blockip/actions";
import { fadeInAnimation } from "../../../animations/core";

import { PermissionService } from "../../../admin/users/services/permission.service";

@Component({
  templateUrl: "./blockip.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class BlockIPComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private modalService: NgbModal,
    public permission: PermissionService,
    private coreActions: CoreAPIActions,
    private actions: BlockIPAPIActions
  ) {}

  @select(["blockip", "filteroptions"])
  readonly filteroptions$: Observable<any>;

  @select(["blockip", "itemsselected"])
  readonly isItemSelected$: Observable<any>;

  @select(["blockip", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["blockip", "records"])
  readonly records$: Observable<any>;

  @select(["blockip", "pagination"])
  readonly pagination$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete

  heading = "Block IP";
  subheading = "Management";
  SearchOptions: any;
  ToolbarOptions: any;

  SelectedItems: any; // selected items in list by check / uncheck options
  isItemsSelected = false; // check the isenabled of items there or not
  FilterOptions: any; // local copy of observable query filters
  Records = 0;
  Pagination: any = {};
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
      case "add":
        this.AddRecord();
        return;
      case "m_markas":
        this.ProcessActions(selection.value);
        return;
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
    const _options: NgbModalOptions = {
      backdrop: false
    };
    const modalRef = this.modalService.open(ViewComponent, _options);
    modalRef.componentInstance.Info = {
      title: "Block IP Address",
      data: this.settingService.getInitObject()
    };
    modalRef.result.then(
      result => {
        this.actions.addRecord({
          id: 0,
          ipaddress: result.data.ipaddress,
          created_at: new Date()
        });
        // this.closeResult = `Closed with: ${result}`;
      },
      dismissed => {
        console.log("dismissed");
      }
    );
  }

  getSelectedItems(arr: any) {
    this.SelectedItems = arr;
    let _selection = false;
    if (this.SelectedItems.length > 0) {
      _selection = true;
    }
    this.actions.updateItemsSelectionStatus(_selection);
    console.log("items selected");
    console.log(this.SelectedItems);
  }

  ProcessActions(selection: any) {
    if (this.SelectedItems.length > 0) {
      for (const item of this.SelectedItems) {
        item.actionstatus = selection.actionstatus;
      }
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
