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
import { IAbuseReportStats } from "./model";
import { tassign } from "tassign";
import { CoreService } from "../../../admin/core/coreService";

type Payload = any;
interface MetaData {}
export type AbuseReportAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class AbuseReportActions {


  static readonly LOAD_STARTED = "ABUSEREPORT_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "ABUSEREPORT_LOAD_SUCCEEDED";
  static readonly LOAD_ABUSEREPORT_SUCCEEDED = "ABUSEREPORT_NOALBUM_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "ABUSEREPORT_LOAD_FAILED";
  static readonly APPLY_CHANGES = "ABUSEREPORT_APPLY_CHANGES";
  static readonly APPLY_ABUSEREPORT_CHANGES = "ABUSEREPORT_NOALBUM_APPLY_CHANGES";
  static readonly UPDATE_FILTER_OPTIONS = "ABUSEREPORT_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "ABUSEREPORT_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "ABUSEREPORT_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly SELECT_ALL = "ABUSEREPORT_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "ABUSEREPORT_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "ABUSEREPORT_ADD_RECORD";
  static readonly UPDATE_RECORD = "ABUSEREPORT_UPDATE_RECORD";
  static readonly UPDATE_USER = "ABUSEREPORT_UPDATE_USER"; // update user info in filter options (to load logged in user data)
  static readonly LOAD_END = "ABUSEREPORT_YT_LOADEND";
  // REFERESH LOAD
  static readonly REFRESH_DATA = "ABUSEREPORT_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "ABUSEREPORT_REFRESH_PAGINATION";
  static readonly REFRESH_ABUSEREPORT_PAGINATION =
    "ABUSEREPORT_NOALBUM_REFRESH_PAGINATION";

  @dispatch()
  loadStarted = (): AbuseReportAction => ({
    type: AbuseReportActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadPhotosSucceeded = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.LOAD_ABUSEREPORT_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): AbuseReportAction => ({
    type: AbuseReportActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  applyPhotoChanges = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.APPLY_ABUSEREPORT_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  selectAll = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): AbuseReportAction => ({
    type: AbuseReportActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): AbuseReportAction => ({
    type: AbuseReportActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });

  @dispatch()
  refresh_ABUSEREPORT_pagination = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.REFRESH_ABUSEREPORT_PAGINATION,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateFilter = (payload: Payload): AbuseReportAction => ({
    type: AbuseReportActions.UPDATE_USER,
    // meta: { },
    payload: payload
  });
}

export class AbuseReportBLL {
  
  service = new CoreService();

  loadSucceeded(state: IAbuseReportStats, action: any) {
    if (action.payload.posts.length > 0) {
      for (const item of action.payload.posts) {
        item.enc_id = this.service.encrypt(item.id);
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
      records: action.payload.records,
      pagination: _pagination,
      filteroptions: _filteroption,
      loading: false,
      isloaded: true
    });
  }

  updateUserFilter(state: IAbuseReportStats, action: any) {
    const filters = state.filteroptions;
    filters.userid = action.payload.id;
    return tassign(state, {
      filteroptions: filters
    });
  }

  applyFilterChanges(state: IAbuseReportStats, action: any) {
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

  updatePagination(state: IAbuseReportStats, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: IAbuseReportStats, action: any) {
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

  addRecord(state: IAbuseReportStats, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: IAbuseReportStats, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  applyChanges(state: IAbuseReportStats, action: any) {
    let _updated_state = state.posts;
    for (const selected of action.payload.SelectedItems) {
      for (const item of _updated_state) {
        if (item.id === selected.id) {
          if (
            selected.actionstatus === "delete"
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

  refreshpagination(state: IAbuseReportStats, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
  }
}
