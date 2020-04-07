/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IVideoState {
  posts: any;
  sm_posts: any;
  settings: any;
  records: number;
  loading: boolean;
  error: any;
  pagination: any;
  filteroptions: any;
  categories: any; // Website Categories
  yt_categories: any; // Youtube Categories
  yt_result: any; // Youtube Search Result
  selectall: boolean;
  itemsselected: boolean;
  isloaded: boolean;
  uploadedfiles: any; // ffmpeg video uploaded files
  triggerreload: any; // enable refresing list after few seconds if enabled
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
  ispublished: 2,
  userid: "",
  isapproved: 2,
  isenabled: 2,
  isfeatured: 3,
  type: 0,
  albumid: 0,
  isprivate: 3,
  isexternal: 3,
  isadult: 2,
  tags: "",
  categories: [],
  category_ids: [], // string mode
  price: 0,
  actors: "",
  actresses: "",
  errorcode: 0,
  order: "video.created_at desc",
  term: "", // search term
  pagesize: 21, // default page size
  pagenumber: 1, // current page number
  datefilter: 0,
  isSummary: true,
  nofilter: false,
  ispublic: false,
  loadstats: true, // just for first time for loading categories,
  track_filter: false // just to keep track whether find record or any filter option changed or called on page
};

export const VIDEOS_INITIAL_STATE: IVideoState = {
  posts: [],
  sm_posts: [],
  settings: [],
  records: 0,
  loading: false,
  error: null,
  pagination: IPagination,
  filteroptions: IFilterOption,
  categories: [],
  yt_categories: [],
  yt_result: [],
  selectall: false,
  itemsselected: false,
  isloaded: false,
  uploadedfiles: [],
  triggerreload: false
};
