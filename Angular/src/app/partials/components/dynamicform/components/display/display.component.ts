/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import {
  Component,
  Input,
  Output,
  OnInit,
  OnChanges,
  EventEmitter
} from "@angular/core";
@Component({
  selector: "app-file-display",
  templateUrl: "./display.html",
  providers: []
})
export class FileDisplayComponent implements OnInit, OnChanges {
  @Input() files: any = [];
  @Input() showFileName = true;
  @Input() showoriginalSize = false;
  @Input() avatormode = false;
  @Input() colcss = "col-lg-4 col-md-6 col-sm-12";
  @Input() photouploader = false;

  fileType = "file"; // image | video | file | audio
  @Output() onRemove = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {
    console.log("org size is " + this.showoriginalSize);
  }

  ngOnChanges() {
    this.initializeFileType();
  }

  initializeFileType() {
    if (this.files.length > 0) {
      const file = this.files[0]; //  pick one
      switch (file.filetype) {
        case ".jpg":
        case ".jpeg":
        case ".png":
          this.fileType = "image";
          break;
        case ".mp4":
          this.fileType = "video";
          break;
        case ".mp3":
          this.fileType = "audio";
          break;
      }
    }
  }

  filterList(files: any) {
    this.onRemove.emit(files);
  }
}
