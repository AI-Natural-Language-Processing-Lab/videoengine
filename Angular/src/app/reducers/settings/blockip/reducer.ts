/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { BlockIPBLL, BlockIPAPIAction, BlockIPAPIActions } from "./actions";
import { IBlockIPState, BLOCK_IP_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createBlockIPReducer() {
  return function createblockIPReducer(
    state: IBlockIPState = BLOCK_IP_INITIAL_STATE,
    a: Action
  ): IBlockIPState {
    const action = a as BlockIPAPIAction;
    const bll = new BlockIPBLL();
    /*if (!action.meta) {
      return state;
    }*/

    switch (action.type) {
      case BlockIPAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case BlockIPAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case BlockIPAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case BlockIPAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case BlockIPAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case BlockIPAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case BlockIPAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case BlockIPAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case BlockIPAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case BlockIPAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      // remove loader
      case BlockIPAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case BlockIPAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case BlockIPAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case BlockIPAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);
    }

    return state;
  };
}
