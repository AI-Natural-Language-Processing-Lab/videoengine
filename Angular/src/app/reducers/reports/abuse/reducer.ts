/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  AbuseReportAction,
  AbuseReportActions,
  AbuseReportBLL
} from "./actions";
import { IAbuseReportStats, ABUSE_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createAbuseReportReducer() {
  return function abuseRepordReducer(
    state: IAbuseReportStats = ABUSE_INITIAL_STATE,
    a: Action
  ): IAbuseReportStats {
    const action = a as AbuseReportAction;

    const bll = new AbuseReportBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case AbuseReportActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case AbuseReportActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case AbuseReportActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case AbuseReportActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case AbuseReportActions.UPDATE_USER:
        return bll.updateUserFilter(state, action);

      case AbuseReportActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case AbuseReportActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case AbuseReportActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case AbuseReportActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case AbuseReportActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case AbuseReportActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case AbuseReportActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);

      // remove loader
      case AbuseReportActions.LOAD_END:
        return tassign(state, { loading: false });

      case AbuseReportActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case AbuseReportActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
