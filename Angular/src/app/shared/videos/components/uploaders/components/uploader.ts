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
  EventEmitter,
  OnChanges
} from "@angular/core";
import { AppConfig } from "../../../../../configs/app.config";

@Component({
  selector: "app-uploader",
  templateUrl: "./uploader.html"
})
export class VideoUploadComponent implements OnChanges {
  constructor(public config: AppConfig) {}

  @Input() uploadedFiles = [];
  @Input() extensions = "mp4,avi,wmv,mpg,mpeg,webm,flv,ogv";
  @Input() filesize = "1000mb"
  @Input() max_concurrent_uploads = 5;
  @Input() Info: any = {};
  @Output() OnUploaded = new EventEmitter<any>();
  @Output() onRemove = new EventEmitter<any>();

  InitUploader = false;
  uploadoptions = {
    handlerpath: this.config.getConfig("host") + "api/ffmpeg/upload",
    pickfilecaption: "Select Videos",
    pickbuttoncss: "btn btn-lg btn-success text-center",
    uploadfilecaption: "Start Uploading",
    maxfilesize: this.filesize,
    chunksize: "8mb",
    headers: {},
    extensiontitle: "Video Files",
    extensions: this.extensions,
    filepath: "",
    username: "",
    removehandler: "",
    maxallowedfiles: this.max_concurrent_uploads,
    showFileName: false, // show filename with media file
    showoriginalSize: false, // show media in original size
    avatormode: false, // if enabled it will allow single upload and replace existing photo with uploaded photo
    value: []
  };

  ngOnChanges() {
    this.uploadoptions.username = this.Info.id;
    if (this.uploadoptions.username !== undefined) {
      this.InitUploader = true;
    }
  }

  /*remove(obj, index, event) {
        if (index > -1) {
          this.uploadedFiles.splice(index, 1);
          this.onRemove.emit({ files: this.uploadedFiles, removedItems: obj });
        }
        event.stopPropagation();
    }*/

  filesUploaded(files: any) {
    //  this.uploadedFiles = []; // need to replace existing image with uploaded image
    console.log("uploaded files");
    console.log(files);
    /*const mediafiles = [];
        for (const file of files) {
            files.push(file);
        }*/
    this.OnUploaded.emit(files);
  }
}
