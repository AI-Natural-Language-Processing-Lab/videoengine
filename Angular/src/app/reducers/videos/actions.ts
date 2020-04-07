/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { dispatch } from "@angular-redux/store";
import { FluxStandardAction } from "flux-standard-action";
import { tassign } from "tassign";
import { IVideoState } from "./model";
import { CoreService } from "../../admin/core/coreService";
type Payload = any;
interface MetaData {}
export type VideoAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class VideoAPIActions {
  static readonly LOAD_STARTED = "VIDEO_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "VIDEO_LOAD_SUCCEEDED";
  
  static readonly LOAD_FAILED = "VIDEO_LOAD_FAILED";

  static readonly APPLY_CHANGES = "VIDEO_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED = "VIDEO_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "VIDEO_APPLY_CHANGES_SUCCEEDED";

  static readonly UPDATE_FILTER_OPTIONS = "VIDEO_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "VIDEO_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "VIDEO_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly UPDATE_CATEGORIES = "VIDEO_UPDATE_CATEGORIES";

  static readonly SELECT_ALL = "VIDEOS_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "VIDEOS_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "VIDEOS_ADD_RECORD";
  static readonly UPDATE_RECORD = "VIDEOS_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "VIDEOS_REMOVE_RECORD";

  static readonly UPDATE_USER = "VIDEOS_UPDATE_USER"; // update user info in filter options (to load logged in user data)

  // Youtube Uploader Actions
  static readonly LOAD_YT_CATEGORIES = "VIDEOS_YT_CATEGORIES";
  static readonly LOAD_YT_SEARCH = "VIDEOS_YT_SEARCH_RESULT";
  static readonly LOAD_END = "VIDEOS_YT_LOADEND";
  static readonly REMOVE_YT_SEARCH = "VIDEOS_RM_YT_SEARCH";
  // FFMPEG Uploader Actions
  static readonly UPLOADED_FILES = "VIDEO_UPLOADED_FILES";
  static readonly RESET_PUBLISHING = "VIDEO_RESET_PUBLISHING";
  static readonly UPDATE_VIDEO_FILE = "VIDEOS_UPDATE_SELECTED_FILE";

  // REFERESH LOAD
  static readonly REFRESH_DATA = "VIDEOS_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "VIDEOS_REFRESH_PAGINATION";
  
  // TRIGGER RELOAD
  static readonly UPDATE_TRIGGER_RELOAD = "VIDEOS_UPDATE_RELOAD";

  @dispatch()
  loadStarted = (): VideoAPIAction => ({
    type: VideoAPIActions.LOAD_STARTED,
    //  meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.LOAD_SUCCEEDED,
    //  meta: { },
    payload
  });

  @dispatch()
  toggleReload = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPDATE_TRIGGER_RELOAD,
    //  meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): VideoAPIAction => ({
    type: VideoAPIActions.LOAD_FAILED,
    //  meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.APPLY_CHANGES,
    //  meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.APPLY_CHANGES_SUCCEEDED,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): VideoAPIAction => ({
    type: VideoAPIActions.APPLY_CHANGES_SUCCEEDED,
    //  meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPDATE_FILTER_OPTIONS,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.APPLY_FILTER,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  updateCategories = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPDATE_CATEGORIES,
    //  meta: { },
    payload: payload
  });
  @dispatch()
  selectAll = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.SELECT_ALL,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.IS_ITEM_SELECTED,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.ADD_RECORD,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPDATE_RECORD,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  loadYoutubeCategories = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.LOAD_YT_CATEGORIES,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  loadYoutubeSearchResult = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.LOAD_YT_SEARCH,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): VideoAPIAction => ({
    type: VideoAPIActions.LOAD_END,
    //  meta: { },
    payload: null
  });

  @dispatch()
  cleanupYoutubeSearchResult = (): VideoAPIAction => ({
    type: VideoAPIActions.REMOVE_YT_SEARCH,
    //  meta: { },
    payload: null
  });

  @dispatch()
  uploadedVideoFiles = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPLOADED_FILES,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  resetPublishing = (): VideoAPIAction => ({
    type: VideoAPIActions.RESET_PUBLISHING,
    //  meta: { },
    payload: null
  });

  @dispatch()
  updateVideoFile = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPDATE_VIDEO_FILE,
    //  meta: { },
    payload: payload
  });
  @dispatch()
  reloadList = (): VideoAPIAction => ({
    type: VideoAPIActions.REFRESH_DATA,
    //  meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.REFRESH_PAGINATION,
    //  meta: { },
    payload: payload
  });

  @dispatch()
  updateFilter = (payload: Payload): VideoAPIAction => ({
    type: VideoAPIActions.UPDATE_USER,
    // meta: { },
    payload: payload
  });
}

export class VideosBLL {
  service = new CoreService();
  loadSucceeded(state: IVideoState, action: any) {

    let triggerreload = false;
    if (action.payload.posts.length > 0) {
      for (const item of action.payload.posts) {
        item.enc_id = this.service.encrypt(item.id);
        // enable reloading (refresh listing after few seconds untli publishing pending videos display result)
        if (item.ispublished === 0) {
           triggerreload = true;
        }
      }
    }
    // update totalrecords object in pagination prop
    const _pagination = state.pagination;
    _pagination.totalRecords = action.payload.records;
    _pagination.pageSize = state.filteroptions.pagesize;
    _pagination.currentPage = state.filteroptions.pagenumber;
    // avoid loading categories again in next call
    const _filteroption = state.filteroptions;
    _filteroption.loadstats = false;

    return tassign(state, {
      posts: action.payload.posts,
      settings: action.payload.settings,
      records: action.payload.records,
      pagination: _pagination,
      filteroptions: _filteroption,
      loading: false,
      isloaded: true,
      triggerreload: triggerreload
    });
  }


  applyFilterChanges(state: IVideoState, action: any) {
    const filters = state.filteroptions;
    for (const prop in filters) {
      if (prop === action.payload.attr) {
        filters[prop] = action.payload.value;
      }
    }
    filters.track_filter = true; // force filter subscriber to call loadRecord function to refresh data
    return tassign(state, {
      filteroptions: Object.assign({}, state.filteroptions, filters)
    });
  }

  updatePagination(state: IVideoState, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: IVideoState, action: any) {
    const posts = state.posts;
    for (const item of posts) {
      item.Selected = action.payload.checked;
    }
    return tassign(state, {
      selectall: action.payload.checked,
      posts: posts,
      itemsselected: action.payload.checked
    });
    
  }

  addRecord(state: IVideoState, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: IVideoState, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  updateVideoFile(state: IVideoState, action: any) {
    const files = state.uploadedfiles;
    for (const file of files) {
      if (file.id === action.payload.id) {
        for (const prop in file) {
          if (prop === action.payload.attr) {
            file[prop] = action.payload.value;
          }
        }
      }
    }
    return tassign(state, {
      filteroptions: Object.assign([], state.uploadedfiles, files)
    });
  }

  applyChanges(state: IVideoState, action: any) {
    let _updated_state = state.posts;
    for (const selected of action.payload.SelectedItems) {
      for (const item of _updated_state) {
        if (item.id === selected.id) {
          if (
            selected.actionstatus === "delete" ||
            selected.actionstatus === "delete_fav" ||
            selected.actionstatus === "delete_like" ||
            selected.actionstatus === "delete_playlist"
          ) {
            item.isdeleted = true;
          } else {
            for (const prop in item) {
              if (prop === action.payload.isenabled.attr) {
                item[prop] = action.payload.isenabled.value;
              }
            }
          }
        }
      }
    }
    return tassign(state, { posts: _updated_state });
   
  }

  updateUserFilter(state: IVideoState, action: any) {
    const filters = state.filteroptions;
    filters.userid = action.payload.id;
    return tassign(state, { filteroptions: filters });
  }

  refreshpagination(state: IVideoState, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
  
  }
}
