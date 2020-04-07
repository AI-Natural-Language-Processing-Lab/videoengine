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
import { SettingsService } from "../../services/settings.service";
import { DataService } from "../../services/data.service";
import { FormService } from "../../services/form.service";

// shared services
import { CoreService } from "../../../../admin/core/coreService";
import { CoreAPIActions } from "../../../../reducers/core/actions";

// reducer actions
import { VideoAPIActions } from "../../../../reducers/videos/actions";
import { fadeInAnimation } from "../../../../animations/core";

import { PermissionService } from "../../../../admin/users/services/permission.service";

@Component({
  templateUrl: "./updatethumbnail.html",
  selector: "app-video-updatethumb",
  animations: [fadeInAnimation]
})
export class UpdateVideoThumbComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: VideoAPIActions,
    private route: ActivatedRoute,
    private permission: PermissionService,
    private formService: FormService,
    private router: Router
  ) {}

  @Input() isAdmin = true;
  @Input() route_path = '/videos/';

  RecordID = 0;
  showLoader = false;
  formHeading = "Update Video Cover";
  Info: any = {
    video_thumbs: []
  };
  IsLoaded = false;
  Auth: any = {};


  @select(["videos", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["videos", "settings"])
  readonly settings$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete
  Settings: any = {};
  ngOnInit() {
    this.settings$.subscribe((settings: any) => {
      this.Settings = settings;
   });
    // user authentication & access right management
    // full resource access key and readonly key can be generated via roles management
    this.auth$.subscribe((auth: any) => {
      this.Auth = auth;
      if (this.isAdmin) {
        const FullAccessID = "1521395965196";
        const ReadOnlyAccessID = "1521396022188";
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
      } else {
        this.isAccessGranted = true;
        this.isActionGranded = true;
      }
    });

    // fetch param from url
    this.route.params.subscribe(params => {
      this.RecordID = this.coreService.decrypt(params["id"]);

      if (isNaN(this.RecordID)) {
        this.RecordID = 0;
      }

      if (this.RecordID > 0) {
        this.Initialize();
      } else {
          this.Redirect();
      }
    });

    this.isloaded$.subscribe((loaded: boolean) => {
      this.IsLoaded = loaded;
      if (!this.IsLoaded) {
         this.Redirect();
      }
    });

  }

  Initialize() {
    if (this.RecordID > 0) {
      if (!this.isAdmin) {
        // check whether current user is author of existing video before allowing edit
        this.dataService
          .Authorize_Author({
            id: this.RecordID,
            userid: this.Auth.User.id
          })
          .subscribe(
            (authorize: any) => {
              if (authorize.isaccess) {
                // load info
                this.LoadInfo();
              } else {
                this.Redirect();
              }
            },
            err => {
              this.Redirect();
            }
          );
      } else {
        // admin access
        // skip authorization
        this.LoadInfo();
      }
    } else {
      this.Redirect();
    }
  }

  Redirect() {
    this.router.navigate([this.route_path]);
  }

  LoadInfo() {
    this.showLoader = true;
    
    this.dataService.GetVideoInfo(this.RecordID).subscribe((data: any) => {
      if (data.status === "success") {
        // update post
        this.initializeControls(data.post);
      } else {
        this.coreActions.Notify({
          title: data.message,
          text: "",
          css: "bg-error"
        });
        this.initializeControls(this.settingService.getInitObject());
      }
      this.showLoader = false;
    });
  }

  initializeControls(data: any) {
    this.Info = data;

    //console.log(this.Info);
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
      userid: this.Info.userid
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
          
          this.router.navigate([this.route_path]);

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
