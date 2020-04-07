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
export type CoreAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class CoreAPIActions {
  static readonly SHOWMESSAGE = "SHOWMESSAGE";
  static readonly NOTIFY = "NOTIFY";
  static readonly AUTH_FAILED = "NOTIFY_AUTH_FAILED";
  static readonly REFRESHLISTSTATUS = "REFRESHLISTSTATUS";
  static readonly TRIGGER_EVENT = "CORE_TRIGGER_EVENT";
  static readonly RESETSTATS = "RESET_STATS";
  static readonly GLOBAL_LOADER = "CORE_GLOBAL_LOADER";


  @dispatch()
  toggleLoader = (payload: Payload): CoreAPIAction => ({
    type: CoreAPIActions.GLOBAL_LOADER,
    // meta: { },
    payload
  });

  @dispatch()
  showMessage = (payload: Payload): CoreAPIAction => ({
    type: CoreAPIActions.SHOWMESSAGE,
    // meta: { },
    payload
  });

  @dispatch()
  Notify = (payload: Payload): CoreAPIAction => ({
    type: CoreAPIActions.NOTIFY,
    // meta: { },
    payload
  });

  @dispatch()
  ErrorNotify = (payload: Payload): CoreAPIAction => ({
    type: CoreAPIActions.AUTH_FAILED,
    // meta: { },
    payload
  });

  @dispatch()
  refreshListStats = (payload: Payload): CoreAPIAction => ({
    type: CoreAPIActions.REFRESHLISTSTATUS,
    // meta: { },
    payload
  });

  @dispatch()
  resetStats = (): CoreAPIAction => ({
    type: CoreAPIActions.RESETSTATS,
    // meta: { },
    payload: null
  });

  @dispatch()
  triggleEvent = (payload: Payload): CoreAPIAction => ({
    type: CoreAPIActions.TRIGGER_EVENT,
    // meta: { },
    payload
  });
  
}
