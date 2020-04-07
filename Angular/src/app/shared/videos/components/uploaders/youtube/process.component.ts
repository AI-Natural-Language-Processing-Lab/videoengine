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

// shared services
import { CoreService } from "../../../../../admin/core/coreService";
import { CoreAPIActions } from "../../../../../reducers/core/actions";

// reducer actions
import { VideoAPIActions } from "../../../../../reducers/videos/actions";
import { fadeInAnimation } from "../../../../../animations/core";

import { DataService as CategoryDataService } from "../../../../../admin/settings/categories/services/data.service";
import { SettingsService as CategorySettingService } from "../../../../../admin/settings/categories/services/settings.service";
import { CategoriesAPIActions } from "../../../../../reducers/settings/categories/actions";

@Component({
  templateUrl: "./process.html",
  selector: "app-youtube-uploader",
  animations: [fadeInAnimation],
  providers: [CategoryDataService, CategorySettingService, CategoriesAPIActions]
})
export class YoutubeComponent implements OnInit {
  constructor(
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: VideoAPIActions,
    private router: Router,
    private categoryDataService: CategoryDataService,
    private formService: FormService
  ) {}

  @Input() isAdmin = true;
  @Input() route_path = "/videos/";

  // Website own video categories
  @select(["videos", "categories"])
  readonly categories$: Observable<any>;

  @select(["categories", "dropdown_categories"])
  readonly dropdown_categories$: Observable<any>;

  // Youtube categories
  @select(["videos", "yt_categories"])
  readonly YoutubeCategories$: Observable<any>;

  // Youtube Search Result
  @select(["videos", "yt_result"])
  readonly YoutubeSearchResult$: Observable<any>;

  @select(["videos", "loading"])
  readonly loading$: Observable<any>;

  @select(["videos", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  YoutubeQuery: any = {
    term: "",
    advanceoption: false,
    userid: "",
    youtubecategory: "",
    order: 0,
    date: 0
  };

  YoutubeOrderList: any = [
    { id: 0, title: "Date" },
    { id: 1, title: "Rating" },
    { id: 2, title: "Relevance" },
    { id: 3, title: "Title" },
    { id: 4, title: "VideoCount" },
    { id: 5, title: "ViewCount" }
  ];

  YoutubeDateList: any = [
    { id: 3, title: "All Time" },
    { id: 0, title: "Today" },
    { id: 1, title: "This Week" },
    { id: 2, title: "This Month" }
  ];

  YoutubeResult: any = {
    tags: "",
    category: ""
  };

  YoutubeSearchResult: any = [];

  formHeading = "Upload Videos";

  IsLoaded = false;
  controls: any = [];
  step1 = true;
  step2 = false;

  Auth: any = {};
  Categories: any = [];
  showLoader = false;
  submitText = "Save Changes";
  // Categories = [];
  ngOnInit() {
    // load youtube categories
    this.dataService.LoadYoutubeCategories();

    this.isloaded$.subscribe((loaded: boolean) => {
      this.IsLoaded = loaded;
      if (!this.IsLoaded) {
        this.router.navigate([this.route_path]);
      }
    });
    this.auth$.subscribe((auth: any) => {
      this.Auth = auth;
      this.YoutubeQuery.userid = this.Auth.User.id;
    });
    this.YoutubeSearchResult$.subscribe((result: any) => {
      this.YoutubeSearchResult = result;
    });

    
    // already cached category list (with content)
    this.categories$.subscribe((categories: any) => {
      if (categories.length > 0) {
          this.Categories = categories;
          this.updateCategories();
      } else {
        this.showLoader = true;
        this.categoryDataService.LoadDropdownCategories(0); // 0: videos
      }
    });

    // load categories manually if not fetched with content.
    this.dropdown_categories$.subscribe((categories: any) => {
      if (categories.length > 0) {
        this.Categories = categories;
        this.updateCategories();
      } 
      this.showLoader = false;
    });
    // fetch param from url
    /*this.route.params.subscribe(params => {
           // initialize controls with default values
           this.Info = this.settingService.getInitObject();
           // this.initializeControls(this.Info);
       });*/
  }

  submitYoutube() {
    this.dataService.SearchYoutube(this.YoutubeQuery);
  }

  showAdvanceOptions(event: any) {
    this.YoutubeQuery.advanceoption = true;
    event.stopPropagation();
  }

  nextstep() {
    this.step1 = false;
    this.step2 = true;
    this.initializeControls({
      tags: "",
      category_list: []
    });
  }

  initializeControls(data: any) {
    this.controls = this.formService.uploaderOptionControls(data);
    this.updateCategories();
  }

  updateCategories() {
    this.coreService.updateCategories(this.controls, this.Categories);
  }

  SubmitForm(payload) {
    if (this.YoutubeSearchResult.length === 0) {
      this.coreActions.Notify({
        title: "Please select atleast one video to proceed",
        text: "",
        css: "bg-danger"
      });
      return;
    }

    this.showLoader = true;

    for (const item of this.YoutubeSearchResult) {
      item.tags = payload.tags;
      item.categories = this.coreService.returnSelectedCategoryArray(
        payload.categories
      );
      item.userid = this.Auth.User.id;
    }

    this.dataService.AddYoutubeVideos(this.YoutubeSearchResult).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-error"
          });
        } else {
          // clean up search result
          this.actions.cleanupYoutubeSearchResult();
          // display message
          this.coreActions.Notify({
            title: "Videos processed successfully",
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

  removeYt_Video(index, event) {
    if (index > -1) {
      console.log("splice hit");
      this.YoutubeSearchResult.splice(index, 1);
    }
    event.stopPropagation = true;
  }
}
