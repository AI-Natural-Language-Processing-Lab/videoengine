/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export type UserType = string;

export interface IUserPage {
  posts: any;
  auth: IAuth;
  records: number;
  loading: boolean;
  error: any;
  pagination: any;
  filteroptions: any;
  categories: any;
  selectall: boolean;
  itemsselected: boolean;
  isloaded: boolean;
}

export interface IAuth {
  isAuthenticated: boolean;
  User: any;
  Role: any;
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
  id: "",
  lockoutenabled: 2, // (0: Not Lockout, 1: Lockout, 2: all)
  emailconfirmed: 2, // (0: Not Confirmed, 1: Confirmed, 2: all)
  type: 3,
  isenabled: 2, // (0: Suspended Account, 1: Active Account, 2: all)
  email: "",
  havephoto: false,
  username: "",
  order: "created_at desc",
  accounttype: 0,
  gender: "",
  countryname: "",
  term: "", // search term
  pagesize: 24, // default page size
  pagenumber: 1, // current page number
  datefilter: 0,
  nofilter: false,
  ispublic: false,
  loadstats: true,
  track_filter: false // just to keep track whether find record or any filter option changed or called on page
};

export const USER_INITIAL_STATE: IUserPage = {
  posts: [],
  auth: {
    isAuthenticated: false,
    User: {},
    Role: []
  },
  records: 0,
  loading: false,
  error: null,
  pagination: IPagination,
  filteroptions: IFilterOption,
  categories: [],
  selectall: false,
  itemsselected: false,
  isloaded: false
};
