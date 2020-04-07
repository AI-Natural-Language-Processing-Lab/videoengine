
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import { Component, Input, OnInit } from "@angular/core";
@Component({
  selector: "app-display-audio",
  templateUrl: "./audio.html",
  providers: []
})
export class DisplayAudioComponent implements OnInit {
  @Input() audioFiles: any = [];

  constructor() {}

  ngOnInit() {}
}
