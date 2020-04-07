/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { ConfigAPIActions } from "./actions";
import { IConfigState, CNF_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createConfigReducer() {
  return function configReducer(
    state: IConfigState = CNF_INITIAL_STATE,
    a: Action
  ): IConfigState {
    const action = a as any;

    /*if (!action.meta) {
       return state;
    }*/
    switch (action.type) {
      case ConfigAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case ConfigAPIActions.LOAD_SUCCEEDED:
        return tassign(state, {
          configs: action.payload.configurations,
          error: null,
          loading: false
        });

      case ConfigAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      case ConfigAPIActions.GET_CONFIG:
        // action.payload should be id of config
        const configs = state.configs;
        let selected_value = "";
        for (const config of configs) {
          if (config.id === action.payload) {
            selected_value = config.value;
          }
        }
        return tassign(state, {
          selected_value: selected_value
        });
    }

    return state;
  };
}
