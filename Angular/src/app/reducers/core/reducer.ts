/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { CoreAPIActions } from "./actions";
import { ICoreState, CORE_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createCoreReducer() {
  return function usersReducer(
    state: ICoreState = CORE_INITIAL_STATE,
    a: Action
  ): ICoreState {
    const action = a as any;

    /*if (!action.meta) {
       return state;
    }*/
    switch (action.type) {
     
      case CoreAPIActions.GLOBAL_LOADER:
        return tassign(state, {
          loader: action.payload
        });
      case CoreAPIActions.SHOWMESSAGE:
        return tassign(state, {
          message: action.payload
        });
      case CoreAPIActions.NOTIFY:
        return tassign(state, {
          notify: {
            title: action.payload.title,
            text: action.payload.text,
            css: action.payload.css
          }
        });
      case CoreAPIActions.AUTH_FAILED:
        return tassign(state, {
          auth_failed: {
            title: action.payload.title
          }
        });

      case CoreAPIActions.REFRESHLISTSTATUS:
        const obj = {
          first_boundary: 1,
          last_boundary: action.payload.pagesize,
          totalrecords: action.payload.totalrecords
        };
        if (action.payload.totalrecords > 0) {
          obj.first_boundary =
            (action.payload.pagenumber - 1) * action.payload.pagesize + 1;
        }

        obj.last_boundary =
          (action.payload.pagenumber - 1) * action.payload.pagesize +
          action.payload.pagesize;

        if (obj.last_boundary > action.payload.totalrecords) {
          obj.last_boundary = action.payload.totalrecords;
        }

        return tassign(state, { liststats: obj });

      case CoreAPIActions.RESETSTATS:
        const init_obj = {
          first_boundary: 0,
          last_boundary: 0,
          totalrecords: 0
        };

        return tassign(state, {
          liststats: Object.assign({}, state.liststats, init_obj)
        });
      case CoreAPIActions.TRIGGER_EVENT:
        return tassign(state, {
          event: Object.assign({}, state.event, action.payload)
        });
    }

    return state;
  };
}
