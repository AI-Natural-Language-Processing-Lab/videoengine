/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core";
@Component({
  selector: "app-display-video",
  templateUrl: "./video.html",
  providers: []
})
export class DisplayVideoComponent implements OnInit {
  @Input() videos: any = [];
  @Output() onRemove = new EventEmitter<any>();
  constructor() {}

  ngOnInit() {}

  remove(obj, index, event) {
    if (index > -1) {
      this.videos.splice(index, 1);
      this.onRemove.emit({ files: this.videos, removedItems: obj });
    }
    event.stopPropagation();
  }
}
