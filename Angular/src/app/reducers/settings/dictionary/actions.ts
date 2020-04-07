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
import { IDictionaryState } from "./model";
type Payload = any;
interface MetaData {}
export type DictionaryAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class DictionaryAPIActions {
  static readonly LOAD_STARTED = "DICTIONARY_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "DICTIONARY_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "DICTIONARY_LOAD_FAILED";

  static readonly APPLY_CHANGES = "DICTIONARY_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED =
    "DICTIONARY_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "DICTIONARY_APPLY_CHANGES_SUCCEEDED";

  static readonly UPDATE_FILTER_OPTIONS = "DICTIONARY_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "DICTIONARY_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "DICTIONARY_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly UPDATE_CATEGORIES = "DICTIONARY_UPDATE_CATEGORIES";

  static readonly SELECT_ALL = "DICTIONARY_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "DICTIONARY_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "DICTIONARY_ADD_RECORD";
  static readonly UPDATE_RECORD = "DICTIONARY_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "DICTIONARY_REMOVE_RECORD";

  // REFERESH LOAD
  static readonly LOAD_END = "DICTIONARY_YT_LOADEND";
  static readonly REFRESH_DATA = "DICTIONARY_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "DICTIONARY_REFRESH_PAGINATION";

  @dispatch()
  loadStarted = (): DictionaryAPIAction => ({
    type: DictionaryAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): DictionaryAPIAction => ({
    type: DictionaryAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): DictionaryAPIAction => ({
    type: DictionaryAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateCategories = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.UPDATE_CATEGORIES,
    // meta: { },
    payload: payload
  });
  @dispatch()
  selectAll = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): DictionaryAPIAction => ({
    type: DictionaryAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): DictionaryAPIAction => ({
    type: DictionaryAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): DictionaryAPIAction => ({
    type: DictionaryAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class DictionaryBLL {
  loadSucceeded(state: IDictionaryState, action: any) {
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

  applyFilterChanges(state: IDictionaryState, action: any) {
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

  updatePagination(state: IDictionaryState, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: IDictionaryState, action: any) {
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

  addRecord(state: IDictionaryState, action: any) {
    const posts = state.posts;
    const pagination = state.pagination;
    pagination.totalRecords = pagination.totalRecords + 1;
    posts.unshift(action.payload);
    /* const coreActions = new CoreAPIActions();
    coreActions.refreshListStats(
      { totalrecords: pagination.totalRecords,
        pagesize:  pagination.pagesize,
        pagenumber: pagination.pagenumber
      }); */
    return tassign(state, {
      posts: posts,
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  updateRecord(state: IDictionaryState, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  applyChanges(state: IDictionaryState, action: any) {
    const _updated_state = state.posts;
    for (const selected of action.payload.SelectedItems) {
      for (const item of _updated_state) {
        if (item.id === selected.id) {
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
    return tassign(state, { posts: _updated_state });
  }

  refreshpagination(state: IDictionaryState, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
