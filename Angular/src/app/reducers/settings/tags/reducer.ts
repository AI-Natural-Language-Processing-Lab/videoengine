
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { TagsAPIAction, TagsAPIActions, TagsBLL } from "./actions";
import { ITagState, TAGS_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createTagsReducer() {
  return function tagsReducer(
    state: ITagState = TAGS_INITIAL_STATE,
    a: Action
  ): ITagState {
    const action = a as TagsAPIAction;

    const bll = new TagsBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case TagsAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case TagsAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case TagsAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case TagsAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case TagsAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case TagsAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case TagsAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case TagsAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case TagsAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case TagsAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case TagsAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);
      // remove loader
      case TagsAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case TagsAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case TagsAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
