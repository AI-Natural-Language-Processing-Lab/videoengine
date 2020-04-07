/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { ROLEAPIActions } from "../../../../reducers/settings/roles/actions";
import { HttpClient } from "@angular/common/http";

import { SettingsService } from "./settings.service";
import { CoreAPIActions } from "../../../../reducers/core/actions";

@Injectable()
export class DataService {
  constructor(
    private settings: SettingsService,
    private http: HttpClient,
    private actions: ROLEAPIActions,
    private coreActions: CoreAPIActions
  ) {}

  LoadRoles() {
    const URL = this.settings.getApiOptions().load_roles;
    this.actions.loadStarted();
    this.http.post(URL, {}).subscribe(
      (data: any) => {
        // update core data
        this.actions.loadSucceeded(data);
      },
      err => {
        this.actions.loadFailed(err);
      }
    );
  }
  LoadObjects() {
    const URL = this.settings.getApiOptions().load_objects;
    this.actions.loadObjectStarted();
    this.http.post(URL, {}).subscribe(
      (data: any) => {
        // update core data
        this.actions.loadObjectSucceeded(data);
      },
      err => {
        this.actions.loadObjectFailed(err);
      }
    );
  }
  /* add record modal popup case */
  AddRole(obj) {
    console.log(this.settings.getApiOptions().add_role);
    this.http
      .post(this.settings.getApiOptions().add_role, JSON.stringify(obj))
      .subscribe(
        (data: any) => {
          // update core data
          this.actions.addRole(data.record);
        },
        err => {
          this.actions.loadObjectFailed(err);
        }
      );
  }

  AddObject(obj) {
    this.http
      .post(this.settings.getApiOptions().add_object, JSON.stringify(obj))
      .subscribe(
        (data: any) => {
          // update core data
          if (obj.id > 0) {
            this.actions.updateObject(data.record);
          } else {
            this.actions.addObject(data.record);
          }
        },
        err => {
          this.actions.loadObjectFailed(err);
        }
      );
  }

  /* -------------------------------------------------------------------------- */
  /*                              Get Single Record                             */
  /* -------------------------------------------------------------------------- */
  GetInfo(id: number) {
    const URL = this.settings.getApiOptions().getinfo;
    return this.http.post(URL, JSON.stringify({ id }));
  }

  UpdatePermission(arr) {
    const URL = this.settings.getApiOptions().update_permission;
    return this.http.post(URL, JSON.stringify(arr));
  }

  DeleteRecord(item, index, url, type) {
    item.actionstatus = "delete";
    const arr = [];
    arr.push(item);
    this.ProcessActions(arr, "delete", url, type);
  }

  
  ProcessActions(SelectedItems, isenabled, url, type) {
    if (type === 1) {
      this.actions.applyRoleChanges({
        SelectedItems,
        isenabled
      });
    } else {
      this.actions.applyObjectChanges({
        SelectedItems,
        isenabled
      });
    }
    this.http.post(url, JSON.stringify(SelectedItems)).subscribe(
      (data: any) => {
        // this.coreActions.Notify(data.message);
        let message = "Operation Performed";
        if (isenabled === "delete") {
          message = "Record Removed";
        }
        this.coreActions.Notify({
          title: message,
          text: "",
          css: "bg-success"
        });
      },
      err => {
        this.coreActions.Notify({
          title: "Error Occured",
          text: "",
          css: "bg-danger"
        });
      }
    );
  }
}
