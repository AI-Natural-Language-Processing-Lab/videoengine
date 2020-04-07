/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  MailTemplatesAPIAction,
  MailTemplatesAPIActions,
  MailTemplatesBLL
} from "./actions";
import { IMailtemplateStates, MAIL_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createMailTemplateReducer() {
  return function mailtemplateReducer(
    state: IMailtemplateStates = MAIL_INITIAL_STATE,
    a: Action
  ): IMailtemplateStates {
    const action = a as MailTemplatesAPIAction;

    const bll = new MailTemplatesBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case MailTemplatesAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case MailTemplatesAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case MailTemplatesAPIActions.LOAD_STARTED:
        return tassign(state, { loading: true, error: null });

      case MailTemplatesAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case MailTemplatesAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case MailTemplatesAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case MailTemplatesAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case MailTemplatesAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case MailTemplatesAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case MailTemplatesAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case MailTemplatesAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);
      // remove loader
      case MailTemplatesAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      case MailTemplatesAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case MailTemplatesAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
