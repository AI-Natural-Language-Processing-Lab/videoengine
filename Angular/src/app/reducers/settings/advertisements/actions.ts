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
import { IAdvertisementState } from "./model";
import { tassign } from "tassign";

type Payload = any;
interface MetaData {}
export type AdvertisementAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class AdvertisementAPIActions {
  static readonly LOAD_STARTED = "ADVERITESEMENT_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "ADVERITESEMENT_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "ADVERITESEMENT_LOAD_FAILED";

  static readonly APPLY_CHANGES = "ADVERITESEMENT_APPLY_CHANGES";
  static readonly APPLY_CHANGES_SUCCEEDED =
    "ADVERITESEMENT_APPLY_CHANGES_SUCCEEDED";
  static readonly APPLY_CHANGES_FAILED = "ADVERITESEMENT_APPLY_CHANGES_FAILED";

  static readonly UPDATE_FILTER_OPTIONS =
    "ADVERITESEMENT_UPDATE_FILTER_OPTIONS";
  static readonly APPLY_FILTER = "ADVERITESEMENT_APPLY_FILTER";
  static readonly UPDATE_PAGINATION_CURRENTPAGE =
    "ADVERITESEMENT_UPDATE_PAGINATION_CURRENTPAGE";

  static readonly UPDATE_RECORD = "ADVERTISEMENT_UPDATE_RECORD";

  // REFERESH LOAD
  static readonly LOAD_END = "ADVERTISEMENT_YT_LOADEND";
  static readonly REFRESH_DATA = "ADVERTISEMENT_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "ADVERTISEMENT_REFRESH_PAGINATION";

  @dispatch()
  loadStarted = (): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  applyChanges = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.APPLY_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  actionSucceeded = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  actionFailed = (error): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.APPLY_CHANGES_SUCCEEDED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  updateFilterOptions = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.UPDATE_FILTER_OPTIONS,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyFilter = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.APPLY_FILTER,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updatePaginationCurrentPage = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.UPDATE_PAGINATION_CURRENTPAGE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateRecord = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.UPDATE_RECORD,
    // meta: { },
    payload: payload
  });

  @dispatch()
  loadEnd = (): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): AdvertisementAPIAction => ({
    type: AdvertisementAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class AdvertisementBLL {
  loadSucceeded(state: IAdvertisementState, action: any) {
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

  applyFilterChanges(state: IAdvertisementState, action: any) {
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

  updatePagination(state: IAdvertisementState, action: any) {
    const pagination = state.pagination;
    pagination.currentPage = action.payload.currentpage;

    return tassign(state, {
      pagination: Object.assign({}, state.pagination, pagination)
    });
  }

  updateRecord(state: IAdvertisementState, action: any) {
    const ads = state.posts;
    for (let ad of ads) {
      if (ad.id === action.payload.id) {
        ad = Object.assign({}, ad, action.payload);
      }
    }
    return tassign(state, { posts: Object.assign([], state.posts, ads) });
  }

  refreshpagination(state: IAdvertisementState, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
