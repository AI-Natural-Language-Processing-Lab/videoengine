
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IRoles {
  roles: any;
  objects: any;
  role_records: number;
  object_records: number;
  role_permissions: any;
  loading: boolean;
  error: any;
  pagination: any;
  role_filteroptions: any;
  object_fileoptions: any;
  selectall: boolean;
  itemsselected: boolean;
  isroleloaded: boolean;
  isobjectloaded: boolean;
}

export const IPagination = {
  currentPage: 1,
  totalRecords: 0,
  pageSize: 4000,
  showFirst: 1,
  showLast: 1,
  paginationstyle: 0,
  totalLinks: 7,
  prevCss: "",
  nextCss: "",
  urlpath: ""
};

export const IRoleFilterOption = {
  id: 0,
  term: "",
  order: "id desc",
  pagesize: 4000,
  pagenumber: 1
};

export const IObjectsFilterOption = {
  id: 0,
  uniqueid: "",
  term: "",
  order: "id desc",

  pagesize: 4000,
  pagenumber: 1
};

export const ROLE_INITIAL_STATE: IRoles = {
  roles: [],
  objects: [],
  role_records: 0,
  object_records: 0,
  role_permissions: [],
  loading: false,
  error: null,
  pagination: IPagination,
  role_filteroptions: IRoleFilterOption,
  object_fileoptions: IObjectsFilterOption,
  selectall: false,
  itemsselected: false,
  isroleloaded: false,
  isobjectloaded: false
};
