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
import { UserAPIActions } from "../../../reducers/users/actions";
import { DataService } from "../services/data.service";
import { Router } from "@angular/router";
import { CoreAPIActions } from "../../../reducers/core/actions";

@Component({
  selector: "app-userlist",
  templateUrl: "./list.component.html",
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
export class UListComponent implements OnInit {
  constructor(
    private actions: UserAPIActions,
    private dataService: DataService,
    private coreActions: CoreAPIActions,
    private router: Router
  ) {}

  @Input() isActionGranded = false;
  @Input() PublicView = false; 
  @Input() isAdmin = true;
  @Input() route_path = "/users/";
  @Input() NoRecordText = "No Users Registered Yet!";

  @select(["users", "posts"])
  readonly Data$: Observable<any>;

  @select(["users", "loading"])
  readonly loading$: Observable<boolean>;

  @select(["users", "pagination"])
  readonly pagination$: Observable<any>;

  @select(["users", "selectall"])
  readonly selectAll$: Observable<any>;

  @Output() View = new EventEmitter<any>();
  @Output() SelectedItems = new EventEmitter<any>();

  selectall = false;

  ngOnInit() {
    this.selectAll$.subscribe((selectall: boolean) => {
      this.selectall = selectall;
      this.checkChange();
    });
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

  checkChange() {
    this.Data$.subscribe(items => {
      const _items = [];
      for (const item of items) {
        if (item.Selected) {
          _items.push(item);
        }
      }
      this.SelectedItems.emit(_items);
    });
  }
}
