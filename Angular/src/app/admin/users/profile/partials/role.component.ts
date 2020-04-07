/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// role services
import * as RoleDataService from "../../../settings/roles/services/data.service";
import * as RoleSettingService from "../../../settings/roles/services/settings.service";
import { ROLEAPIActions } from "../../../../reducers/settings/roles/actions";

// services
import { DataService } from "../../services/data.service";

// shared services
import { CoreAPIActions } from "../../../../reducers/core/actions";

@Component({
  selector: "app-role",
  templateUrl: "./role.html",
  providers: [
    RoleDataService.DataService,
    RoleSettingService.SettingsService,
    ROLEAPIActions
  ]
})
export class UserRoleComponent implements OnInit {
  @Input() Info: any = {};
  @Input() isActionGranded = false;
  isLoading = false;

  // load all available roles
  @select(["roles", "roles"])
  readonly roles$: Observable<any>;

  @select(["roles", "isroleloaded"])
  readonly isroleloaded$: Observable<any>;

  constructor(
    private dataService: DataService,
    private coreActions: CoreAPIActions,
    private roledataService: RoleDataService.DataService
  ) {}

  ngOnInit() {
    this.isroleloaded$.subscribe((loaded: boolean) => {
      if (!loaded) {
        this.roledataService.LoadRoles();
      }
    });
  }

  updateRole() {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    this.isLoading = true;

    this.dataService.UpdateRole(this.Info).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-error"
          });
        } else {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-success"
          });
        }
        this.isLoading = false;
      },
      err => {
        this.isLoading = false;
        this.coreActions.Notify({
          title: "Error Occured",
          text: "",
          css: "bg-danger"
        });
      }
    );
  }
}
