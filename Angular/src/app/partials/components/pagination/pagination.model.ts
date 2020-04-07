/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface PaginationEntity {
  currentPage: number;
  totalRecords: number;
  pageSize: number;
  showFirst: number;
  showLast: number;
  paginationstyle: number;
  totalLinks: number;
  prevCss: string;
  nextCss: string;
  urlpath: string;
}

export interface PaginationLinkEntity {
  id: string;
  url: string;
  name: string;
  tooltip: string;
  css: string;
}
