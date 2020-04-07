/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";
import { FormService } from "./services/form.service";

// shared services
import { CoreService } from "../../core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { ROLEAPIActions } from "../../../reducers/settings/roles/actions";
import { fadeInAnimation } from "../../../animations/core";
import { ContentTypes } from "../../../configs/settings";
// import { ContentType } from '@angular/http/src/enums';

import { PermissionService } from "../../../admin/users/services/permission.service";

/* modal popup */
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";
import { ViewComponent } from "./partials/modal.component";

@Component({
  templateUrl: "./roles.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class RolesComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: ROLEAPIActions,
    private route: ActivatedRoute,
    private formService: FormService,
    public permission: PermissionService,
    private router: Router,
    private modalService: NgbModal
  ) {}

  SearchOptions: any;
  controls: any = [];
  showLoader = false;
  formHeading = "Manage Roles";
  RoleTypes = ContentTypes.ROLE_TYPES;
  RoleList = [];
  ObjectList = [];
  RoleType = 1;
  FilterOptions: any;
  IsRoleLoaded = false; // check data is loaded for first time
  isObjectLoaded = false;

  @select(["roles", "roles"])
  readonly roles$: Observable<any>;

  @select(["roles", "objects"])
  readonly objects$: Observable<any>;

  @select(["roles", "isroleloaded"])
  readonly isroleloaded$: Observable<any>;

  @select(["roles", "isobjectloaded"])
  readonly isobjectloaded$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete

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
    // setup navigation list
    this.SearchOptions = this.settingService.getSearchOptions();
    this.SearchOptions.showSearchPanel = false;
    this.SearchOptions.actions = [];

    this.roles$.subscribe((roles: any) => {
      this.RoleList = roles;
    });

    this.objects$.subscribe((objects: any) => {
      this.ObjectList = objects;
    });

    this.isroleloaded$.subscribe((loaded: boolean) => {
      this.IsRoleLoaded = loaded;
      if (!this.IsRoleLoaded) {
        this.dataService.LoadRoles();
      }
    });

    this.isobjectloaded$.subscribe((loaded: boolean) => {
      this.isObjectLoaded = loaded;
      if (!this.isObjectLoaded) {
        this.dataService.LoadObjects();
      }
    });
  }

  filterCategories(obj, event) {
    this.RoleType = obj.value;
    console.log("obj value " + obj.value);
    if (this.RoleType === 1) {
      this.formHeading = "Manage Roles";
    } else {
      this.formHeading = "Manage Role Objects";
    }
    event.stopPropagation();
  }

  editObject(obj, event) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    const viewType = 2;
    const title = "Update Role Object";
    this._triggleModal(obj, viewType, title);
    event.stopPropagation();
  }

  editRole(obj, event) {
    this.router.navigate(["/settings/roles/process/" + obj.id]);
    event.stopPropagation();
  }

  addRole() {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    // this.router.navigate(['/settings/roles/process/0']);
    const viewType = 1;
    const title = "Add Role";
    this._triggleModal(
      this.settingService.getInitRoleObject(),
      viewType,
      title
    );
  }

  addObject() {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    const viewType = 2;
    const title = "Add Role Object";
    this._triggleModal(
      this.settingService.getInitRoleObjectObject(),
      viewType,
      title
    );
  }

  _triggleModal(entity: any, viewType: number, title: string) {
    const _options: NgbModalOptions = {
      backdrop: false
    };
    const modalRef = this.modalService.open(ViewComponent, _options);
    modalRef.componentInstance.Info = {
      title: title,
      viewtype: viewType, // 1: for manage roles, 2: for manage objects
      data: entity
    };
    modalRef.result.then(
      result => {
        console.log(result);
        if (result.viewtype === 1) {
          this.dataService.AddRole(result.data);
        } else {
          this.dataService.AddObject(result.data);
          if (result.data.id > 0) {
            for (const object of this.ObjectList) {
              if (object.id === result.data.id) {
                object.objectname = result.data.objectname;
                object.description = result.data.description;
              }
            }
          }
        }
      },
      dismissed => {
        console.log("dismissed");
      }
    );
  }

  // delete role
  delete(item: any, index: number, event) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    const r = confirm("Are you sure you want to delete selected record?");
    if (r === true) {
      // this.RoleList.splice(index, 1);
      this.dataService.DeleteRecord(
        item,
        index,
        this.settingService.getApiOptions().delete_role,
        1
      );
    }
  }

  // delete object
  deleteObject(item: any, index: number, event) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    const r = confirm("Are you sure you want to delete selected record?");
    if (r === true) {
      // this.ObjectList.splice(index, 1);
      this.dataService.DeleteRecord(
        item,
        index,
        this.settingService.getApiOptions().delete_object,
        2
      );
    }
  }
}
