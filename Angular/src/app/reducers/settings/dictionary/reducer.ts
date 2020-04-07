/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  DictionaryAPIAction,
  DictionaryAPIActions,
  DictionaryBLL
} from "./actions";
import { IDictionaryState, DICTIONARY_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createDictionaryReducer() {
  return function dictionaryReducer(
    state: IDictionaryState = DICTIONARY_INITIAL_STATE,
    a: Action
  ): IDictionaryState {
    const action = a as DictionaryAPIAction;

    const bll = new DictionaryBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case DictionaryAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case DictionaryAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case DictionaryAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case DictionaryAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case DictionaryAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case DictionaryAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case DictionaryAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case DictionaryAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case DictionaryAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case DictionaryAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case DictionaryAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);
      // remove loader
      case DictionaryAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case DictionaryAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case DictionaryAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
