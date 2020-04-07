/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface ITagState {
  posts: any;
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
  type: 0,
  isenabled: 2,
  order: "id desc",
  ispublic: false,
  tag_level: 100,
  tag_type: 2,
  term: "", // search term
  pagesize: 20, // default page size
  pagenumber: 1, // current page number
  isSummary: false,
  track_filter: false // just to keep track whether find record or any filter option changed or called on page
};

export const TAGS_INITIAL_STATE: ITagState = {
  posts: [],
  records: 0,
  loading: false,
  error: null,
  pagination: IPagination,
  filteroptions: IFilterOption,
  selectall: false,
  itemsselected: false,
  isloaded: false
};
