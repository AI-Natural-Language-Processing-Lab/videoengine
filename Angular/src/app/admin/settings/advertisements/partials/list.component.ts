/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, EventEmitter, Input, Output } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { select } from "@angular-redux/store";
import { AdvertisementAPIActions } from "../../../../reducers/settings/advertisements/actions";
import { DataService } from "../services/data.service";
import { CoreAPIActions } from "../../../../reducers/core/actions";
@Component({
  selector: "app-list",
  templateUrl: "./list.html"
})
export class ListComponent implements OnInit {
  constructor(
    private actions: AdvertisementAPIActions,
    private dataService: DataService,
    private coreActions: CoreAPIActions
  ) {}

  @Input() isActionGranded = false;

  @select(["advertisement", "posts"])
  readonly Data$: Observable<any>;

  @select(["advertisement", "loading"])
  readonly loading$: Observable<boolean>;

  @select(["advertisement", "pagination"])
  readonly pagination$: Observable<any>;

  ngOnInit() {}

  toggleEditView(item: any, event) {
    if (!item.editview) {
      item.editview = true;
    } else {
      item.editview = false;
    }
    event.stopPropagation();
  }

  _UpdateRecord(item: any) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    item.editview = false;
    this.dataService.UpdateRecord(item);
  }

  // Since we're observing an array of items, we need to set up a 'trackBy'
  // parameter so Angular doesn't tear down and rebuild the list's DOM every
  // time there's an update.
  getKey(_, item: any) {
    return item.id;
  }

  /* pagination click event */
  PaginationChange(value: number) {
    // update filter option to query database
    this.actions.applyFilter({ attr: "pagenumber", value: value });
    // update pagination current page (to hightlight selected page)
    this.actions.updatePaginationCurrentPage({ currentpage: value });
  }
}
