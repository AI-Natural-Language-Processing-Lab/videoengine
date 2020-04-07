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
import { VideoAPIActions } from "../../../../reducers/videos/actions";
import { DataService } from "../../services/data.service";
import { Router } from "@angular/router";
import { CoreAPIActions } from "../../../../reducers/core/actions";

@Component({
  selector: "app-video-list",
  templateUrl: "./list.html"
})
export class ListComponent implements OnInit {
  constructor(
    private actions: VideoAPIActions,
    private coreActions: CoreAPIActions,
    private dataService: DataService,
    private router: Router
  ) {}

  @Input() route_path = '/videos/';
  @Input() PublicView = false;
  @Input() isActionGranded = false;
  @Input() isAdmin = true;
  @Input() type = 0; // 0: My Videos, 1: Favorited Videos, 2: Liked Videos, 3: Playlist Videos
  @Input() NoRecordText = "Not Videos Uploaded Yet!";
  @Input() showReportLink = false;

  // Content Type for Abuse Reporting (Videos) => api ref (AbuseReport.Types)
  AbuseContentType = 0 ;

  @select(["videos", "posts"])
  readonly Data$: Observable<any>;

  @select(["videos", "loading"])
  readonly loading$: Observable<boolean>;

  @select(["videos", "pagination"])
  readonly pagination$: Observable<any>;

  @select(["videos", "selectall"])
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

  /*delete(item: any, index: number, event) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    const r = confirm("Are you sure you want to delete selected record?");
    if (r === true) {
      this.dataService.DeleteRecord(item, index, this.type);
    }
  } */
  
  processChange() {
    this.actions.selectAll(this.selectall);
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
