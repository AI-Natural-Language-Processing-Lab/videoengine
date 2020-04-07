/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, EventEmitter, Input, Output } from "@angular/core";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

@Component({
  selector: "app-toolbar-v2",
  templateUrl: "./toolbar.html"
})
export class BootstrapToolbarComponent implements OnInit {

  @Input() Options: any; 
  @Input() isItemsSelected = false;
  @Input() isAdmin = true;

  @Output() Action = new EventEmitter<any>();
  @Output() SelectAllCard = new EventEmitter<boolean>();
  @Input() ProfileView = false;

  selectall = false;

  @select(["core", "liststats"])
  readonly liststats$: Observable<any>;

  ngOnInit() {
    // let pagination = new PaginationService(this.Options);
    // this.Links = pagination.ProcessPagination();
  }

  /* action trigger */
  toolbaraction(action, value, event) {
    console.log("clicked");
    this.Action.emit({ action, value });
    event.stopPropagation();
  }

  processChange() {
    this.SelectAllCard.emit(this.selectall);
  }
}
