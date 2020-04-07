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

import { ILanguageState } from "./model";
type Payload = any;
interface MetaData {}
export type LanguageAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class LanguageAPIActions {
  static readonly LOAD_STARTED = "LANGUAGE_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "LANGUAGE_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "LANGUAGE_LOAD_FAILED";

  static readonly APPLY_CHANGES = "LANGUAGE_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED = "LANGUAGE_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "LANGUAGE_APPLY_CHANGES_SUCCEEDED";

  static readonly UPDATE_FILTER_OPTIONS = "LANGUAGE_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "LANGUAGE_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "LANGUAGE_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly UPDATE_CATEGORIES = "LANGUAGE_UPDATE_CATEGORIES";

  static readonly SELECT_ALL = "LANGUAGE_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "LANGUAGE_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "LANGUAGE_ADD_RECORD";
  static readonly UPDATE_RECORD = "LANGUAGE_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "LANGUAGE_REMOVE_RECORD";

  // REFERESH LOAD
  static readonly LOAD_END = "LANGUAGE_YT_LOADEND";
  static readonly REFRESH_DATA = "LANGUAGE_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "LANGUAGE_REFRESH_PAGINATION";

  @dispatch()
  loadStarted = (): LanguageAPIAction => ({
    type: LanguageAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): LanguageAPIAction => ({
    type: LanguageAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): LanguageAPIAction => ({
    type: LanguageAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateCategories = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.UPDATE_CATEGORIES,
    // meta: { },
    payload: payload
  });
  @dispatch()
  selectAll = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): LanguageAPIAction => ({
    type: LanguageAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): LanguageAPIAction => ({
    type: LanguageAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): LanguageAPIAction => ({
    type: LanguageAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class LanguageBLL {
  loadSucceeded(state: ILanguageState, action: any) {
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
      isloaded: false
    });
  }

  applyFilterChanges(state: ILanguageState, action: any) {
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

  updatePagination(state: ILanguageState, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: ILanguageState, action: any) {
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

  addRecord(state: ILanguageState, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: ILanguageState, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  applyChanges(state: ILanguageState, action: any) {
    const _updated_state = state.posts;
    for (const selected of action.payload.SelectedItems) {
      switch (action.payload.isenabled.action) {
        case "default":
          console.log("default selected");
          for (const item of _updated_state) {
            if (item.id === selected.id) {
              item.isdefault = 1;
            } else {
              item.isdefault = 0;
            }
          }
          break;
        case "selected":
          console.log("selected selected");
          for (const item of _updated_state) {
            if (item.id === selected.id) {
              if (item.isselected === 1) {
                item.isselected = 0;
              } else {
                item.isselected = 1;
              }
            }
          }
          break;
      }
    }
    return tassign(state, { posts: _updated_state });
  }

  refreshpagination(state: ILanguageState, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
