
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
import { ITagState } from "./model";

type Payload = any;
interface MetaData {}
export type TagsAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class TagsAPIActions {
  static readonly LOAD_STARTED = "TAGS_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "TAGS_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "TAGS_LOAD_FAILED";

  static readonly APPLY_CHANGES = "TAGS_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED = "TAGS_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "TAGS_APPLY_CHANGES_SUCCEEDED";

  static readonly UPDATE_FILTER_OPTIONS = "TAGS_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "TAGS_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "TAGS_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly UPDATE_CATEGORIES = "TAGS_UPDATE_CATEGORIES";

  static readonly SELECT_ALL = "TAGS_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "TAGS_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "TAGS_ADD_RECORD";
  static readonly UPDATE_RECORD = "TAGS_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "TAGS_REMOVE_RECORD";

  // REFERESH LOAD
  static readonly LOAD_END = "TAGS_YT_LOADEND";
  static readonly REFRESH_DATA = "TAGS_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "TAGS_REFRESH_PAGINATION";
  @dispatch()
  loadStarted = (): TagsAPIAction => ({
    type: TagsAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): TagsAPIAction => ({
    type: TagsAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): TagsAPIAction => ({
    type: TagsAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateCategories = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.UPDATE_CATEGORIES,
    // meta: { },
    payload: payload
  });
  @dispatch()
  selectAll = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): TagsAPIAction => ({
    type: TagsAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): TagsAPIAction => ({
    type: TagsAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): TagsAPIAction => ({
    type: TagsAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class TagsBLL {
  loadSucceeded(state: ITagState, action: any) {
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

  applyFilterChanges(state: ITagState, action: any) {
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

  updatePagination(state: ITagState, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: ITagState, action: any) {
    const posts = state.posts;
    for (const item of posts) {
      item.Selected = action.payload;
    }

    return tassign(state, {
      selectall: action.payload,
      posts: posts,
      itemsselected: action.payload
    });
  }

  addRecord(state: ITagState, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: ITagState, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  applyChanges(state: ITagState, action: any) {
    const _updated_state = state.posts;
    for (const selected of action.payload.SelectedItems) {
      if (action.payload.isenabled.action === "togglestatus") {
        for (const item of _updated_state) {
          if (item.id === selected.id) {
            if (item.isenabled === 1) {
              item.isenabled = 0;
            } else {
              item.isenabled = 1;
            }
          }
        }
      } else {
        console.log("delete isenabled tracking");
        for (const item of _updated_state) {
          console.log(item);
          if (item.id === selected.id) {
            console.log("matached");
            console.log(item);
            console.log(selected.actionstatus);
            if (selected.actionstatus === "delete") {
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
    }
    return tassign(state, { posts: _updated_state });
  }

  refreshpagination(state: ITagState, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
