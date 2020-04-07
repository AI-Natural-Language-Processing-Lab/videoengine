/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { VideoAPIActions } from "../../../reducers/videos/actions";
import { HttpClient } from "@angular/common/http";

import { SettingsService } from "./settings.service";
import { CoreAPIActions } from "../../../reducers/core/actions";

@Injectable()
export class DataService {
  constructor(
    private settings: SettingsService,
    private http: HttpClient,
    private actions: VideoAPIActions,
    private coreActions: CoreAPIActions
  ) {}

  LoadRecords(FilterOptions) {
    /*if (!isadmin) {
      FilterOptions.ispublic = true;
    }*/
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

  /* -------------------------------------------------------------------------- */
  /*                       load few videos (no pagination)                      */
  /* -------------------------------------------------------------------------- */
  LoadSmList(queryOptions: any) {
  
    return this.http.post(this.settings.getApiOptions().load, JSON.stringify(queryOptions));

  }

  /* -------------------------------------------------------------------------- */
  /*                       load reports (no pagination)                      */
  /* -------------------------------------------------------------------------- */
  LoadReports(queryOptions: any) {
  
    return this.http.post(this.settings.getApiOptions().load_reports, JSON.stringify(queryOptions));

  }

  LoadYoutubeCategories() {
    const URL = this.settings.getApiOptions().youtube;
    this.http.post(URL, {}).subscribe(
      (data: any) => {
        this.actions.loadYoutubeCategories(data.categorylist);
      },
      err => {
        this.actions.loadFailed(err);
      }
    );
  }

  SearchYoutube(FilterOptions) {
    const URL = this.settings.getApiOptions().fetch_youtube;
    this.actions.loadStarted();
    this.http.post(URL, JSON.stringify(FilterOptions)).subscribe(
      (data: any) => {
        // update core data
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-success"
          });
        } else {
          this.actions.loadYoutubeSearchResult(data.posts);
        }
      },
      err => {
        this.actions.loadFailed(err);
      }
    );
  }

  AddYoutubeVideos(arr) {
    return this.http.post(
      this.settings.getApiOptions().yt_proc,
      JSON.stringify(arr)
    );
  }

  AddFFMPEGVideos(arr) {
    return this.http.post(
      this.settings.getApiOptions().proc_ffmpeg_videos,
      JSON.stringify(arr)
    );
  }

  AddAWSVideos(arr) {
    return this.http.post(
      this.settings.getApiOptions().aws_proc,
      JSON.stringify(arr)
    );
  }

  AddMovie(arr) {
    return this.http.post(
      this.settings.getApiOptions().movie_proc,
      JSON.stringify(arr)
    );
  }

  EmbedVideo(arr) {
    return this.http.post(
      this.settings.getApiOptions().embed_proc,
      JSON.stringify(arr)
    );
  }

  EncodeVideo(obj) {
    return this.http.post(
      this.settings.getApiOptions().encodevideo,
      JSON.stringify([obj])
    );
  }

  DirectVideo(obj) {
    return this.http.post(
      this.settings.getApiOptions().direct_proc,
      JSON.stringify(obj)
    );
  }

  ProcessRecord(obj) {
    return this.http.post(
      this.settings.getApiOptions().update_video_info,
      JSON.stringify(obj)
    );
  }

  EditRecord(obj) {
    return this.http.post(
      this.settings.getApiOptions().editvideo,
      JSON.stringify(obj)
    );
  }

  DeletePhotos(obj) {
    return this.http.post(
      this.settings.getApiOptions().deletevideo,
      JSON.stringify(obj)
    );
  }
  UpdateThumbnail(obj) {
    return this.http.post(
      this.settings.getApiOptions().update_video_thumb,
      JSON.stringify(obj)
    );
  }
  Authorize_Author(obj) {
    return this.http.post(
      this.settings.getApiOptions().authorize_author,
      JSON.stringify(obj)
    );
  }
  /* -------------------------------------------------------------------------- */
  /*                  Get Single Record  (for preview)                          */
  /* -------------------------------------------------------------------------- */
  GetInfo(id: number) {
    const URL = this.settings.getApiOptions().getinfo;
    return this.http.post(URL, JSON.stringify({ id }));
  }

  /* -------------------------------------------------------------------------- */
  /*                              Get Single Record                             */
  /* -------------------------------------------------------------------------- */
  GetVideoInfo(id: number) {
    const URL = this.settings.getApiOptions().getinfo_acc;
    return this.http.post(URL, JSON.stringify({ id }));
  }
  
  /* DeleteRecord(item, index, type, userid) {
    console.log('hit again');
    if (type > 0) {
      item.userid = userid;
    }
    //if (this.config.getGlobalVar('isdelete')) {
    item.actionstatus = "delete";
    switch (type) {
      case 1:
        item.actionstatus = "delete_fav";
        break;
      case 2:
        item.actionstatus = "delete_like";
        break;
      case 3:
        item.actionstatus = "delete_playlist";
        break;
    }
    const arr = [];
    arr.push(item);
    this.ProcessActions(arr, "delete", type);
    // }
  } */

  ProcessActions(SelectedItems, isenabled, type: number) {
    // apply changes directory instate
    this.actions.applyChanges({
      SelectedItems,
      isenabled,
      type: type
    });

    this.http
      .post(this.settings.getApiOptions().action, JSON.stringify(SelectedItems))
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
}
