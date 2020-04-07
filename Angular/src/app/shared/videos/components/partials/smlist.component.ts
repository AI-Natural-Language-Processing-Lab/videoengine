/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { select } from "@angular-redux/store";
import { VideoAPIActions } from "../../../../reducers/videos/actions";
import { DataService } from "../../services/data.service";
import { SettingsService } from "../../services/settings.service";
import { Router } from "@angular/router";
import { CoreAPIActions } from "../../../../reducers/core/actions";
import { CoreService } from "../../../../admin/core/coreService";
@Component({
  selector: "app-smvideo-list",
  templateUrl: "./smlist.html",
  providers: [VideoAPIActions, DataService, SettingsService]
})
export class SMVideoListComponent implements OnInit {
  constructor(
    private actions: VideoAPIActions,
    private coreActions: CoreAPIActions,
    private dataService: DataService,
    private router: Router,
    private coreService: CoreService
  ) {}

  @Input() title = "My Videos";
  @Input() type = 0; // 0: My Videos, 1: Favorited Videos, 2: Liked Videos, 3: Playlist Videos
  @Input() mediaType = 0; // 0: videos, 1: audio
  @Input() browse_url = '/';
  @Input() rout_url = '/my-videos/';
  @Input() NoRecordText = "No Videos Uploaded Yet!";
  @Input() isAdmin = false;
  @Input() stats = 0; 
  @Input() row_class = "col-md-4 col-sm-6 col-xs-12";
  @Input() pagesize = 6;
  @Input() orderby = "video.created_at desc";
  @select(["users", "auth"])

  readonly auth$: Observable<any>;

  loaddata = false;
  Data: any =[];
  User: any = {};
  ngOnInit() {
    this.auth$.subscribe((auth: any) => {
       this.User = auth.User;
       this.LoadRecords(auth.User);
    });
     
  }

  LoadRecords(user: any) {
     const query: any = {
        order: this.orderby,
        pagesize: this.pagesize,
        isSummary: true,
        nofilter: false,
        ispublic: false,
        loadstats: false,
        ispublished: 2, // all
        isprivate: 3, // all
        isenabled: 2, // all (as this module will load all user own videos)
        isapproved: 2, // all (author specific)
     };
     if (this.type === 1) {
         // favorited
         query.loadfavorites = true;
     }
     if (!this.isAdmin) {
        query.userid = user.id;
     }
     if (this.mediaType === 1) {
        // audio
        query.type = 1;
     }
     this.loaddata = true;
     this.dataService.LoadSmList(query).subscribe(
        (data: any) => {
          this.Data = data.posts;
          for (const item of this.Data) {
            item.enc_id = this.coreService.encrypt(item.id);
          }
          this.loaddata = false;
        },
        err => {
          this.actions.loadFailed(err);
        }
      );
  }

  /*order(type) {
     if (type === 'mostviewed') {
        this.orderby = "video.views desc";
        this.LoadRecords(this.User);
     }
  }*/

}
