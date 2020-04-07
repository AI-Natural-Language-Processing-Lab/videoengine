/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */


export interface IAbuseReportStats {
  posts: any; 
  records: number; // total no of records
  loading: boolean;
  error: any;
  pagination: any; // records pagination
  filteroptions: any; // records query build options
  selectall: boolean;
  itemsselected: boolean;
  isloaded: boolean; // records list
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

// Album Query Builder Entity
export const IFilterOption = {
  id: 0,
  contentid: 0,
  type: 0,
  status: 3,
  order: "id desc",
  term: "", // search term
  pagesize: 20, // default page size
  pagenumber: 1, // current page number
  datefilter: 0,
  nofilter: false,
  ispublic: false,
  loadall: false,
  track_filter: false // just to keep track whether find record or any filter option changed or called on page
};

export const ABUSE_INITIAL_STATE: IAbuseReportStats = {
  posts: [],
  records: 0,
  loading: false,
  error: null,
  pagination: IPagination,
  filteroptions: IFilterOption,
  selectall: false,
  itemsselected: false,
  isloaded: false,
};
