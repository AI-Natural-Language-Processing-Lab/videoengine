/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { dispatch } from "@angular-redux/store";
import { FluxStandardAction } from "flux-standard-action";
import { tassign } from "tassign";

import { IConfigurationState } from "./model";

type Payload = any;
interface MetaData {}
export type ConfigurationsAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class ConfigurationsAPIActions {
  static readonly LOAD_STARTED = "CONFIGURATIONS_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "CONFIGURATIONS_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "CONFIGURATIONS_LOAD_FAILED";

  static readonly UPDATE_CONFIGS = "CONFIGURATIONS_UPDATE_CONFIGS";

  @dispatch()
  loadStarted = (): ConfigurationsAPIAction => ({
    type: ConfigurationsAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): ConfigurationsAPIAction => ({
    type: ConfigurationsAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): ConfigurationsAPIAction => ({
    type: ConfigurationsAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });
}

export class ConfigurationsBLL {
  loadSucceeded(state: IConfigurationState, action: any) {
    return tassign(state, {
      //posts: action.payload.settings,
      configurations: action.payload.configurations,
      loading: false
    });
  }
}
