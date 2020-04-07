/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  AdvertisementAPIAction,
  AdvertisementAPIActions,
  AdvertisementBLL
} from "./actions";
import { IAdvertisementState, ADS_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createAdvertisementReducer() {
  return function advertisementReducer(
    state: IAdvertisementState = ADS_INITIAL_STATE,
    a: Action
  ): IAdvertisementState {
    const action = a as AdvertisementAPIAction;
    const bll = new AdvertisementBLL();
    /*if (!action.meta) {
      return state;
    }*/

    switch (action.type) {
      case AdvertisementAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case AdvertisementAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case AdvertisementAPIActions.LOAD_FAILED:
        return tassign(state, {
          posts: [],
          records: 0,
          loading: false,
          error: action.error
        });

      /* update wholefilter options */
      case AdvertisementAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case AdvertisementAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case AdvertisementAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      case AdvertisementAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);
      // remove loader
      case AdvertisementAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case AdvertisementAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case AdvertisementAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
