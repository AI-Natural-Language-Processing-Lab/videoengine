/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { LanguageAPIAction, LanguageAPIActions, LanguageBLL } from "./actions";
import { ILanguageState, LANGUAGE_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createLanguageReducer() {
  return function languagereducer(
    state: ILanguageState = LANGUAGE_INITIAL_STATE,
    a: Action
  ): ILanguageState {
    const action = a as LanguageAPIAction;

    const bll = new LanguageBLL();
    switch (action.type) {
      case LanguageAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case LanguageAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case LanguageAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case LanguageAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case LanguageAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case LanguageAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case LanguageAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case LanguageAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case LanguageAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case LanguageAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case LanguageAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);
      // remove loader
      case LanguageAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case LanguageAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case LanguageAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
