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
import { SettingsService } from "../../../shared/videos/services/settings.service";
import { DataService } from "../../../shared/videos/services/data.service";

// shared services
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { fadeInAnimation } from "../../../animations/core";

/* modal popup */
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";
// modal popup
import { ViewComponent } from "../../../shared/videos/components/partials/modal.component";

import { PermissionService } from "../../../admin/users/services/permission.service";

@Component({
  templateUrl: "./process.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class VideoProfileComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreActions: CoreAPIActions,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private permission: PermissionService,
    private router: Router
  ) {}

  ToolbarOptions: any;
  ToolbarLogOptions: any;
  isItemsSelected = true;
  RecordID = 0;
  SearchOptions: any;
  FilterOptions: any = {};
  controls: any = [];
  showLoader = false;
  formHeading = "Video Detail";
  submitText = "Submit";
  Info: any = {};
  uploadedFiles = [];
  SelectedItems: any = [];
  Author_FullName = "";
  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete
  ngOnInit() {
    // user authentication & access right management
    // full resource access key and readonly key can be generated via roles management
    this.auth$.subscribe((auth: any) => {
      const FullAccessID = "1521153486644";
      const ReadOnlyAccessID = "1521395130448";
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

    this.ToolbarOptions = this.settingService.getToolbarOptions(0, true);
    this.ToolbarOptions.showtoolbar = false;
    this.ToolbarOptions.showcheckAll = false;
    this.ToolbarOptions.showsecondarytoolbar = true;
    this.ToolbarOptions.ProfileView = true;

    // fetch param from url
    this.route.params.subscribe(params => {
      this.RecordID = Number.parseInt(params["id"], 10);
      if (isNaN(this.RecordID)) {
        this.RecordID = 0;
      }
      if (this.RecordID > 0) {
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
        // redirect to main page
        this.router.navigate(["/videos"]);
      } else {
        this.Info = data.post;
        if (
          this.Info.author.firstname === null ||
          this.Info.author.firstname === ""
        ) {
          this.Author_FullName = this.Info.author.username;
        } else {
          this.Author_FullName =
            this.Info.author.firstname + " " + this.Info.author.lastname;
        }

        // tag processing
        if (this.Info.tags !== null && this.Info.tags !== "") {
          this.Info.tags_arr = this.ProcessCategories(
            this.Info.tags.split(",")
          );
        } else {
          this.Info.tags_arr = [];
        }
        // for actions like enable / disable / delete
        this.SelectedItems = []; // reset
        this.SelectedItems.push(this.Info);

        // process thumbs
        this.processUpdateThumbs();
      }
      this.showLoader = false;
    });
  }
  ProcessCategories(categories) {
    const arr = [];
    for (const category of categories) {
      arr.push({
        title: category.trim(),
        slug: category
          .trim()
          .replace(/\s+/g, "-")
          .toLowerCase()
      });
    }

    return arr;
  }
  toolbaraction(selection: any) {
    switch (selection.action) {
      case "m_markas":
        this.ProcessActions(selection.value);
        break;
      case "add":
        this.Trigger_Modal({
          title: "Create Account",
          isActionGranded: this.isActionGranded,
          data: this.settingService.getInitObject(),
          viewType: 1
        });
        return;
      case "edit":
        this.Trigger_Modal({
          title: "Edit User Profile",
          isActionGranded: this.isActionGranded,
          data: this.Info,
          viewType: 2
        });
        return;
    }
  }

  /* Add Record */
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
      this.dataService.ProcessActions(this.SelectedItems, selection, 0);
    }
  }

  // responsible for generating story board and allow user to update default thumb
  processUpdateThumbs() {
    console.log(this.Info);
    if (this.Info.isexternal === 0) {
      this.Info.video_thumbs = [];
      // only own videos
      for (let i = 1; i <= 10; i++) {
        let _name = "";
        if (i <= 9) {
          _name = this.Info.picturename + "_00" + i + ".jpg";
        } else {
          _name = this.Info.picturename + "_0" + i + ".jpg";
        }
        let _selected = false;
        if (_name === this.Info.thumb_url) {
          _selected = true;
        } else {
          _selected = false;
        }
        this.Info.video_thumbs.push({
          id: i,
          filename: _name,
          selected: _selected
        });
      }
    }
  }

  updateSelectedThumb(thumb, event) {
    // reset all thumbnail selection.
    if (this.Info.video_thumbs.length > 0) {
      for (const thumbnail of this.Info.video_thumbs) {
        if (thumbnail.id === thumb.id) {
          thumbnail.selected = true;
        } else {
          thumbnail.selected = false;
        }
      }
      event.stopPropagation();
    }
  }

  updateThumb() {
    const obj: any = {
      id: this.Info.id,
      username: this.Info.id
    };
    for (const thumbnail of this.Info.video_thumbs) {
      if (thumbnail.selected) {
        obj.picturename = thumbnail.filename;
      }
    }
    this.dataService.UpdateThumbnail(obj).subscribe(
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
