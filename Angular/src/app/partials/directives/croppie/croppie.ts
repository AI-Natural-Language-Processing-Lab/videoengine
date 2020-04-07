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
  ElementRef,
  EventEmitter,
  OnInit,
  AfterViewInit
} from "@angular/core";
// import * as Croppie from 'croppie';
declare var $: any;

@Component({
  selector: "app-croppie",
  templateUrl: "./croppie.html"
})
export class CroppieComponent implements OnInit, AfterViewInit {
  private el: ElementRef;
  basic: any;

  @Input() Image = "";
  @Input() MediaSettings: any;
  @Input() CropOption = 0; // 0: user image, 1: company logo, 2: company banner
  @Input() ShowZoomer = true;
  @Output() OnCropped = new EventEmitter<any>();
  @Output() OnCroppedCancel = new EventEmitter<any>();

  constructor(el: ElementRef) {
    this.el = el;
  }
  ngOnInit() {}

  ngAfterViewInit() {
    if (this.Image !== "") {
      if (this.CropOption === 1) {
        let width = 350;
        let height = 350;

        if (this.MediaSettings !== undefined) {
          if (
            this.MediaSettings.width !== undefined &&
            this.MediaSettings.width > 0
          ) {
            width = this.MediaSettings.width;
          }
          if (
            this.MediaSettings.height !== undefined &&
            this.MediaSettings.height > 0
          ) {
            height = this.MediaSettings.height;
          }
        }
        let boundary_width = width + 50;
        let boundary_height = height + 50;
        if (boundary_width <= 200) {
          boundary_width = 400;
        }
        if (boundary_height <= 200) {
          boundary_height = 400;
        }

        this.basic = $("#croppie-demo").croppie({
          enableExif: true,
          enableResize: false,
          enforceBoundary: true,
          enableOrientation: true,
          viewport: {
            width: width,
            height: height,
            type: "square"
          },
          boundary: {
            width: boundary_width,
            height: boundary_height
          }
        });
      } else if (this.CropOption === 0) {
        let width = 400;
        let height = 400;
        if (this.MediaSettings !== undefined) {
          if (
            this.MediaSettings.user_thumbnail_width !== undefined &&
            this.MediaSettings.user_thumbnail_width > 0
          ) {
            width = this.MediaSettings.user_thumbnail_width;
          }
          if (
            this.MediaSettings.user_thumbnail_height !== undefined &&
            this.MediaSettings.user_thumbnail_height > 0
          ) {
            height = this.MediaSettings.user_thumbnail_height;
          }
        }
        let boundary_width = width + 50;
        let boundary_height = height + 50;
        if (boundary_width <= 200) {
          boundary_width = 400;
        }
        if (boundary_height <= 200) {
          boundary_height = 400;
        }
        this.basic = $("#croppie-demo").croppie({
          enableExif: true,
          enableResize: false,
          enforceBoundary: true,
          enableOrientation: true,
          viewport: {
            width: width,
            height: height,
            type: "square"
          },
          boundary: {
            width: boundary_width,
            height: boundary_height
          }
        });
      } else if (this.CropOption === 2) {
        this.basic = $("#croppie-demo").croppie({
          enableExif: true,
          enableResize: true,
          viewport: {
            width: 1900,
            height: 300,
            type: "square"
          },
          boundary: {
            width: 2000,
            height: 400
          }
        });
      }

      this.basic.croppie("bind", {
        url: this.Image
        // points: [77, 469, 280, 739]
      });
    }
  }

  left() {
    this.basic
      .croppie("bind", {
        url: this.Image,
        degrees: "-90"
      })
      .then(function(resp) {
        console.log("rotate left");
      });
  }

  right() {
    this.basic
      .croppie("bind", {
        url: this.Image,
        degrees: "90"
      })
      .then(function(resp) {
        console.log("rotate right");
      });
  }

  crop() {
    console.log("crop button clicked");
    const componentReference = this;
    this.basic
      .croppie("result", {
        type: "canvas"
        /*size: size,
            resultSize: {
                width: 50,
                height: 50
            }*/
      })
      .then(function(resp) {
        componentReference.OnCropped.emit({ image: resp });
      });
  }

  cancel() {
    this.OnCroppedCancel.emit({});
  }
}
