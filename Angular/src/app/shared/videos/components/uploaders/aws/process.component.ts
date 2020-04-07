/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { Router } from "@angular/router";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// services
import { DataService } from "../../../services/data.service";
import { FormService } from "../../../services/form.service";

import { DataService as CategoryDataService } from "../../../../../admin/settings/categories/services/data.service";
import { SettingsService as CategorySettingService } from "../../../../../admin/settings/categories/services/settings.service"
import { CategoriesAPIActions } from "../../../../../reducers/settings/categories/actions";
// shared services
import { CoreService } from "../../../../../admin/core/coreService";
import { CoreAPIActions } from "../../../../../reducers/core/actions";

// reducer actions
import { VideoAPIActions } from "../../../../../reducers/videos/actions";
import { fadeInAnimation } from "../../../../../animations/core";

import { AppConfig } from "../../../../../configs/app.config";

@Component({
  templateUrl: "./process.html",
  selector: "app-aws-uploader",
  animations: [fadeInAnimation],
  providers: [CategoryDataService, CategorySettingService, CategoriesAPIActions]
})
export class AWSComponent implements OnInit {

  constructor(
    private dataService: DataService,
    private coreActions: CoreAPIActions,
    private actions: VideoAPIActions,
    private router: Router,
    private formService: FormService,
    private coreService: CoreService,
    private categoryDataService: CategoryDataService,
    public config: AppConfig
  ) {}
 
  @Input() isAdmin = true;
  @Input() route_path = '/videos/';

  // Website own video categories
  @select(["videos", "categories"])
  readonly categories$: Observable<any>;

  @select(["categories", "dropdown_categories"])
  readonly dropdown_categories$: Observable<any>;

  @select(["videos", "loading"])
  readonly loading$: Observable<any>;

  @select(["videos", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["videos", "uploadedfiles"])
  readonly uploadedfiles$: Observable<any>;

  @select(["configuration", "configs"])
  readonly settings$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  formHeading = "Upload Videos";
  Auth: any = {};
  showLoader = false;
  Panel = 0; // 0: uploader, 1: input form
  VideoInfo: any = {
    tags: "",
    category: ""
  };
  extensions = "mp4,avi,wmv,mpg,mpeg,webm,flv,ogv";
  filesize = "1000mb"
  max_concurrent_uploads = 5;
  Categories: any = [];
  controls: any = [];
  submitText = "Save Changes";
  showCancel = true;
  cancelText = "Back";
  Settings: any = {};
  UploadedFiles: any = [];
  ngOnInit() {
    // load required categories
    // this.dataService.LoadCategories();

    this.auth$.subscribe((auth: any) => {
      this.Auth = auth;
    });

    // already cached category list (with content)
    this.categories$.subscribe((categories: any) => {
      if (categories.length > 0) {
          this.Categories = categories;
      } else {
        this.showLoader = true;
        this.categoryDataService.LoadDropdownCategories(0); // 0: videos
      }
    });

    // load categories manually if not fetched with content.
    this.dropdown_categories$.subscribe((categories: any) => {
      if (categories.length > 0) {
        this.Categories = categories;
        // this.updateCategories();
      } 
      this.showLoader = false;
    });
    

    this.settings$.subscribe((settings: any) => {
      this.Settings = settings;
      if (this.Settings.videos.general !== undefined ) {
          this.extensions = this.Settings.videos.general.extensions;
          this.filesize = this.Settings.videos.general.max_size;
          this.max_concurrent_uploads = this.Settings.videos.general.max_concurrent_uploads;
      }
    });

    this.uploadedfiles$.subscribe((files: any) => {
      this.UploadedFiles = files;
    });
  }

  OnUploadedVideos(videos: any) {
    // prepare output
    for (const video of videos) {
      video.id = 0;
      video.title = "", 
      video.sf = video.filename; // backend api need sf as source video file
      video.description = "";
      video.categories = "";
      video.privacy = 0;
      video.userid = this.Auth.User.id;
      video.albumid = 0;
      video.type = 0;
    }

    // update video files in video reducer
    this.actions.uploadedVideoFiles(videos);
  }

  nextPanel() {
    this.Panel = 1;
    this.initializeControls({
      tags: '',
      category_list: []
    });
  }

  Cancel(event: any) {
    this.Panel = 0;
  }

  initializeControls(data: any) {
    this.controls = this.formService.uploaderOptionControls(
      data
    );
    this.updateCategories();
  }

  updateCategories() {
    
    this.coreService.updateCategories(this.controls, this.Categories);

   
  }

  SubmitForm(payload) {
    if (this.UploadedFiles.length > 0) {
      for (let item of this.UploadedFiles) {
        item.tags = payload.tags;
        item.categories = this.coreService.returnSelectedCategoryArray(
          payload.categories
        );
        item.userid = this.Auth.User.id;
      }
      this.processVideos(this.UploadedFiles);
    }
  }

  processVideos(videos: any) {
    this.showLoader = true;
    // console.log('selected videos');
    // console.log(videos);
    this.dataService.AddAWSVideos(videos).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-error"
          });
        } else {
          // clean up search result
          this.actions.resetPublishing();
          // display message
          this.coreActions.Notify({
            title: "Uploaded videos submitted for processing",
            text: "",
            css: "bg-success"
          });

          // enable reload action to refresh data
          this.actions.reloadList();
          // redirect to videos page
          this.router.navigate([this.route_path]);
        }
        this.showLoader = false;
      },
      err => {
        this.actions.loadEnd(); // hide progressbar
        this.coreActions.Notify({
          title: "Error Occured",
          text: "",
          css: "bg-danger"
        });
      }
    );
  }
}
