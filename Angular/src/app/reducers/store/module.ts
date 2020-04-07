/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { provideReduxForms } from "@angular-redux/form";
import { NgReduxRouter, NgReduxRouterModule } from "@angular-redux/router";
import {
  DevToolsExtension,
  NgRedux,
  NgReduxModule
} from "@angular-redux/store";

// Redux ecosystem stuff.
import { FluxStandardAction } from "flux-standard-action";
import { createLogger } from "redux-logger";
import { createEpicMiddleware } from "redux-observable";

// The top-level reducers and epics that make up our app's logic.

import { AppState, initialAppState } from "./model";
import { rootReducer } from "./reducers";

@NgModule({
  imports: [NgReduxModule, NgReduxRouterModule.forRoot()],
  providers: []
})
export class StoreModule {
  constructor(
    public store: NgRedux<AppState>,
    devTools: DevToolsExtension,
    ngReduxRouter: NgReduxRouter
  ) {
    // Tell Redux about our reducers and epics. If the Redux DevTools
    // chrome extension is available in the browser, tell Redux about
    // it too.
    /*const epicMiddleware = createEpicMiddleware<
      FluxStandardAction<any, any>,
      FluxStandardAction<any, any>,
      AppState
    >();*/

    store.configureStore(
      rootReducer,
      initialAppState(),
      [createLogger()],
      // configure store typings conflict with devTools typings
      (devTools.isEnabled() ? [devTools.enhancer()] : []) as any
    );

    // Enable syncing of Angular router state with our Redux store.
    if (ngReduxRouter) {
      ngReduxRouter.initialize();
    }

    // Enable syncing of Angular form state with our Redux store.
    provideReduxForms(store);
  }
}
