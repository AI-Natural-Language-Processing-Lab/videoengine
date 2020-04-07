/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, EventEmitter, Input, Output } from "@angular/core";
import {
  trigger,
  style,
  transition,
  animate,
  keyframes
} from "@angular/animations";
import { Observable } from "rxjs/Observable";
import { select } from "@angular-redux/store";
import { LanguageAPIActions } from "../../../../reducers/settings/language/actions";
import { DataService } from "../services/data.service";
import { CoreAPIActions } from "../../../../reducers/core/actions";

@Component({
  selector: "app-list",
  templateUrl: "./list.html",
  animations: [
    trigger("fadeIn", [
      transition(":enter", [
        style({ opacity: "0", background: "#f5fb98" }),
        animate(
          "300ms ease-out",
          keyframes([
            style({ opacity: 0, transform: "translateY(-75%)", offset: 0 }),
            style({ opacity: 0.5, transform: "translateY(35px)", offset: 0.5 }),
            style({ opacity: 1, transform: "translateY(0)", offset: 1.0 })
          ])
        )
      ])
    ])
  ]
})
export class ListComponent implements OnInit {
  constructor(
    private actions: LanguageAPIActions,
    private coreActions: CoreAPIActions,
    private dataService: DataService
  ) {}

  @Input() isActionGranded = false;
  @select(["language", "posts"])
  readonly Data$: Observable<any>;

  @select(["language", "loading"])
  readonly loading$: Observable<boolean>;

  @select(["language", "pagination"])
  readonly pagination$: Observable<any>;

  @select(["language", "selectall"])
  readonly selectAll$: Observable<any>;

  @Output() View = new EventEmitter<any>();
  @Output() SelectedItems = new EventEmitter<any>();

  sortedFields: any = {
    culturename: {
      sort: "culturename",
      direction: "desc"
    },
    region: {
      sort: "region",
      direction: "desc"
    }
  };
  selectall = false;
  fieldstates = {
    culturename: false,
    region: false
  };

  ngOnInit() {
    this.selectAll$.subscribe((selectall: boolean) => {
      this.selectall = selectall;
    });
  }
  Sort(field: string) {
    if (this.sortedFields[field] === undefined) {
      this.sortedFields[field] = {
        sort: field,
        direction: "desc"
      };
    } else {
      if (this.sortedFields[field].direction === "desc") {
        this.sortedFields[field].direction = "asc";
      } else {
        this.sortedFields[field].direction = "desc";
      }
    }

    for (const st in this.fieldstates) {
      if (st === field) {
        this.fieldstates[st] = true;
      } else {
        this.fieldstates[st] = false;
      }
    }
    console.log(this.fieldstates);
    // update filter
    this.actions.applyFilter({
      attr: "order",
      value:
        this.sortedFields[field].sort + " " + this.sortedFields[field].direction
    });
  }
  /* action trigger */
  triggleAction(obj, action) {
    this.View.emit({ action: action, value: obj });
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
