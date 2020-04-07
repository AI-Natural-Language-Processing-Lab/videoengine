/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  ConfigurationsAPIAction,
  ConfigurationsAPIActions,
  ConfigurationsBLL
} from "./actions";
import { IConfigurationState, CONFIG_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createConfigurationsReducer() {
  return function configurationsReducer(
    state: IConfigurationState = CONFIG_INITIAL_STATE,
    a: Action
  ): IConfigurationState {
    const action = a as ConfigurationsAPIAction;

    const bll = new ConfigurationsBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case ConfigurationsAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case ConfigurationsAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case ConfigurationsAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });
    }

    return state;
  };
}
