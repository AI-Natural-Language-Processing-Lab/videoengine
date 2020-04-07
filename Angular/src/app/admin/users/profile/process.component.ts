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
import { SettingsService } from "../services/settings.service";
import { DataService } from "../services/data.service";
import { FormService } from "../services/form.service";

// shared services
import { CoreService } from "../../core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { UserAPIActions } from "../../../reducers/users/actions";
import { fadeInAnimation } from "../../../animations/core";

import { PermissionService } from "../../../admin/users/services/permission.service";

@Component({
  templateUrl: "./process.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class UserProfileComponent implements OnInit {
  @select(["configuration", "configs"])
  readonly configs$: Observable<any>;
  // load all available roles
  @select(["roles", "roles"])
  readonly roles$: Observable<any>;

  @select(["roles", "isroleloaded"])
  readonly isroleloaded$: Observable<any>;

  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: UserAPIActions,
    private route: ActivatedRoute,
    private formService: FormService,
    public permission: PermissionService,
    private router: Router
  ) {}

  controls: any = [];
  ToolbarOptions: any;
  ToolbarLogOptions: any;
  isItemsSelected = true;
  RecordID = "";
  SearchOptions: any;
  FilterOptions: any = {};
  showLoader = false;
  formHeading = "User Information";
  submitText = "Submit";
  Info: any = {};
  uploadedFiles = [];
  SelectedItems: any = [];
  FullName = "";
  ViewType = 0; // 0: profile view, 1: log view, 2: edit view
  UserLog = [];

  // User Roles
  Roles: any = [];

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete

  ngOnInit() {
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

    this.SearchOptions = this.settingService.getSearchOptionsProfile();
    this.ToolbarOptions = this.settingService.getToolbarOptions();
    this.ToolbarOptions.showtoolbar = false;
    this.ToolbarOptions.showcheckAll = false;
    this.ToolbarOptions.showsecondarytoolbar = true;
    this.ToolbarOptions.ProfileView = true;

    this.ToolbarLogOptions = this.settingService.getUserLogToolbarOptions();
    this.ToolbarLogOptions.showcheckAll = false;

    // fetch param from url
    this.route.params.subscribe(params => {
      this.RecordID = params["id"];
      if (this.RecordID === null) {
        this.RecordID = "";
      }
      if (this.RecordID !== "") {
        this.LoadInfo();
      }
    });
  }

  LoadInfo() {
    this.showLoader = true;
    this.dataService.GetInfo(this.RecordID).subscribe((data: any) => {
      if (data.status === "error") {
        this.coreActions.Notify({
          title: data.message,
          text: "",
          css: "bg-success"
        });
        // redirect to user page
        this.router.navigate(["/users"]);
      } else {
        this.Info = data.post;

        // for actions like enable / disable / delete
        this.SelectedItems = []; // reset
        this.SelectedItems.push(this.Info);

        if (this.Info.firstname === null || this.Info.firstname === "") {
          this.FullName = this.Info.id;
        } else {
          this.FullName = this.Info.firstname + " " + this.Info.lastname;
        }
        const _files = [];
        if (this.Info.img_url !== "") {
          _files.push({
            fname: this.Info.img_url,
            filename: this.Info.img_url,
            filetype: ".jpg",
            url: this.Info.img_url
          });
          this.uploadedFiles = _files;
        }
        this.AdjustNavigationLinks();

        // dynamic attribute processing
        this.coreService.prepareDynamicControlData(this.Info);
       
      }
      this.showLoader = false;
    });
  }

  AdjustNavigationLinks() {
    for (const nav of this.SearchOptions.navList) {
      if (!nav.clickevent) {
        nav.url = "/users/history/" + this.Info.id;
      }
    }
  }

  OnUploadedImages(images: any) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    this.dataService.UpdateThumb(this.Info, images);
  }

  toolbaraction(selection: any) {
    switch (selection.action) {
      case "m_markas":
        this.ProcessActions(selection.value);
        break;
      case "m_log_markas":
        this.ProcessLogActions(selection.value);
        break;
      case "add":
        this.formHeading = "Create Account";
        this.Trigger_Modal({
          data: this.settingService.getInitObject(),
          isActionGranded: this.isActionGranded,
          viewType: 1
        });
        return;
      case "edit_profile":
        this.formHeading = "Edit User Profile";
        this.Trigger_Modal({
          data: this.Info,
          isActionGranded: this.isActionGranded,
          viewType: 2
        });
        return;
      case "change_email":
        this.formHeading = "Change User Email";
        this.Trigger_Modal({
          data: this.Info,
          isActionGranded: this.isActionGranded,
          viewType: 3
        });
        return;
      case "change_password":
        this.formHeading = "Change User Password";
        this.Trigger_Modal({
          data: this.Info,
          isActionGranded: this.isActionGranded,
          viewType: 4
        });
        return;
      case "change_usertype":
        this.formHeading = "Update User Type";
        this.Trigger_Modal({
          data: this.Info,
          isActionGranded: this.isActionGranded,
          viewType: 5
        });
        return;
      case "view_history":
        this.ViewType = 1;
        if (this.UserLog.length === 0) {
          this.loadUserLog();
        }
        break;
      case "home_view":
        this.ViewType = 0;
        break;
    }
  }

  loadUserLog() {
    this.showLoader = true;
    this.dataService.GetUserLog(this.Info.id).subscribe((data: any) => {
      if (data.status === "error") {
        this.coreActions.Notify({
          title: data.message,
          text: "",
          css: "bg-success"
        });
      } else {
        this.UserLog = data.posts;
      }
      this.showLoader = false;
    });
  }

  /* Add Record */
  Trigger_Modal(obj: any) {
    this.ViewType = 2;
    this.controls = this.formService.getControls(obj.data, obj.viewType);
    this.Info.viewType = obj.viewType;
  }

  SubmitForm(payload) {
    // permission check
    if (this.Info.isActionGranded !== undefined) {
      if (!this.Info.isActionGranded) {
        this.coreActions.Notify({
          title: "Permission Denied",
          text: "",
          css: "bg-danger"
        });
        return;
      }
    }
    // custom validation
    if (this.Info.viewType === 1 || this.Info.viewType === 4) {
      if (payload.password !== payload.cpassword) {
        this.coreActions.Notify({
          title: "Password Not Matched",
          text: "",
          css: "bg-danger"
        });
      }
    }

    this.showLoader = true;
    // adjust values with actual object
    if (this.Info.viewType !== 2) {
      for (const prop of Object.keys(this.Info)) {
        for (const payload_prop of Object.keys(payload)) {
          if (prop === payload_prop) {
            this.Info[prop] = payload[payload_prop];
          }
        }
      }
    } else {
      // update profile
      // need custom adjustments based on api call data structure
      this.Info.firstname = payload.firstname;
      this.Info.lastname = payload.lastname;
      // this.Info.profile.gender = payload.gender;

      this.Info.attr_values = this.coreService.processDynamicControlsData(payload, this.Info);
      // end dynamic attributes

      this.Info.settings.isemail = 0;
      if (payload.isemail) this.Info.settings.isemail = 1;

      this.Info.settings.issendmessages = 0;
      if (payload.issendmessages) this.Info.settings.issendmessages = 1;

      this.Info.account.credits = payload.credits;

      this.Info.account.islifetimerenewal = 0;
      if (payload.islifetimerenewal)
        this.Info.account.islifetimerenewal = 1;

      this.Info.account.paypal_subscriber = 0;
    }

    // enable admin related additional edit options
    this.Info.isadmin = true;
    
    //console.log(this.Info);
    this.dataService.AddRecord(this.Info).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-danger"
          });
        } else {
          let message = "Account Created Successfully";
          switch (this.Info.viewType) {
            case 1:
              this.actions.addRecord(data.record);
              break;
            case 2:
              message = "Profile Updated Successfully";
              this.actions.updateRecord(data.record);
              break;
            case 3:
              message = "Email Updated Successfully";
              break;
            case 4:
              message = "Password Changed";
              break;
            case 5:
              message = "Account Type Changed";
              break;
          }
          this.coreActions.Notify({
            title: message,
            text: "",
            css: "bg-success"
          });
        }
        this.showLoader = false;
        // back to public view
        this.ViewType = 0;
      },
      err => {
        this.showLoader = false;
        this.coreActions.Notify({
          title: "Error Occured",
          text: "",
          css: "bg-danger"
        });
      }
    );
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
      }
      this.dataService.ProcessActions(this.SelectedItems, selection);
    }
  }

  ProcessLogActions(selection: any) {
    if (this.UserLog.length > 0) {
      for (const item of this.UserLog) {
        item.actionstatus = selection.actionstatus;
      }
      this.dataService.ProcessLogActions(this.UserLog, selection);
    }
  }

  onImageUploaded(info: any) {
    console.log("image uploaded");
    console.log(info);
    this.dataService.UpdateAvator(info);
  }
}
