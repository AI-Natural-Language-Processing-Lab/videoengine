/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface ICategoriesState {
  posts: any;
  dropdown_categories: any;
  records: number;
  loading: boolean;
  error: any;
  pagination: any;
  filteroptions: any;
  selectall: boolean;
  itemsselected: boolean;
  isloaded: boolean;
}

export const IPagination = {
  currentPage: 1,
  totalRecords: 0,
  pageSize: 40,
  showFirst: 1,
  showLast: 1,
  paginationstyle: 0,
  totalLinks: 7,
  prevCss: "",
  nextCss: "",
  urlpath: ""
};

export const IFilterOption = {
  id: 0,
  type: 0, // load videos by default
  mode: 0,
  isenabled: 2,
  parentid: -1,
  order: "level asc", // don't change this
  term: "", // search term
  ispublic: false,
  pagesize: 20, // default page size
  pagenumber: 1, // current page number
  isSummary: false, // load all data,
  track_filter: false // just to keep track whether find record or any filter option changed or called on page
};

export const CATEGORIES_INITIAL_STATE: ICategoriesState = {
  posts: [],
  dropdown_categories: [], // state for saving all categories loaded for dropdowns (no pagination, no type filter)
  records: 0,
  loading: false,
  error: null,
  pagination: IPagination,
  filteroptions: IFilterOption,
  selectall: false,
  itemsselected: false,
  isloaded: false
};
