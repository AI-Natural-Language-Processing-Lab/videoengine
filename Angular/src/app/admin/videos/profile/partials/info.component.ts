/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, Input, OnInit } from "@angular/core";
/* modal popup */
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";
// modal popup
import { ViewComponent } from "../../../../shared/videos/components/partials/modal.component";
import { SettingsService } from "../../../../shared/videos/services/settings.service";
import { DataService } from "../../../../shared/videos/services/data.service";
import { DomSanitizer } from "@angular/platform-browser";

@Component({
  selector: "app-video-info",
  templateUrl: "./info.html"
})
export class VideoProfileInfoComponent implements OnInit {
  constructor(
    private modalService: NgbModal,
    private settingService: SettingsService,
    private dataService: DataService,
    private sanitizer: DomSanitizer
  ) {}

  @Input() Info: any = {};
  @Input() Author_FullName = "";

  ngOnInit() {
    if (this.Info.player.youtubeid !== null) {
      this.Info.player.youtubeid = this.sanitizer.bypassSecurityTrustResourceUrl(
        this.Info.player.youtubeid
      );
    }
  }
  edit() {
    this.TriggleModal();
  }

  TriggleModal() {
    const _options: NgbModalOptions = {
      backdrop: false
    };
    const title = "Edit Video Information";
    const modalRef = this.modalService.open(ViewComponent, _options);
    modalRef.componentInstance.Info = {
      title: title,
      data: this.Info,
      viewType: 1
    };
    modalRef.result.then(
      result => {
        this.Info.title = result.data.title;
        this.Info.description = result.data.description;
      },
      dismissed => {
        console.log("dismissed");
      }
    );
  }

  youtubeURL() {
    console.log(this.Info.player.youtubeid);
    // return this.sanitizer.bypassSecurityTrustResourceUrl(this.Info.player.youtubeid);
  }
}
