/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { PaymentsComponent } from "./payments.components";
import { MyPurchaseModule } from "./mypurchases/mypurchases.module";
import { HistoryModule } from "./history/history.module";
import { PackagesModule } from "./packages/packages.module";

import { PaymentRoutingModule } from "./payments.routing.module";

@NgModule({
  imports: [
    CommonModule,
    NgbModule,
    PackagesModule,
    MyPurchaseModule,
    HistoryModule,
    PaymentRoutingModule
  ],
  declarations: [PaymentsComponent]
})
export class PaymentModule {}
