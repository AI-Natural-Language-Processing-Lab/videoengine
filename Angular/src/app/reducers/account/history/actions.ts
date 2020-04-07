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
import { IHistory } from "./model";
import { tassign } from "tassign";

type Payload = any;
interface MetaData {}
export type HistoryAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class HistoryAPIActions {
  static readonly LOAD_STARTED = "HISTORY_LOAD_STARTED";
  static readonly LOAD_SUCCEEDED = "HISTORY_LOAD_SUCCEEDED";
  static readonly LOAD_FAILED = "HISTORY_LOAD_FAILED";

  @dispatch()
  loadStarted = (): HistoryAPIAction => ({
    type: HistoryAPIActions.LOAD_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): HistoryAPIAction => ({
    type: HistoryAPIActions.LOAD_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): HistoryAPIAction => ({
    type: HistoryAPIActions.LOAD_FAILED,
    // meta: { },
    payload: null,
    error
  });
}

export class HistoryBLL {
  loadSucceeded(state: IHistory, action: any) {
    return tassign(state, {
      posts: action.payload.history,
      loading: false,
      isloaded: true
    });
  }
}
