/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  templateUrl: "./modal.html"
})
export class CropperViewComponent implements OnInit {
  @Input() Info: any;
  title: string;
  data: any;
  CropOption = 0; // 0: user logo, 1: logo (e.g agency), 2: banner
  constructor(public activeModal: NgbActiveModal) {}

  ngOnInit() {
    this.title = this.Info.title;
  }

  croppedImage(data: any) {
    this.activeModal.close({
      data: data
    });
    console.log('close modal');
  }

  close(event: any) {
    this.activeModal.dismiss("Cancel Clicked");
  }
}
