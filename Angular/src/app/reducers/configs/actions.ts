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

type Payload = any;
interface MetaData {}
export type ConfigAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class ConfigAPIActions {
  static readonly LOAD_STARTED = "CONFIG_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "CONFIG_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "CONFIG_LOAD_FAILED";
  static readonly GET_CONFIG = "GET_CONFIGURATION_VALUE";

  @dispatch()
  loadStarted = (): ConfigAPIAction => ({
    type: ConfigAPIActions.LOAD_STARTED,
    //  meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): ConfigAPIAction => ({
    type: ConfigAPIActions.LOAD_SUCCEEDED,
    //  meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): ConfigAPIAction => ({
    type: ConfigAPIActions.LOAD_FAILED,
    //  meta: { },
    payload: null,
    error
  });

  @dispatch()
  getConfig = (payload: Payload): ConfigAPIAction => ({
    type: ConfigAPIActions.GET_CONFIG,
    //  meta: { },
    payload
  });
}
