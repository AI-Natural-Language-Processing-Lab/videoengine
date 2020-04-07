/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

declare var require: any;
const $ = require("jquery");
const plupload = require("../../../../assets/js/plupload-2.3.6/js/plupload.full.min.js");
import {
  Component,
  Input,
  Output,
  ElementRef,
  EventEmitter,
  OnInit,
  OnChanges,
  ChangeDetectorRef
} from "@angular/core";
import { iPlUpload } from "./iPlUpload";


@Component({
  selector: "app-plupload",
  templateUrl: "./plupload.html"
})
export class PlUploadDirective implements OnInit, OnChanges {
  @Input() options: any;
  @Input() totaluploads = 0; // share stats of already uploaded files
  @Input() disposeUploader = false;

  @Output() onCompletion = new EventEmitter<any>();
  @Output() uploadProgress = new EventEmitter<any>();

  selectedFiles: any = [];
  uploadedFiles: any = [];
  ProcessCompleted = false;
  startUploading = false;
  tempfilename = "";
  message = "";
  showProgress = false;
  isInitialized = false;
  private el: ElementRef;
  uploader: any;

  constructor(el: ElementRef, private ref: ChangeDetectorRef) {
    this.el = el;
  }

  ngOnInit() {}
  
  InitializeUploader() {
    const _Options = this.options;
    let _uploadedFiles = this.uploadedFiles;
    const _OnCompletion = this.onCompletion;
    const _uploadProgress = this.uploadProgress;
    let _selectedFiles = this.selectedFiles;
    const _this = this;

    const uploader = new plupload.Uploader({
      runtimes: "html5,flash,silverlight,html4",
      browse_button: "pickfiles", // you can pass an id...
      container: "plupload_container",
      drop_element: "plupload_container", // 'FileUploadContainer',
      multi_selection: true,
      unique_names: _Options.unique_names,
      chunk_size: "8mb",
      url: _Options.handlerpath,
      flash_swf_url: "../../../../assets/js/plupload/js/Moxie.swf",
      silverlight_xap_url: "../../../../assets/js/plupload/js/Moxie.xap",
      headers: { UGID: "0", UName: _Options.username },
      filters: {
        max_file_size: _Options.maxfilesize,
        mime_types: [
          { title: _Options.extensiontitle, extensions: _Options.extensions }
        ]
      }
    });
    uploader.bind("Init", function(up, params) {});
    uploader.init();
    uploader.bind("PostInit", function() {
      $("#plupload_container").on(
        {
          click: function(e) {
            this.ProcessCompleted = false;
            this.startUploading = true;
            uploader.start();
            return false;
          }
        },
        "#uploadfiles"
      );
    });
    $(() => {
      uploader.bind("FilesAdded", (up, files) => {
        console.log("files added bind");
        if (_Options.filename !== "") {
          this.tempfilename = _Options.filename;
        }

        const _max_files = _Options.maxallowedfiles - this.totaluploads;

        _selectedFiles = files;
        _this.selectedFiles = files;
        if (_this.selectedFiles.length > _max_files) {
          _this.message = 'You can\'t upload more than ' + _Options.maxallowedfiles + ' files';
          /*$("#progress_container").html(
            "You can't upload more than " + _Options.maxallowedfiles + " files"
          );*/
          // this.message = 'You can\'t upload more than ' + _Options.maxallowedfiles + ' files';
          $.each(files, function(i, file) {
            uploader.removeFile(file);
          });
          _this.selectedFiles = [];
          $("#uploadfiles").hide();
        } else {
          _this.message = "";
          for (let i = 0; i <= this.selectedFiles.length - 1; i++) {
            this.selectedFiles[i].css = "progress-bar-danger";
            this.selectedFiles[i].percent = 0;
            $("#progress_container").append(
              '<div class="m-b-5">' +
              _this.selectedFiles[i].name +
                '</div><div class="progress"><div id="progress_' +
                _this.selectedFiles[i].id +
                '" class="progress-bar" role="progressbar" style="width: 0%;" aria-valuemin="0" aria-valuemax="100"><span id="pvalue_' +
                _this.selectedFiles[i].id +
                '">0%</span></div></div>'
            );
          }
          $("#pickfiles").hide();
          _this.ProcessCompleted = false;
          _this.startUploading = true;

          uploader.start();

          _uploadProgress.emit({
            uploadstatus: "started"
          });
        }

        up.refresh();
      });
    });

    uploader.bind("UploadProgress", function(up, file) {
      $("#progress_" + file.id).attr("style", "width: " + file.percent + "%");
      $("#pvalue_" + file.id).html(file.percent + "%");

      for (let i = 0; i <= _selectedFiles.length - 1; i++) {
        if (file.id === _selectedFiles[i].id) {
          _selectedFiles[i].percent = file.percent;
        }
      }
      /*_uploadProgress.emit({
                uploadstatus: 'progress'
            });*/
    });
    uploader.bind("Error", function(up, err) {
      console.log("error bind");
      $("#modalmsg").append(
        "<div>Error: " +
          err.code +
          ", Message: " +
          err.message +
          (err.file ? ", File: " + err.file.name : "") +
          "</div>"
      );
      _uploadProgress.emit({
        uploadstatus: "error"
      });
      up.refresh(); // Reposition Flash/Silverlight
    });
    uploader.bind("FileUploaded", function(up, file, info) {
      const rpcResponse = JSON.parse(info.response);
      // let result = '';
      _this.showProgress = false;
      if (typeof rpcResponse !== "undefined" && rpcResponse.result === "OK") {
        _uploadedFiles.push(rpcResponse);

        $("#progress_" + file.id).addClass("bg-success");
        for (let i = 0; i <= _selectedFiles.length - 1; i++) {
          if (file.id === _selectedFiles[i].id) {
            _selectedFiles[i].percent = 100;
            _selectedFiles[i].css = "progress-bar-success";
          }
        }
        if (_selectedFiles.length === _uploadedFiles.length) {
          this.ProcessCompleted = true;
          $("#pickfiles").show();
          console.log("uploaded files");
          console.log(_uploadedFiles);

          _OnCompletion.emit(_uploadedFiles);
          // cleanup progress data once submitted
          $("#progress_container").html("");
          // reset
          this.startUploading = false;
          this.fileUploaded = false;
          _selectedFiles = [];
          _uploadedFiles = [];
        }
      } else {
        
        let code;
        let message;
        if (typeof rpcResponse.error !== "undefined") {
          code = rpcResponse.error.code;
          message = rpcResponse.error.message;
          if (message === undefined || message === "") {
            message = rpcResponse.error.data;
          }
        } else {
          code = 0;
          message = "Error uploading the file to the server";
        }
        uploader.trigger("Error", {
          code: code,
          message: message,
          file: ""
        });
      }
      _this.ref.detectChanges();
    });
  }

  ngOnChanges() {
    if (!this.isInitialized) {
      this.InitializeUploader();
      this.isInitialized = true;
    }
    /*if(this.disposeUploader) {
           this.uploader.destroy();
           //this.el.nativeElement.querySelector('some-elem').destroy();
           this.disposeUploader = false;
        }*/
  }
}
