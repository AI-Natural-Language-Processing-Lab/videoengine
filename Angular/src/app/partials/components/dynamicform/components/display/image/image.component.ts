/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core";

@Component({
  selector: "app-display-image",
  templateUrl: "./image.html",
  providers: []
})
export class DisplayImageComponent implements OnInit {
  @Input() images: any = [];
  @Input() showFileName = true;
  @Input() showoriginalSize = false;
  @Output() onRemove = new EventEmitter<any>();
  @Input() cropImage = false;
  @Input() avatormode = false;
  @Input() colcss = "col-md-6 col-sm-12";
  @Input() photouploader = false;
  data: any;
  constructor() {}

  ngOnInit() {}

  remove(obj, index, event) {
    if (index > -1) {
      this.images.splice(index, 1);
      this.onRemove.emit({ files: this.images, removedItems: obj });
    }
    event.stopPropagation();
  }
}
