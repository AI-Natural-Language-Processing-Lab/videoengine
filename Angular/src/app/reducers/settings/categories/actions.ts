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

import { ICategoriesState } from "./model";

type Payload = any;
interface MetaData {}
export type CategoriesAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class CategoriesAPIActions {
  static readonly LOAD_STARTED = "CATEGORIES_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "CATEGORIES_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "CATEGORIES_LOAD_FAILED";

  static readonly APPLY_CHANGES = "CATEGORIES_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED =
    "CATEGORIES_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "CATEGORIES_APPLY_CHANGES_SUCCEEDED";

  static readonly UPDATE_FILTER_OPTIONS = "CATEGORIES_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "CATEGORIES_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "CATEGORIES_UPDATE_PAGINATION_CURRENTPAGE";
  static readonly UPDATE_CATEGORIES = "CATEGORIES_UPDATE_CATEGORIES";

  static readonly SELECT_ALL = "CATEGORIES_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "CATEGORIES_IP_IS_ITEM_SELECTED";

  static readonly ADD_RECORD = "CATEGORIES_ADD_RECORD";
  static readonly UPDATE_RECORD = "CATEGORIES_UPDATE_RECORD";
  static readonly REMOVE_RECORD = "CATEGORIES_REMOVE_RECORD";

  static readonly LOAD_DROPDOWN_CATEGORIES = "CATEGORIES_LOAD_DROPDOWN";

  // REFERESH LOAD
  static readonly LOAD_END = "CATEGORIES_YT_LOADEND";
  static readonly REFRESH_DATA = "CATEGORIES_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "CATEGORIES_REFRESH_PAGINATION";

  @dispatch()
  loadDropdownCategories = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.LOAD_DROPDOWN_CATEGORIES,
    // meta: { },
    payload
  });

  @dispatch()
  loadStarted = (): CategoriesAPIAction => ({
    type: CategoriesAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): CategoriesAPIAction => ({
    type: CategoriesAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): CategoriesAPIAction => ({
    type: CategoriesAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateCategories = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.UPDATE_CATEGORIES,
    // meta: { },
    payload: payload
  });

  @dispatch()
  selectAll = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRecord = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.ADD_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): CategoriesAPIAction => ({
    type: CategoriesAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): CategoriesAPIAction => ({
    type: CategoriesAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): CategoriesAPIAction => ({
    type: CategoriesAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class CategoriesBLL {
  loadSucceeded(state: ICategoriesState, action: any) {
    const categories = action.payload.posts;
    for (const category of categories) {
      if (
        category.level !== undefined &&
        category.level !== null &&
        category.level !== ""
      ) {
        const dots = category.level.match(/\./g) || [];
        const total_dots = dots.length;
        console.log(category.level + " includes " + total_dots + " dots");
        for (let i = 0; i <= total_dots - 1; i++) {
          category.title = "-" + category.title;
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
      records: action.payload.records,
      pagination: _pagination,
      filteroptions: _filteroption,
      loading: false,
      isloaded: true
    });
  }

  loadDropdownCategories(state: ICategoriesState, action: any) {
    return tassign(state, {
      dropdown_categories: action.payload.posts
    });
  }
  applyFilterChanges(state: ICategoriesState, action: any) {
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

  updatePagination(state: ICategoriesState, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  selectAll(state: ICategoriesState, action: any) {
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

  addRecord(state: ICategoriesState, action: any) {
    const posts = state.posts;
    posts.unshift(action.payload);
    return tassign(state, { posts: posts });
  }

  updateRecord(state: ICategoriesState, action: any) {
    const posts = state.posts;
    for (let post of posts) {
      if (post.id === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, posts) });
  }

  applyChanges(state: ICategoriesState, action: any) {
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

  refreshpagination(state: ICategoriesState, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
