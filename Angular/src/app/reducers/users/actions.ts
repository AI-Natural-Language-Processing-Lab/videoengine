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
import { IUserPage } from "./model";
import { tassign } from "tassign";


type Payload = any;
interface MetaData {}
export type UserAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class UserAPIActions {
  static readonly LOAD_STARTED = "USERS_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "USERS_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "USERS_LOAD_FAILED";

  static readonly APPLY_CHANGES = "USERS_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED = "USERS_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "USERS_APPLY_CHANGES_SUCCEEDED";

  static readonly UPDATE_FILTER_OPTIONS = "USERS_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "USERS_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "USERS_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly UPDATE_CATEGORIES = "USERS_UPDATE_CATEGORIES";

  static readonly SELECT_ALL = "USERS_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "USERS_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "USERS_ADD_RECORD";
  static readonly UPDATE_RECORD = "USERS_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "USERS_REMOVE_RECORD";

  static readonly UPDATE_THUMB = "USERS_UPDATE_THUMB";

  /* Authentication */
  static readonly AUTHENTICATE = "USERS_AUTHENTICATE";
  static readonly SIGNOUT = "USERS_SIGNOUT";
  static readonly UPDATE_AUTH_THUMB = "USERS_UPDATE_AUTH_THUMB";
  // REFERESH LOAD
  static readonly LOAD_END = "USERS_YT_LOADEND";
  static readonly REFRESH_DATA = "USERS_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "USERS_REFRESH_PAGINATION";
  @dispatch()
  SignOut = (): UserAPIAction => ({
    type: UserAPIActions.SIGNOUT,
    // meta: { },
    payload: null
  });

  @dispatch()
  Authenticate = (payload): UserAPIAction => ({
    type: UserAPIActions.AUTHENTICATE,
    // meta: { },
    payload
  });

  @dispatch()
  UpdateThumb = (payload): UserAPIAction => ({
    type: UserAPIActions.UPDATE_THUMB,
    // meta: { },
    payload
  });

  @dispatch()
  loadStarted = (): UserAPIAction => ({
    type: UserAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): UserAPIAction => ({
    type: UserAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): UserAPIAction => ({
    type: UserAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateCategories = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.UPDATE_CATEGORIES,
    // meta: { },
    payload: payload
  });
  @dispatch()
  selectAll = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): UserAPIAction => ({
    type: UserAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): UserAPIAction => ({
    type: UserAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): UserAPIAction => ({
    type: UserAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });

  @dispatch()
  UpdateAuthThumb = (payload): UserAPIAction => ({
    type: UserAPIActions.UPDATE_AUTH_THUMB,
    // meta: { },
    payload
  });
}

export class UserBLL {
  // update user auth object
  updateAuthThumb(state: IUserPage, action: any) {
    const auth_object = state.auth;
    auth_object.User.picturename = action.payload.picturename;
    auth_object.User.img_url = action.payload.img_url;

    return tassign(state, { auth: Object.assign({}, state.auth, auth_object) });
  }

  updateThumb(state: IUserPage, action: any) {
    const posts = state.posts;
    for (const post of posts) {
      if (post.id === action.payload.id) {
        post.picturename = action.payload.picturename;
        post.img_url = action.payload.img_url;
      }
    }

    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  loadSucceeded(state: IUserPage, action: any) {
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

  applyFilterChanges(state: IUserPage, action: any) {
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

  updatePagination(state: IUserPage, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: IUserPage, action: any) {
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

  addRecord(state: IUserPage, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: IUserPage, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  /*  removeRecord(state: IUserPage, action: any) {
      const posts = state.posts;
      console.log('remove record');
      console.log(action.payload);

      if (action.payload.index > -1) {
         posts.splice(action.payload.index, 1);
      }
      return tassign(state, { posts: Object.assign([], state.posts, posts) });
  } */

  applyChanges(state: IUserPage, action: any) {
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

  refreshpagination(state: IUserPage, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
