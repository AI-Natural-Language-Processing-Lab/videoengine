/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import { Component, OnInit, Input } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// services
import { SettingsService } from "../../../services/settings.service";
import { DataService } from "../../../../../shared/videos/services/data.service";
import { FormService } from "../../../../../shared/videos/services/form.service";

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
  selector: "app-direct-uploader",
  animations: [fadeInAnimation],
  providers: [CategoryDataService, CategorySettingService, CategoriesAPIActions]
})
export class DirectUploaderComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: VideoAPIActions,
    private route: ActivatedRoute,
    private formService: FormService,
    private categoryDataService: CategoryDataService,
    private router: Router,
    public config: AppConfig
  ) {}

  @Input() isAdmin = true;
  @Input() route_path = "/videos/";

  @select(["videos", "categories"])
  readonly categories$: Observable<any>;

  @select(["categories", "dropdown_categories"])
  readonly dropdown_categories$: Observable<any>;

  @select(["videos", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;
  RecordID = 0;
  SearchOptions: any;
  controls: any = [];
  showLoader = false;
  formHeading = "Upload Videos";
  submitText = "Submit";
  Info: any;
  IsLoaded = false;
  Categories: any = [];
  UploadedFiles: any = [];
  Steps = 1;
  URL = "";
  Auth: any = {};

  ngOnInit() {
    this.URL = this.config.getConfig("host");
    this.isloaded$.subscribe((loaded: boolean) => {
      this.IsLoaded = loaded;
      if (!this.IsLoaded) {
        this.router.navigate(["/videos/"]);
      }
    });
    this.auth$.subscribe((auth: any) => {
      this.Auth = auth;
    });
    // fetch param from url
    this.route.params.subscribe(params => {
      this.RecordID = Number.parseInt(params["id"]);

      if (isNaN(this.RecordID)) {
        this.RecordID = 0;
      }
      if (this.RecordID > 0) {
        // this.formHeading = 'Update Post';
        // this.submitText = 'Update';
        // this.LoadInfo();
      } else {
        // initialize controls with default values
        this.Info = this.settingService.getInitObject();
        this.initializeControls(this.Info);
      }
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
  }

  LoadInfo() {
    /* this.showLoader = true;
      this.dataService.GetInfo(this.RecordID).subscribe( (data: any) => {
           if (data.status === 'success') {
               // update post
               this.initializeControls(data.post);
           } else {
               this.coreActions.Notify({
                    title: data.message,
                    text: '',
                    css: 'bg-error' });
               this.initializeControls(this.settingService.getInitObject());
           }
           this.showLoader = false;
       }); */
  }

  updateCategories() {
    this.coreService.updateCategories(this.controls, this.Categories);
  }

  initializeControls(data: any) {
    this.controls = this.formService.getControls(data, this.Auth);
    this.updateCategories();
  }
  makeid() {
    let text = "";
    const possible =
      "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (let i = 0; i < 5; i++) {
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
  }

  gDur(dur ) {

    const hours = Math.floor(dur / (1 * 60 * 60));
    let hours_str = "";
    dur -= hours * (1 * 60 * 60);
    if (hours < 10) {
      hours_str = "0" + hours;
    }

    const mins = Math.floor(dur / (1 * 60));
    let mins_str = "";
    dur -= mins * (1 * 60);
    if (mins < 10) {
      mins_str = "0" + mins;
    }
    const seconds = Math.floor(dur / 1);
    let seconds_str = "";
    dur -= seconds * 1;
    if (seconds < 10) {
      seconds_str = "0" + seconds;
    } else {
      seconds_str = seconds.toString();
    }
    return hours_str + ":" + mins_str + ":" + seconds_str;
  }

  attachthumbnails(obj: any) {
    console.log(obj);
    this.TriggleModal(obj);
  }

  TriggleModal(obj: any) {
    /* const _options: NgbModalOptions = {
      backdrop: false
    };
    const title = "Upload Video Thumbnails";
    const modalRef = this.modalService.open(ViewComponent, _options);
    modalRef.componentInstance.Info = {
      title: title,
      data: obj,
      viewType: 0,
      auth: this.Auth
    };
    modalRef.result.then(
      result => {
        for (const video of this.UploadedFiles.files) {
          console.log(result);
          if (video.id === result.data.id) {
            console.log("matched");
            console.log(result.data.id);
            video.video_thumbs = result.data.video_thumbs;
          }
        }
      },
      dismissed => {
        console.log("dismissed");
      }
    ); */
  }

  delete(arr: any, index: number, event: any) {
    if (index > -1) {
      arr.video_thumbs.splice(index, 1);
    }
    event.stopPropagation();
  }
  capture(obj: any) {
    console.log(obj);
    /* create canvas */
    const _id = this.makeid();
    // prepare context
    const video: any = document.getElementById("vd_" + obj.id); // $('#vd_' + obj.id); //document.querySelector('video');
    const width = video.videoWidth;
    const height = video.videoHeight;
    const duration = this.gDur(video.duration);

    const canvas: any = document.createElement("canvas");
    canvas.id = _id;
    canvas.width = 800;
    canvas.height = 600;
    canvas.style.zIndex = 8;
    canvas.style.border = "1px solid";
    const ctx = canvas.getContext("2d");
    ctx.drawImage(video, 0, 0, 800, 600);

    const view: any = document.getElementById("view_" + obj.id);
    view.appendChild(canvas);

    // console.log(canvas.toDataURL());

    /* create temp canvas just for preview only */
    const _temp_canvas_id = "temp_" + _id;
    const temp_canvas: any = document.createElement("canvas");
    temp_canvas.id = _temp_canvas_id;
    temp_canvas.width = 180;
    temp_canvas.height = 120;
    temp_canvas.style.zIndex = 8;
    temp_canvas.style.border = "1px solid";

    const temp_ctx = temp_canvas.getContext("2d");
    temp_ctx.drawImage(video, 0, 0, 180, 120);

    let _selected = false;
    if (obj.video_thumbs.length === 0) {
      _selected = true;
    }

    obj.video_thumbs.push({
      id: _id,
      temp: temp_canvas.toDataURL(),
      filename: canvas.toDataURL(),
      selected: _selected
    });
  }
  SubmitForm(payload) {
    if (payload.files.length > 0) {
      for (const file of payload.files) {
        file.id = this.makeid();
        file.videofilename = file.filename;
        file.title = "";
        file.thumb_url = "";
        file.description = "";
        file.duration = "";
        file.duration_sec = 0;
        file.userid = this.Auth.User.id;
        file.video_thumbs = [];
        file.categories = this.coreService.returnSelectedCategoryArray(
          payload.categories
        );
        file.tags = payload.tags;
      }

      this.UploadedFiles = payload;
      this.Steps = 2;
    }
  }
  captureDuration(obj: any) {
    const video: any = document.getElementById("vd_" + obj.id); // $('#vd_' + obj.id); //document.querySelector('video');
    obj.duration_sec = video.duration;
    obj.duration = this.gDur(video.duration);
  }

  saveChanges() {
    this.showLoader = true;
    // console.log(this.UploadedFiles.files);
    // return;
    for (const file of this.UploadedFiles.files) {
      file.duration_sec = parseInt(file.duration_sec, 10);
      file.id = 0;
    }
    //console.log(this.UploadedFiles.files);
    this.dataService.DirectVideo(this.UploadedFiles.files).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-error"
          });
        } else {
          this.coreActions.Notify({
            title: "Record Processed Successfully",
            text: "",
            css: "bg-success"
          });

          // enable reload action to refresh data
          this.actions.reloadList();
          this.router.navigate([this.route_path]);
        }
        this.showLoader = false;
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

  remove(index, event) {
    if (index > -1) {
      this.UploadedFiles.files.splice(index, 1);
    }
    event.stopPropagation = true;
  }
}
