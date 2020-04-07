/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { PaginationEntity, PaginationLinkEntity } from "./pagination.model";
// import { each } from "lodash";

@Injectable()
export class PaginationService {
  currentPage = 0;
  totalRecords = 0;
  pageSize = 20;
  showFirst = 1;
  showLast = 1;
  paginationstyle = 0;
  totalLinks = 7;
  prevCss = "previous";
  nextCss = "next";
  urlpath = "";
  totalPages = 0;

  PaginationLinks: PaginationLinkEntity[] = [];

  constructor() {
  }

  ProcessPagination(entity: PaginationEntity): PaginationLinkEntity[] {
    // initialize values
    this.currentPage = entity.currentPage;
    this.totalRecords = entity.totalRecords;
    this.pageSize = entity.pageSize;
    this.showFirst = entity.showFirst;
    this.showLast = entity.showLast;
    this.paginationstyle = entity.paginationstyle;
    this.totalLinks = entity.totalLinks;
    if (entity.prevCss) this.prevCss = entity.prevCss;
    if (entity.nextCss) this.nextCss = entity.nextCss;
    this.urlpath = entity.urlpath;
    
    // normal script
    let firstbound: number = 0;
    let lastbound: number = 0;
    let tooltip: string = "";
    if (this.totalRecords > this.pageSize) {
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize);

      if (this.currentPage > 1) {
        if (this.showFirst === 1 && this.paginationstyle != 2) {
          firstbound = 1;
          lastbound = firstbound + this.pageSize - 1;
          tooltip =
            "showing " +
            firstbound +
            " - " +
            lastbound +
            " records of " +
            this.totalRecords +
            " records";
          // First Link
          this.addLink(1, "#/" + this.urlpath, "First", tooltip, "first");
        }

        firstbound = (this.totalPages - 1) * this.pageSize;
        lastbound = firstbound + this.pageSize - 1;

        if (lastbound > this.totalRecords) {
          lastbound = this.totalRecords;
        }

        tooltip =
          "showing " +
          firstbound +
          " - " +
          lastbound +
          " records of " +
          this.totalRecords +
          " records";
        // Previous Link Enabled
        var pid = this.currentPage - 1;
        if (pid < 1) pid = 1;

        let prevPageCss: string = "";
        let prevIcon: string = "Prev";

        if (this.paginationstyle === 2) {
          if (this.prevCss != "") prevPageCss = this.prevCss;

          prevIcon = "&larr; Previous";
        }

        let _urlpath: string = "";
        if (this.urlpath !== "") _urlpath = this.urlpath + "/" + pid;

        this.addLink(pid, "#/" + _urlpath, prevIcon, tooltip, "previous");

        // Normal Links
        if (this.paginationstyle !== 2) this.gen_links(this.urlpath);

        if (this.currentPage < this.totalPages)
          this.set_prev_last_links(this.urlpath);
      } else {
        if (this.paginationstyle !== 2) this.gen_links(this.urlpath);

        if (this.currentPage < this.totalPages)
          this.set_prev_last_links(this.urlpath);
      }
    }
    return this.PaginationLinks;
  }

  gen_links(urlpath) {
    let firstbound: number = 0;
    let lastbound: number = 0;
    let tooltip: string = "";

    let Links: number[] = this.SimplePagination();
    if (Links.length > 0) {
      let _this = this;
      for (let link of Links) {
      // each(Links, function(link) {
        // console.log(link);
        firstbound = (link - 1) * _this.pageSize + 1;
        lastbound = firstbound + _this.pageSize - 1;
        if (lastbound > _this.totalRecords) lastbound = _this.totalRecords;

        tooltip =
          "showing " +
          firstbound +
          " - " +
          lastbound +
          " records  of " +
          _this.totalRecords +
          " records";

        let _urlpath: string = "";
        if (urlpath != "") _urlpath = urlpath + "/" + link;

        _this.addLink(link, "#/" + _urlpath, link, tooltip, "");
      }
      // });
    }
  }

  set_prev_last_links(urlpath) {
    let firstbound: number = this.currentPage * this.pageSize + 1;
    let lastbound: number = firstbound + this.pageSize - 1;

    if (lastbound > this.totalRecords) lastbound = this.totalRecords;

    let tooltip =
      "showing " +
      firstbound +
      " - " +
      lastbound +
      " records of " +
      this.totalRecords +
      " records";

    // Next Link
    var pid = this.currentPage + 1;
    if (pid > this.totalPages) pid = this.totalPages;

    let nextPageCss: string = "";
    let nextPageIcon: string = "Next";

    if (this.paginationstyle === 2) {
      if (this.nextCss != "") nextPageCss = this.nextCss;

      nextPageIcon = "Next &rarr;";
    }

    this.addLink(pid, "#/" + urlpath, nextPageIcon, tooltip, "next");

    if (this.showLast === 1 && this.paginationstyle !== 2) {
      // Last Link
      firstbound = (this.totalPages - 1) * this.pageSize + 1;
      lastbound = firstbound + this.pageSize - 1;
      if (lastbound > this.totalRecords) lastbound = this.totalRecords;

      tooltip =
        "showing " +
        firstbound +
        " - " +
        lastbound +
        " records of " +
        this.totalRecords +
        " records";

      let _urlpath: string = "";
      if (urlpath != "") _urlpath = urlpath + "/" + this.totalPages;

      this.addLink(lastbound, "#/" + _urlpath, "Last", tooltip, "last");
    }
  }

  addLink(id, url, name, tooltip, css) {
    this.PaginationLinks.push({
      id: id,
      url: url,
      name: name,
      tooltip: tooltip,
      css: css
    });
  }

  SimplePagination(): number[] {
    let arr: number[] = [];
    if (this.totalPages < this.totalLinks) {
      for (var i = 1; i <= this.totalPages; i++) {
        arr[i - 1] = i;
      }
    } else {
      let startindex: number = this.currentPage;
      let lowerbound: number = startindex - Math.floor(this.totalLinks / 2);
      let upperbound = startindex + Math.floor(this.totalLinks / 2);
      if (lowerbound < 1) {
        //calculate the difference and increment the upper bound
        upperbound = upperbound + (1 - lowerbound);
        lowerbound = 1;
      }
      //if upperbound is greater than total page is
      if (upperbound > this.totalPages) {
        //calculate the difference and decrement the lower bound
        lowerbound = lowerbound - (upperbound - this.totalPages);
        upperbound = this.totalPages;
      }
      var counter = 0;
      for (var i = lowerbound; i <= upperbound; i++) {
        arr[counter] = i;
        counter++;
      }
    }
    return arr;
  }
}
