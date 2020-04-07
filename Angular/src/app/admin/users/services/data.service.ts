/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { UserAPIActions } from "../../../reducers/users/actions";
import { HttpClient } from "@angular/common/http";
import { SettingsService } from "./settings.service";
import { CoreAPIActions } from "../../../reducers/core/actions";

@Injectable()
export class DataService {
  constructor(
    private settings: SettingsService,
    private http: HttpClient,
    private actions: UserAPIActions,
    private coreActions: CoreAPIActions
  ) {}

  /* -------------------------------------------------------------------------- */
  /*                           Core load data api call                          */
  /* -------------------------------------------------------------------------- */
  LoadRecords(FilterOptions) {
    const URL = this.settings.getApiOptions().load;
    this.actions.loadStarted();
    this.http.post(URL, JSON.stringify(FilterOptions)).subscribe(
      (data: any) => {
        // update core data
        this.actions.loadSucceeded(data);
        if (data.categories.length > 0) {
          // if enabled, api send list of categories too
          // update categories in state
          this.actions.updateCategories(data.categories);
        }
        // update list stats
        this.coreActions.refreshListStats({
          totalrecords: data.records,
          pagesize: FilterOptions.pagesize,
          pagenumber: FilterOptions.pagenumber
        });
      },
      err => {
        this.actions.loadFailed(err);
      }
    );
  }

  AddRecord(obj: any) {
    let API_URL = "";
    // 1: create account, 2: edit profile, 3: change email, 4: change password, 5: change user type
    switch (obj.viewType) {
      case 1:
        API_URL = this.settings.getApiOptions().proc;
        break;
      case 2:
        API_URL = this.settings.getApiOptions().proc;
        break;
      case 3:
        API_URL = this.settings.getApiOptions().cemail;
        break;
      case 4:
        API_URL = this.settings.getApiOptions().chpass;
        break;
      case 5:
        API_URL = this.settings.getApiOptions().ctype;
        break;
    }
    if (obj.viewType === 2) {
      // edit profile
      obj.settings.userid = obj.id;
      obj.account.userid = obj.id;

      return this.http.post(
        API_URL,
        JSON.stringify({
          id: obj.id,
          firstname: obj.firstname,
          lastname: obj.lastname,
          attr_values: obj.attr_values,
          settings: obj.settings,
          account: obj.account
        })
      );
    } else {
      return this.http.post(API_URL, JSON.stringify(obj));
    }
  }

  UpdateThumb(info: any, images: any) {
    const URL = this.settings.getApiOptions().updatethumb;
    const param: any = {};
    param.Id = info.id;
    for (const image of images) {
      param.picturename = image.fname;
    }
  
    this.http.post(URL, JSON.stringify(param)).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-danger"
          });
        } else {
          this.actions.UpdateThumb(data.record);
          this.coreActions.Notify({
            title: "Profile Photo Updated",
            text: "",
            css: "bg-success"
          });
        }
      },
      err => {
        this.coreActions.Notify({
          title: err,
          text: "",
          css: "bg-danger"
        });
      }
    );
  }

  UpdateAvator(info: any) {
    const URL = this.settings.getApiOptions().updateavator;
    this.http.post(URL, JSON.stringify(info)).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-danger"
          });
        } else {
          this.actions.UpdateThumb(data.record);
          this.coreActions.Notify({
            title: "Avator Updated Successfully",
            text: "",
            css: "bg-success"
          });
        }
      },
      err => {
        this.coreActions.Notify({
          title: err,
          text: "",
          css: "bg-danger"
        });
      }
    );
  }

  /* -------------------------------------------------------------------------- */
  /*                              Get Single Record                             */
  /* -------------------------------------------------------------------------- */
  GetInfo(userid: string) {
    const URL = this.settings.getApiOptions().getinfo;
    return this.http.post(URL, JSON.stringify({ id: userid }));
  }

  /* -------------------------------------------------------------------------- */
  /*                       load reports (no pagination)                      */
  /* -------------------------------------------------------------------------- */
  LoadReports(queryOptions: any) {
  
    return this.http.post(this.settings.getApiOptions().load_reports, JSON.stringify(queryOptions));

  }


  Authenticate(user: any) {
    const URL = this.settings.getApiOptions().authenticate;
    return this.http.post(URL, JSON.stringify(user));
  }

  /* update control panel role */
  UpdateRole(user: any) {
    const URL = this.settings.getApiOptions().updaterole;
    return this.http.post(URL, JSON.stringify(user));
  }

  GetUserLog(userid: string) {
    const URL = this.settings.getApiOptions().userlog;
    return this.http.post(URL, JSON.stringify({ userid }));
  }

  DeleteAccount(user: any) {
    const URL = this.settings.getApiOptions().archive;
    return this.http.post(URL, JSON.stringify(user));
  }

  DeleteRecord(item, index) {
    item.actionstatus = "delete";
    const arr = [];
    arr.push(item);
    this.ProcessActions(arr, "delete");
  }
  
 /* -------------------------------------------------------------------------- */
  /*               Perform actions (enable, disable, approve) etc               */
  /* -------------------------------------------------------------------------- */
  ProcessActions(SelectedItems, isenabled) {
    // apply changes directory instate
    this.actions.applyChanges({
      SelectedItems,
      isenabled
    });

    const arr = [];
    for (const item of SelectedItems) {
      arr.push({
        id: item.id,
        actionstatus: item.actionstatus
      });
    }
   
    this.http
      .post(this.settings.getApiOptions().action, JSON.stringify(arr))
      .subscribe(
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
  
  ProcessLogActions(SelectedItems, isenabled) {
    this.coreActions.Notify({
      title: "Feature not yet implemented",
      text: "",
      css: "bg-success"
    });
  }
}
