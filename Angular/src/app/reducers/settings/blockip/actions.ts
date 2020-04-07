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
import { IBlockIPState } from "./model";
import { tassign } from "tassign";


type Payload = any;
interface MetaData {}
export type BlockIPAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class BlockIPAPIActions {
  static readonly LOAD_STARTED = "BLOCK_IP_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "BLOCK_IP_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "BLOCK_IP_LOAD_FAILED";

  static readonly APPLY_CHANGES = "BLOCK_IP_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED = "BLOCK_IP_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "BLOCK_IP_APPLY_CHANGES_FAILED";

  static readonly UPDATE_FILTER_OPTIONS = "BLOCK_IP_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "BLOCK_IP_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "BLOCK_IP_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly SELECT_ALL = "BLOCK_IP_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "BLOCK_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "BLOCK_IP_ADD_RECORD";
  static readonly UPDATE_RECORD = "BLOCK_IP_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "BLOCK_IP_REMOVE_RECORD";

  // REFERESH LOAD
  static readonly LOAD_END = "BLOCK_YT_LOADEND";
  static readonly REFRESH_DATA = "BLOCK_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "BLOCK_REFRESH_PAGINATION";

  @dispatch()
  loadStarted = (): BlockIPAPIAction => ({
    type: BlockIPAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): BlockIPAPIAction => ({
    type: BlockIPAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): BlockIPAPIAction => ({
    type: BlockIPAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  selectAll = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): BlockIPAPIAction => ({
    type: BlockIPAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): BlockIPAPIAction => ({
    type: BlockIPAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): BlockIPAPIAction => ({
    type: BlockIPAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class BlockIPBLL {
  loadSucceeded(state: IBlockIPState, action: any) {
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

  applyFilterChanges(state: IBlockIPState, action: any) {
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

  updatePagination(state: IBlockIPState, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: IBlockIPState, action: any) {
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

  addRecord(state: IBlockIPState, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: IBlockIPState, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  applyChanges(state: IBlockIPState, action: any) {
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

  refreshpagination(state: IBlockIPState, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
