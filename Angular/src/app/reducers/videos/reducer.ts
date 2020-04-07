/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { VideoAPIAction, VideoAPIActions, VideosBLL } from "./actions";
import { IVideoState, VIDEOS_INITIAL_STATE } from "./model";
import { tassign } from "tassign";
import { Action } from "redux";

export function createVideosReducer() {
  return function videosReducer(
    state: IVideoState = VIDEOS_INITIAL_STATE,
    a: Action
  ): IVideoState {
    const action = a as VideoAPIAction;

    const bll = new VideosBLL();
    /*if (!action.meta) {
      return state;
    }*/
    switch (action.type) {
      case VideoAPIActions.IS_ITEM_SELECTED:
        return tassign(state, { itemsselected: action.payload });

      case VideoAPIActions.SELECT_ALL:
        return bll.selectAll(state, action);

      case VideoAPIActions.LOAD_STARTED:
        let isloading = true;
        if (state.triggerreload) {
          isloading = false;
        }
        return tassign(state, { loading: isloading, error: null });

      case VideoAPIActions.LOAD_SUCCEEDED:
        return bll.loadSucceeded(state, action);

      case VideoAPIActions.UPDATE_TRIGGER_RELOAD:
        return tassign(state, { triggerreload: action.payload });

      case VideoAPIActions.UPDATE_USER:
        return bll.updateUserFilter(state, action);

      case VideoAPIActions.LOAD_FAILED:
        return tassign(state, { loading: false, error: action.error });

      /* update wholefilter options */
      case VideoAPIActions.UPDATE_FILTER_OPTIONS:
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, action.payload)
        });

      /* update specific filter option */
      case VideoAPIActions.APPLY_FILTER:
        return bll.applyFilterChanges(state, action);

      /* update pagination current page */
      case VideoAPIActions.UPDATE_PAGINATION_CURRENTPAGE:
        return bll.updatePagination(state, action);

      /* add record */
      case VideoAPIActions.ADD_RECORD:
        return bll.addRecord(state, action);

      /* update record state */
      case VideoAPIActions.UPDATE_RECORD:
        return bll.updateRecord(state, action);

      case VideoAPIActions.UPDATE_CATEGORIES:
        return tassign(state, { categories: action.payload });

      /* apply changes (multiple selection items e.g delete selected records or enable selected records) */
      case VideoAPIActions.APPLY_CHANGES:
        return bll.applyChanges(state, action);
      // Youtube Uploader Reducer Calls
      case VideoAPIActions.LOAD_YT_CATEGORIES:
        return tassign(state, { yt_categories: action.payload });

      case VideoAPIActions.LOAD_YT_SEARCH:
        return tassign(state, { yt_result: action.payload, loading: false });

      // remove loader
      case VideoAPIActions.LOAD_END:
        return tassign(state, { loading: false });

      // cleanup youtube search result
      case VideoAPIActions.REMOVE_YT_SEARCH:
        return tassign(state, { yt_result: [] });

      case VideoAPIActions.UPLOADED_FILES:
        return tassign(state, { uploadedfiles: action.payload });

      case VideoAPIActions.RESET_PUBLISHING:
        return tassign(state, { uploadedfiles: [] });

      case VideoAPIActions.UPDATE_VIDEO_FILE:
        return bll.updateVideoFile(state, action);

      case VideoAPIActions.REFRESH_PAGINATION:
        return bll.refreshpagination(state, action);

      case VideoAPIActions.REFRESH_DATA:
        const filterOptions = state.filteroptions;
        filterOptions.track_filter = true;
        return tassign(state, {
          filteroptions: Object.assign({}, state.filteroptions, filterOptions)
        });
    }

    return state;
  };
}
