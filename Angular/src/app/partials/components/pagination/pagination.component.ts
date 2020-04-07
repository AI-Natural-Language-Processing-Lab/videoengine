/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  Component,
  OnInit,
  OnChanges,
  EventEmitter,
  Input,
  Output
} from "@angular/core";
import { PaginationService } from "./pagination.service";
import { PaginationEntity, PaginationLinkEntity } from "./pagination.model";

@Component({
  selector: "pagination",
  template: `
    <nav>
      <ul class="pagination">
        <li
          *ngFor="let link of Links"
          class="page-item"
          [ngClass]="{ active: currentPage === link.id }"
        >
          <a
            class="page-link"
            *ngIf="link.name == 'First'"
            (click)="page(link.id, $event); (false)"
            title="{{ link.tooltip }}"
            href
            ><i class="fa fa-chevron-left"></i
          ></a>
          <a
            class="page-link"
            *ngIf="link.name == 'Prev'"
            (click)="page(link.id, $event); (false)"
            title="{{ link.tooltip }}"
            href
            ><i class="fa fa-backward"></i
          ></a>
          <a
            class="page-link"
            *ngIf="
              link.name != 'First' &&
              link.name != 'Prev' &&
              link.name != 'Next' &&
              link.name != 'Last'
            "
            (click)="page(link.id, $event); (false)"
            title="{{ link.tooltip }}"
            href
            >{{ link.name }}</a
          >
          <a
            class="page-link"
            *ngIf="link.name == 'Next'"
            (click)="page(link.id, $event); (false)"
            title="{{ link.tooltip }}"
            href
            ><i class="fa fa-chevron-right"></i
          ></a>
          <a
            class="page-link"
            *ngIf="link.name == 'Last'"
            (click)="page(link.id, $event); (false)"
            title="{{ link.tooltip }}"
            href
            ><i class="fa fa-forward"></i
          ></a>
        </li>
      </ul>
    </nav>
  `
})
export class PaginationComponent implements OnInit, OnChanges {
  @Input() currentPage = 1;
  @Input() totalRecords = 0;
  @Input() pageSize = 20;
  @Input() showFirst = 1;
  @Input() showLast = 1;
  @Input() paginationstyle = 0;
  @Input() totalLinks = 7;
  @Input() prevCss = "";
  @Input() nextCss = "";
  @Input() urlpath = "";

  // @Input() Options: PaginationEntity;
  @Output() OnSelection = new EventEmitter<number>();

  // OldOptions: PaginationEntity;

  Links: PaginationLinkEntity[] = [];

  ngOnChanges() {
    this.refreshPagination();
  }

  /*ngDoCheck() {

    if(typeof this.OldOptions != "undefined") {

       if (this.OldOptions.currentPage !== this.Options.currentPage) {
         console.log("Object Changed");
       }
    }

  }*/

  ngOnInit() {
    this.refreshPagination();
  }

  refreshPagination() {
    const options: PaginationEntity = {
      currentPage: this.currentPage,
      totalRecords: this.totalRecords,
      pageSize: this.pageSize,
      showFirst: this.showFirst,
      showLast: this.showLast,
      paginationstyle: this.paginationstyle,
      totalLinks: this.totalLinks,
      prevCss: this.prevCss,
      nextCss: this.nextCss,
      urlpath: this.urlpath
    };

    const pagination = new PaginationService();
    this.Links = pagination.ProcessPagination(options);
  }

  page(id, event) {
    this.OnSelection.emit(id);
    event.stopPropagation();
  }
}
