
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import { Component, OnInit, Input } from "@angular/core";
@Component({
  selector: "app-loader",
  template: `
    <div class="m-b-40 m-t-40" [ngSwitch]="type">
      <app-spinner01 *ngSwitchCase="1"></app-spinner01>
      <app-spinner02 *ngSwitchCase="2"></app-spinner02>
      <app-spinner03 *ngSwitchCase="3"></app-spinner03>
      <app-spinner04 *ngSwitchCase="4"></app-spinner04>
      <app-spinner05 *ngSwitchCase="5"></app-spinner05>
      <app-spinner06 *ngSwitchCase="6"></app-spinner06>
    </div>
  `
})
export class LoaderComponent implements OnInit {
  @Input() type = 2; // options.order === undefined ? 1 : options.order;;

  ngOnInit() {
    this.type = this.type === undefined ? 2 : this.type;
  }
}
