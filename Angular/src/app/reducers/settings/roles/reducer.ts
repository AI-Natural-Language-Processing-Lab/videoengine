/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { ROLEAPIAction, ROLEAPIActions, RolesBLL } from "./actions";
import { IRoles, ROLE_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createRoleReducer() {
  return function roleReducer(
    state: IRoles = ROLE_INITIAL_STATE,
    a: Action
  ): IRoles {
    const action = a as ROLEAPIAction;

    const bll = new RolesBLL();
    /*if (!action.meta) {
        return state;
    }*/
    switch (action.type) {
      case ROLEAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case ROLEAPIActions.LOAD_ROLE_STARTED:
        return tassign(state, { loading: true, error: null });

      case ROLEAPIActions.LOAD_ROLE_SUCCEEDED:
        return bll.loadRoleSucceeded(state, action);

      case ROLEAPIActions.LOAD_ROLE_STARTED:
        return tassign(state, { loading: false, error: action.error });

      case ROLEAPIActions.LOAD_OBJECT_STARTED:
        return tassign(state, { loading: true, error: null });

      case ROLEAPIActions.LOAD_OBJECT_SUCCEEDED:
        return bll.loadObjectSucceeded(state, action);

      case ROLEAPIActions.LOAD_OBJECT_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* add record */
      case ROLEAPIActions.ADD_ROLE:
        return bll.addRole(state, action);

      case ROLEAPIActions.ADD_OBJECT:
        return bll.addObject(state, action);

      case ROLEAPIActions.UPDATE_OBJECT:
        return bll.updateObject(state, action);

      case ROLEAPIActions.APPLY_ROLE_CHANGES:
        return bll.applyRoleChanges(state, action);

      case ROLEAPIActions.APPLY_OBJECT_CHANGES:
        return bll.applyObjectChanges(state, action);
      // remove loader
      case ROLEAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case ROLEAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);
    }

    return state;
  };
}
