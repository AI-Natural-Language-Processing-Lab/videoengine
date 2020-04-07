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

import { IRoles } from "./model";
// Flux-standard-action gives us stronger typing of our actions.
type Payload = any;
interface MetaData {}
export type ROLEAPIAction = FluxStandardAction<Payload, MetaData>;

@Injectable()
export class ROLEAPIActions {
  static readonly LOAD_ROLE_STARTED = "ROLE_LOAD_STARTED";
  static readonly LOAD_ROLE_SUCCEEDED = "ROLE_LOAD_SUCCEEDED";
  static readonly LOAD_ROLE_FAILED = "ROLE_LOAD_FAILED";

  static readonly LOAD_OBJECT_STARTED = "ROLE_OBJECT_LOAD_STARTED";
  static readonly LOAD_OBJECT_SUCCEEDED = "ROLE_OBJECT_LOAD_SUCCEEDED";
  static readonly LOAD_OBJECT_FAILED = "ROLE_OBJECT_LOAD_FAILED";

  static readonly SELECT_ALL = "ROLE_SELECT_ALL";
  static readonly IS_ITEM_SELECTED = "ROLE_IS_ITEM_SELECTED";

  static readonly ADD_ROLE = "ROLE_ADD_RECORD";
  static readonly REMOVE_ROLE = "ROLE_REMOVE_RECORD";

  static readonly ADD_OBJECT = "ROLE_ADD_OBJECT";
  static readonly UPDATE_OBJECT = "ROLE_UPDATE_RECORD";
  static readonly REMOVE_OBJECT = "ROLE_REMOVE_OBJECT";

  static readonly APPLY_ROLE_CHANGES = "ROLE_APPLY_ROLE_CHANGES";
  static readonly APPLY_OBJECT_CHANGES = "ROLE_APPLY_OBJECT_CHANGES";

  // REFERESH LOAD
  static readonly LOAD_END = "ROLE_YT_LOADEND";
  static readonly REFRESH_DATA = "ROLE_REFRESH_DATA";
  static readonly REFRESH_PAGINATION = "ROLE_REFRESH_PAGINATION";

  @dispatch()
  loadStarted = (): ROLEAPIAction => ({
    type: ROLEAPIActions.LOAD_ROLE_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadSucceeded = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.LOAD_ROLE_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadFailed = (error): ROLEAPIAction => ({
    type: ROLEAPIActions.LOAD_ROLE_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  loadObjectStarted = (): ROLEAPIAction => ({
    type: ROLEAPIActions.LOAD_OBJECT_STARTED,
    // meta: { },
    payload: null
  });

  @dispatch()
  loadObjectSucceeded = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.LOAD_OBJECT_SUCCEEDED,
    // meta: { },
    payload
  });

  @dispatch()
  loadObjectFailed = (error): ROLEAPIAction => ({
    type: ROLEAPIActions.LOAD_OBJECT_FAILED,
    // meta: { },
    payload: null,
    error
  });

  @dispatch()
  selectAll = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.SELECT_ALL,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateItemsSelectionStatus = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.IS_ITEM_SELECTED,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addRole = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.ADD_ROLE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  removeRole = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.REMOVE_ROLE,
    // meta: { },
    payload: payload
  });

  @dispatch()
  addObject = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.ADD_OBJECT,
    // meta: { },
    payload: payload
  });

  @dispatch()
  updateObject = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.UPDATE_OBJECT,
    // meta: { },
    payload: payload
  });

  @dispatch()
  removeObject = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.REMOVE_OBJECT,
    // meta: { },
    payload: payload
  });

  @dispatch()
  applyRoleChanges = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.APPLY_ROLE_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  applyObjectChanges = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.APPLY_OBJECT_CHANGES,
    // meta: { },
    payload
  });

  @dispatch()
  loadEnd = (): ROLEAPIAction => ({
    type: ROLEAPIActions.LOAD_END,
    // meta: { },
    payload: null
  });

  @dispatch()
  reloadList = (): ROLEAPIAction => ({
    type: ROLEAPIActions.REFRESH_DATA,
    // meta: { },
    payload: null
  });

  @dispatch()
  refresh_pagination = (payload: Payload): ROLEAPIAction => ({
    type: ROLEAPIActions.REFRESH_PAGINATION,
    // meta: { },
    payload: payload
  });
}

export class RolesBLL {
  loadRoleSucceeded(state: IRoles, action: any) {
    return tassign(state, {
      roles: action.payload.posts,
      role_records: action.payload.records,
      loading: false,
      isroleloaded: true
    });
  }

  loadObjectSucceeded(state: IRoles, action: any) {
    return tassign(state, {
      objects: action.payload.posts,
      object_records: action.payload.records,
      loading: false,
      isroleloaded: true
    });
  }

  addRole(state: IRoles, action: any) {
    const posts = state.roles;
    posts.unshift(action.payload);
    return tassign(state, { roles: posts });
  }

  addObject(state: IRoles, action: any) {
    const posts = state.objects;
    posts.unshift(action.payload);
    return tassign(state, { objects: posts });
  }

  updateObject(state: IRoles, action: any) {
    const posts = state.objects;
    for (let post of posts) {
      if (post.od === action.payload.id) {
        post = Object.assign({}, post, action.payload);
      }
    }
    return tassign(state, { objects: Object.assign([], state.objects, posts) });
  }

  applyRoleChanges(state: IRoles, action: any) {
    const _updated_state = state.roles;
    for (const selected of action.payload.SelectedItems) {
      for (const item of _updated_state) {
        if (item.id === selected.id) {
          if (selected.actionstatus === "delete") {
            item.isdeleted = true;
          } else {
            for (const prop in item) {
              if (prop === action.payload.isenabled.attr) {
                item[prop] = action.payload.isenabled.value;
              }
            }
          }
        }
      }
    }
    return tassign(state, { roles: _updated_state });
  }
  applyObjectChanges(state: IRoles, action: any) {
    const _updated_state = state.objects;
    for (const selected of action.payload.SelectedItems) {
      for (const item of _updated_state) {
        if (item.id === selected.id) {
          if (selected.actionstatus === "delete") {
            item.isdeleted = true;
          } else {
            for (const prop in item) {
              if (prop === action.payload.isenabled.attr) {
                item[prop] = action.payload.isenabled.value;
              }
            }
          }
        }
      }
    }
    return tassign(state, { objects: _updated_state });
  }

  refreshpagination(state: IRoles, action: any) {
    const pagination = state.pagination;
    pagination.totalRecords = action.payload.totalrecords;
    pagination.pageSize = action.payload.pagesize;
    return tassign(state, { pagination: pagination });
    // return tassign(state, { pagination: Object.assign({}, state.pagination, pagination) });
  }
}
