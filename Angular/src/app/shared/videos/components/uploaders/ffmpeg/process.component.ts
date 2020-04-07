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
  selector: "app-ffmpeg-uploader",
  animations: [fadeInAnimation],
  providers: [CategoryDataService, CategorySettingService, CategoriesAPIActions]
})
export class FFMPEGComponent implements OnInit {
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

  UploadedFiles: any = [];
  showSubmitBtn = false;
  selectedFile: any = {};
  EncodingIndex = 0; // track encoding file
  showLoader = false;
  Panel = 0; // 0: uploader, 1: input form
  VideoInfo: any = {
    tags: "",
    category: ""
  };

  Categories: any = [];
  extensions = "mp4,avi,wmv,mpg,mpeg,webm,flv,ogv";
  filesize = "1000mb";
  max_concurrent_uploads = 5;
  controls: any = [];
  submitText = "Save Changes";
  showCancel = false;
  cancelText = "Back";

  Settings: any = {};
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

    this.settings$.subscribe((settings: any) => {
      this.Settings = settings;
      if (this.Settings.videos.general !== undefined) {
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
      video.id = this.coreService.makeid();
      video.sf = video.filename;
      (video.pf =
        video.fileIndex + "-" + this.coreService.makeid() + "_pub.mp4"),
        (video.isenabled = "start");
      video.errorcode = 0;
      video.progress = 0;
      (video.progressStyle = {
        width: "0%"
      }), // for progress bar
        (video.title = ""),
        (video.description = "");
      video.categories = "";
      // categories_arr: $scope.Info.categories_arr,
      video.categories = "";
      video.privacy = 0;
      video.userid = this.Auth.User.id;
      video.albumid = 0;
      video.video_thumbs = [];
      video.processID = 0;
      video.panelstatus = 0; // 0: progress bar, 1: show processing, 2: preview video
      video.defaultImg = "";
    }

    // update video files in video reducer
    this.actions.uploadedVideoFiles(videos);

    this.publish();
  }

  publish() {
    this.uploadedfiles$.subscribe((files: any) => {
      if (files.length > 0) {
        if (this.EncodingIndex < files.length) {
          // update selected video in video reducer store
          // this.actions.selectedVideoFile(files[this.EncodingIndex]);
          this.selectedFile = files[this.EncodingIndex];
          this.encodeVideo();
        } else {
          this.finalOutput();
        }
      }
    });
  }

  encodeVideo() {
    this.selectedFile.isenabled = "progress";

    this.dataService
      .EncodeVideo({
        sf: this.selectedFile.sf,
        pf: this.selectedFile.pf,
        tp: 0,
        userid: this.Auth.User.id
      })
      .subscribe(
        (data: any) => {
          if (parseInt(data.ecode, 10) > 0) {
            this.selectedFile.errorcode = parseInt(data.ecode, 10);
            this.selectedFile.edesc = data.edesc;
            this.EncodingIndex++;
            this.publish();
          } else {
            this.selectedFile.processID = data.procid;
            this.selectedFile.IntervalID = setInterval(() => {
              this.getProgress();
            }, 1000);
          }
          // update video output in reducer
          this.actions.updateVideoFile(this.selectedFile);
        },
        err => {
          this.coreActions.Notify({
            title: "Error occured while encoding video",
            text: "",
            css: "bg-danger"
          });
        }
      );
  }

  getProgress() {
    this.dataService
      .EncodeVideo({
        pid: this.selectedFile.processID,
        tp: 1,
        userid: this.Auth.User.id
      })
      .subscribe(
        (data: any) => {
          this.selectedFile.progress = parseInt(data.status, 10);
          this.selectedFile.progressStyle = {
            width: this.selectedFile.progress + "%"
          };
          if (parseInt(data.status, 10) >= 100) {
            this.selectedFile.isenabled = "completed";
            if (this.selectedFile.IntervalID !== 0) {
              clearInterval(this.selectedFile.IntervalID);
            }
            // update video output in reducer
            this.actions.updateVideoFile(this.selectedFile);
            this.getInfo();
          }
        },
        err => {
          this.coreActions.Notify({
            title: "Error occured while encoding video",
            text: "",
            css: "bg-danger"
          });
        }
      );
  }

  getInfo() {
    this.selectedFile.panelstatus = 1; // 0: progress bar, 1: show processing, 2: preview video
    this.dataService
      .EncodeVideo({
        pid: this.selectedFile.processID,
        sf: this.selectedFile.sf,
        pf: this.selectedFile.pf,
        tp: 2,
        userid: this.Auth.User.id
      })
      .subscribe(
        (data: any) => {
          if (data.status === "OK") {
            this.selectedFile.panelstatus = 2; // 0: progress bar, 1: show processing, 2: preview video
            this.selectedFile.errorcode = data.ecode;
            this.selectedFile.pf = data.fname;
            this.selectedFile.duration = data.dur;
            this.selectedFile.dursec = data.dursec;
            this.selectedFile.tfile = data.tfile;
            this.selectedFile.edesc = data.edesc;
            this.selectedFile.isenable = data.isenable;
            this.selectedFile.fIndex = data.fIndex;
            this.selectedFile.img_url = data.img_url;
            
            if (data.fIndex !== undefined) {
              for (let i = 1; i <= 14; i++) {
                let _name = "";
                let _fileIndex = "";
                if (i <= 9) {
                  _fileIndex = "00" + i;
                  _name = data.fIndex + "00" + i + ".jpg";
                } else {
                  _fileIndex = "0" + i;
                  _name = data.fIndex + "0" + i + ".jpg";
                }
                let _selected = false;
                if (i === 8) {
                  this.selectedFile.defaultImg = _name;
                  _selected = true;
                } else {
                  _selected = false;
                }
                this.selectedFile.video_thumbs.push({
                  id: i,
                  filename: _name,
                  fileIndex: "img_" + _fileIndex,
                  selected: _selected
                });
              }
            }
            // update video output in reducer
            this.actions.updateVideoFile(this.selectedFile);
          } else {
            // $scope.ffmpeg.EncodingObj[$scope.ffmpeg.publishedFiles].isenabled = "checkfailed";
          }

          this.EncodingIndex++;
          this.publish();
        },
        err => {
          this.coreActions.Notify({
            title: "Error occured while encoding video",
            text: "",
            css: "bg-danger"
          });
        }
      );
  }

  finalOutput() {
    console.log("final output triggered");
    this.showSubmitBtn = true;
  }

  updateSelectedThumb(thumb, file, event) {
    file.defaultImg = thumb.filename;
    this.selectedFile.defaultImg = thumb.filename;
    // reset all thumbnail selection.
    if (this.selectedFile.video_thumbs.length > 0) {
      for (const thumbnail of this.selectedFile.video_thumbs) {
        if (thumbnail.id === thumb.id) {
          thumbnail.selected = true;
        } else {
          thumbnail.selected = false;
        }
      }
    }
    this.actions.updateVideoFile(this.selectedFile);
    event.stopPropagation();
  }

  nextPanel() {
    this.Panel = 1;
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
    if (this.UploadedFiles.length > 0) {
      for (const item of this.UploadedFiles) {
        item.tags = payload.tags;
        item.categories = this.coreService.returnSelectedCategoryArray(
          payload.categories
        );
        item.userid = this.Auth.User.id;
        item.id = 0;
        item.isenabled = 0;
      }
      this.processVideos(this.UploadedFiles);
    }
  }

  processVideos(videos: any) {
    this.showLoader = true;
    
    this.dataService.AddFFMPEGVideos(videos).subscribe(
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
            title: "All uploaded videos processed successfully",
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
