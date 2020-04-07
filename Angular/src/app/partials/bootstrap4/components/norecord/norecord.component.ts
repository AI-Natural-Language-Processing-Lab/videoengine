/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, Input } from "@angular/core";

@Component({
  selector: "app-norecord",
  template: `
    <div class="card text-center">
      <div class="card-body">
        <h3 class="card-title m-b-30 m-t-30">{{ message }}</h3>
      </div>
    </div>
  `
})
export class NoRecordFoundComponent {
  @Input() message;
  constructor() {
    if (this.message === undefined || this.message === "") {
      this.message = "No Record Found!";
    }
  }
}
