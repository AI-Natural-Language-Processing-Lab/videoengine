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
import { SettingsService as CategorySettingService } from "../../../../../admin/settings/categories/services/settings.service";
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
  selector: "app-movie-uploader",
  animations: [fadeInAnimation],
  providers: [CategoryDataService, CategorySettingService, CategoriesAPIActions]
})
export class UploadMovieComponent implements OnInit {
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
  @Input() route_path = "/videos/";
  @Input() uploadType = 0; // 0: movie, 1: embed
  // Website own video categories
  @select(["videos", "categories"])
  readonly categories$: Observable<any>;

  @select(["categories", "dropdown_categories"])
  readonly dropdown_categories$: Observable<any>;

  @select(["configuration", "configs"])
  readonly settings$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  formHeading = "Upload Movie";
  Auth: any = {};
  showLoader = false;

  Categories: any = [];
  controls: any = [];
  submitText = "Save Changes";
  showCancel = false;
  cancelText = "Cancel";
  Settings: any = {};

  ngOnInit() {
    this.auth$.subscribe((auth: any) => {
      this.Auth = auth;
    });

    // already cached category list (with content)
    this.categories$.subscribe((categories: any) => {
      if (categories.length > 0) {
        this.Categories = categories;
        this.initializeControls();
      } else {
        this.showLoader = true;
        this.categoryDataService.LoadDropdownCategories(0); // 0: videos
      }
    });

    // load categories manually if not fetched with content.
    this.dropdown_categories$.subscribe((categories: any) => {
      if (categories.length > 0) {
        this.Categories = categories;
        this.initializeControls();
      }
      this.showLoader = false;
    });

    this.settings$.subscribe((settings: any) => {
      this.Settings = settings;
    });
  }

  initializeControls() {
    if (this.uploadType === 0) {
      this.initializeMovieControls();
    } else {
      this.initializeEmbedControls();
    }
    this.updateCategories();
  }

  initializeMovieControls() {
    let data: any = {
      title: "",
      description: "",
      tags: "",
      category_list: [],
      duration: "",
      movietype: 0,
      preview_url: "",
      pub_url: "",
      org_url: "",
      thumb_url: "",
      coverurl: "",
      price: 0,
      isapproved: 0
    };
    this.controls = this.formService.uploadMovieControls(data);
  }

  initializeEmbedControls() {
    let data: any = {
      title: "",
      description: "",
      tags: "",
      category_list: [],
      movietype: 0,
      embed_script: "",
      isapproved: 0,
      thumb_url: ""
    };
    this.controls = this.formService.embedVideoControls(data);

  }

  updateCategories() {
    this.coreService.updateCategories(this.controls, this.Categories);
  }

  SubmitForm(payload: any) {
    this.showLoader = true;
    payload.userid = this.Auth.User.id;
    payload.categories = this.coreService.returnSelectedCategoryArray(
      payload.categories
    );
    //console.log(payload);
    if (this.uploadType === 0) {
      this.SubmitMovieForm(payload);
    } else {
      this.SubmitEmbedForm(payload);
    }
   
  }

  SubmitMovieForm(payload: any) {
    this.dataService.AddMovie(payload).subscribe(
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
            title: "Movie Information Submitted",
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

  SubmitEmbedForm(payload: any) {
    payload.isexternal = 1;
    this.dataService.EmbedVideo(payload).subscribe(
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
            title: "Video Submitted",
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
