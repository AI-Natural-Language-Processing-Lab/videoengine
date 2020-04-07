/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  Component,
  OnInit,
  ViewEncapsulation,
  AfterViewInit
} from "@angular/core";
import { Router } from "@angular/router";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
/* modal popup */
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";

// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

// modal popup
import { ViewComponent } from "./partials/modal";

// shared services
import { CoreAPIActions } from "../../reducers/core/actions";
import { fadeInAnimation } from "../../animations/core";
import { UserAPIActions } from "../../reducers/users/actions";
import { PermissionService } from "../../admin/users/services/permission.service";

@Component({
  templateUrl: "./users.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class UsersComponent implements OnInit, AfterViewInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private modalService: NgbModal,
    private coreActions: CoreAPIActions,
    public permission: PermissionService,
    private actions: UserAPIActions,
    private router: Router,
  ) {}

  @select(["users", "filteroptions"])
  readonly filteroptions$: Observable<any>;

  @select(["users", "categories"])
  readonly categories$: Observable<any>;

  @select(["users", "itemsselected"])
  readonly isItemSelected$: Observable<any>;

  @select(["users", "isloaded"])
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
    this.SearchOptions = this.settingService.getSearchOptions();
    this.ToolbarOptions = this.settingService.getToolbarOptions();
    this.ToolbarOptions.showcheckAll = true;
    // user authentication & access right management
    // full resource access key and readonly key can be generated via roles management
    this.auth$.subscribe((auth: any) => {
      const FullAccessID = "1521143362403";
      const ReadOnlyAccessID = "1521143407965";
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
    this.isItemSelected$.subscribe((selectedItems: boolean) => {
      this.isItemsSelected = selectedItems;
    });
  }

  ngAfterViewInit() {
    // console.log('after view init called');
  }

  selectAll(selectall: boolean) {
    this.actions.selectAll(selectall);
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
        this.Trigger_Modal({
          title: "Create Account",
          data: this.settingService.getInitObject(),
          viewType: 1
        });
        return;
      case "settings":
        this.router.navigate(["/users/settings"]);
        return;
      case "m_markas":
        this.ProcessActions(selection.value);
        return;
      case "m_filter":
        this.actions.applyFilter({
          attr: "datefilter",
          value: selection.value
        });
        break;
      case "sort":
        this.actions.applyFilter({ attr: "direction", value: selection.value });
        break;
    }
  }

  /* find records event trigger */
  FindRecords(filters: any) {
    const _filterOptions = filters.filters;
    _filterOptions.pagenumber = 1;
    _filterOptions.track_filter = true; // to force triggering load event via obvervable subscription
    if (
      _filterOptions.categoryname !== null &&
      _filterOptions.categoryname !== ""
    )
      _filterOptions.accounttype = _filterOptions.categoryname;
    this.actions.updateFilterOptions(_filterOptions);
  }

  Trigger_Modal(InstanceInfo: any) {
    const _options: NgbModalOptions = {
      backdrop: false
    };
    const modalRef = this.modalService.open(ViewComponent, _options);
    modalRef.componentInstance.Info = InstanceInfo;
    modalRef.result.then(
      result => {
        // this.closeResult = `Closed with: ${result}`;
      },
      dismissed => {
        console.log("dismissed");
      }
    );
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
    if (this.SelectedItems.length > 0) {
      for (const item of this.SelectedItems) {
        item.actionstatus = selection.actionstatus;
      }
      this.dataService.ProcessActions(this.SelectedItems, selection);
    }
  }
}
