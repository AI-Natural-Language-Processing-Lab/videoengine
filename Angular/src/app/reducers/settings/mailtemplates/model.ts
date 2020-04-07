/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IMailtemplateStates {
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
  term: "",
  templatekey: "",
  type: "-1",
  order: "id desc",
  pagesize: 20,
  pagenumber: 1,
  track_filter: false // just to keep track whether find record or any filter option changed or called on page
};

export const MAIL_INITIAL_STATE: IMailtemplateStates = {
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
