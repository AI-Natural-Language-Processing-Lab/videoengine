/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, Input, Output, EventEmitter, OnInit } from "@angular/core";
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";
import { CropperViewComponent } from "./modal";
@Component({
  selector: "app-banneruploader",
  templateUrl: "./uploader.html"
})
export class BannerUploaderComponent implements OnInit {
  @Input() Info: any;
  @Input() MediaSettings: any;
  @Output() OnCropped = new EventEmitter<any>();
  selectedOption: string;
  showUploadBtn = true;
  CropOption = 0; // 0: user logo, 1: logo (e.g agency), 2: banner
  showLoader = false;
  constructor(private modalService: NgbModal) {}

  ngOnInit() {}

  save() {
    const obj = {
      id: this.Info.id,
      picturename: this.Info.cropped_picture
    };
    console.log(obj);

    this.OnCropped.emit(obj);
  }

  cancel() {
    this.showUploadBtn = true;
    // reset original photo
    this.Info.cropped_picture = this.Info.original_picture;
  }

  deleteImage(event) {
    this.Info.cropped_picture = "";
    this.Info.original_picture = "";
    const obj = {
      id: this.Info.id,
      picturename: ""
    };
    this.showLoader = true;
    event.stopPropagation();
  }

  changeListener($event): void {
    this.readThis($event.target);
    this.showUploadBtn = false;
  }

  readThis(inputValue: any): void {
    const file: File = inputValue.files[0];
    const myReader: FileReader = new FileReader();
    const _this = this;
    myReader.onloadend = function(e) {
      // you can perform an action with readed data here
      _this.Info.original_picture = myReader.result.toString();
      _this.Info.cropped_picture = myReader.result.toString();
      // _this.profile.setProfile(_this.imageForm);
      _this.editThumbnail();
    };
    myReader.readAsDataURL(file);
  }

  editThumbnail() {
    const _options: NgbModalOptions = {
      backdrop: false
    };
    const modalRef = this.modalService.open(CropperViewComponent, _options);
    modalRef.componentInstance.Info = {
      title: "Editor",
      data: this.Info,
      cropoption: this.CropOption,
      settings: this.MediaSettings,
      scroller: false
    };
    modalRef.result.then(
      result => {
        this.Info.cropped_picture = result.data.image;
        this.Info.img_url = result.data.image;
        this.save();
      },
      dismissed => {
        console.log("dismissed");
      }
    );
  }
}
