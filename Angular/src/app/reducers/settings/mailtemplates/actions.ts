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

import { IMailtemplateStates } from "./model";
type Payload = any;
interface MetaData {}
export type MailTemplatesAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class MailTemplatesAPIActions {
  static readonly LOAD_STARTED = "MAIL_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "MAIL_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "MAIL_LOAD_FAILED";

  static readonly APPLY_CHANGES = "MAIL_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED = "MAIL_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "MAIL_APPLY_CHANGES_SUCCEEDED";

  static readonly UPDATE_FILTER_OPTIONS = "MAIL_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "MAIL_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "MAIL_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly UPDATE_CATEGORIES = "MAIL_UPDATE_CATEGORIES";

  static readonly SELECT_ALL = "MAIL_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "MAIL_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "MAIL_ADD_RECORD";
  static readonly UPDATE_RECORD = "MAIL_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "MAIL_REMOVE_RECORD";

  // REFERESH LOAD
  static readonly LOAD_END = "MAIL_YT_LOADEND";
  static readonly REFRESH_DATA = "MAIL_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "MAIL_REFRESH_PAGINATION";

  @dispatch()
  loadStarted = (): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateCategories = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.UPDATE_CATEGORIES,
    // meta: { },
    payload: payload
  });
  @dispatch()
  selectAll = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): MailTemplatesAPIAction => ({
    type: MailTemplatesAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class MailTemplatesBLL {
  loadSucceeded(state: IMailtemplateStates, action: any) {
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

  applyFilterChanges(state: IMailtemplateStates, action: any) {
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

  updatePagination(state: IMailtemplateStates, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: IMailtemplateStates, action: any) {
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

  addRecord(state: IMailtemplateStates, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: IMailtemplateStates, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  applyChanges(state: IMailtemplateStates, action: any) {
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

  refreshpagination(state: IMailtemplateStates, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
