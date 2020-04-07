/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  CategoriesAPIAction,
  CategoriesAPIActions,
  CategoriesBLL
} from "./actions";
import { ICategoriesState, CATEGORIES_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createCategoriesReducer() {
  return function categoryReducer(
    state: ICategoriesState = CATEGORIES_INITIAL_STATE,
    a: Action
  ): ICategoriesState {
    const action = a as CategoriesAPIAction;
    const bll = new CategoriesBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case CategoriesAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case CategoriesAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case CategoriesAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case CategoriesAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case CategoriesAPIActions.LOAD_DROPDOWN_CATEGORIES:
        return bll.loadDropdownCategories(state, action);
      case CategoriesAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case CategoriesAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case CategoriesAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case CategoriesAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case CategoriesAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case CategoriesAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case CategoriesAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);

      // remove loader
      case CategoriesAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case CategoriesAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case CategoriesAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
