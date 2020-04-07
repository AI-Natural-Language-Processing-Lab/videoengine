/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { UserAPIAction, UserAPIActions, UserBLL } from "./actions";
import { IUserPage, USER_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createUsersReducer() {
  return function usersReducer(
    state: IUserPage = USER_INITIAL_STATE,
    a: Action
  ): IUserPage {
    const action = a as UserAPIAction;

    const bll = new UserBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case UserAPIActions.SIGNOUT:
        // reset
        const _auth = {
          isAuthenticated: false,
          User: {}
        };
        return tassign(state, { auth: _auth });
      case UserAPIActions.AUTHENTICATE:
        return tassign(state, { auth: action.payload });

      case UserAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case UserAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case UserAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case UserAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case UserAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case UserAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case UserAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case UserAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case UserAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case UserAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case UserAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);

      /* update categories */
      case UserAPIActions.UPDATE_CATEGORIES:
        return tassign(state, { categories: action.payload });

      /* update thumb (user auth object) */
      case UserAPIActions.UPDATE_AUTH_THUMB:
        return bll.updateAuthThumb(state, action);
      /* update thumb */
      case UserAPIActions.UPDATE_THUMB:
        return bll.updateThumb(state, action);

      // remove loader
      case UserAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case UserAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case UserAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
